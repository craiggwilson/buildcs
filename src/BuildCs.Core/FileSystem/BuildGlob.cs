using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BuildCs.FileSystem
{
    public class BuildGlob : IEnumerable<BuildItem>
    {
        private static readonly Regex _isDriveRegex = new Regex(@"^[A-Za-z]:$", RegexOptions.Compiled);

        public static IEnumerable<BuildItem> Search(string pattern)
        {
            return Search(Path.GetFullPath("."), pattern);
        }

        public static IEnumerable<BuildItem> Search(BuildItem baseDirectory, string pattern)
        {
            var baseDir = baseDirectory.ToString();
            pattern = BuildItem.NormalizePath(pattern);
            pattern = pattern.Replace(baseDir, "");

            var filePattern = Path.GetFileName(pattern);
            return pattern.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(new List<string>(new[] { baseDir }), (acc, p) =>
                {
                    if (p == "**")
                        return acc.Concat(acc.SelectMany(dir => Directory.EnumerateDirectories(dir, "*", SearchOption.AllDirectories))).ToList();
                    else if (p == filePattern)
                        return acc.SelectMany(dir => Directory.EnumerateFiles(dir, filePattern)).ToList();
                    else if (_isDriveRegex.IsMatch(p))
                        return acc.SelectMany(dir => CheckSubDirectories(dir, p, true)).ToList();
                    else
                        return acc.SelectMany(dir => CheckSubDirectories(dir, p, false)).ToList();
                })
                .Select(f => new BuildItem(f))
                .ToList();
        }

        private static IEnumerable<string> CheckSubDirectories(string root, string directory, bool absolute)
        {
            if(directory.Contains("*"))
                return Directory.EnumerateDirectories(root, directory, SearchOption.TopDirectoryOnly);

            var di = absolute
                ? new DirectoryInfo(directory)
                : new DirectoryInfo(Path.Combine(root, directory));

            return di.Exists
                ? new[] { di.FullName }
                : Enumerable.Empty<string>();
        }

        private readonly BuildItem _baseDirectory;
        private readonly List<string> _includes;
        private readonly List<string> _excludes;

        public BuildGlob()
            : this(Path.GetFullPath("."))
        { }

        public BuildGlob(BuildItem baseDirectory)
        {
            _baseDirectory = baseDirectory;
            _includes = new List<string>();
            _excludes = new List<string>();
        }

        public BuildGlob Include(string pattern)
        {
            _includes.Add(pattern);
            return this;
        }

        public BuildGlob Exclude(string pattern)
        {
            _excludes.Insert(0, pattern);
            return this;
        }

        public IEnumerator<BuildItem> GetEnumerator()
        {
            return _includes.SelectMany(p => Search(_baseDirectory, p))
                .Except(_excludes.SelectMany(p => Search(_baseDirectory, p)))
                .Distinct()
                .ToList()
                .GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static BuildGlob operator +(BuildGlob lhs, string rhs)
        {
            return lhs.Include(rhs);
        }

        public static BuildGlob operator -(BuildGlob lhs, string rhs)
        {
            return lhs.Exclude(rhs);
        }
    }
}