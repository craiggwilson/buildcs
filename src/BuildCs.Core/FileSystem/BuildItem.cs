using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.FileSystem
{
    public sealed class BuildItem : IEquatable<BuildItem>
    {
        public static string NormalizePath(string path)
        {
            return path.Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);
        }

        private readonly string _path;

        public BuildItem(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Cannot be null or purely whitespace.", "path");

            _path = NormalizePath(path);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BuildItem);
        }

        public bool Equals(BuildItem other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;

            return _path == other._path;
        }

        public override int GetHashCode()
        {
            return _path.GetHashCode();
        }

        public BuildGlob Glob(string pattern)
        {
            return new BuildGlob(this).Include(pattern);
        }

        public override string ToString()
        {
            return _path;
        }

        public static implicit operator BuildItem(string path)
        {
            return new BuildItem(path);
        }

        public static implicit operator string(BuildItem item)
        {
            return item._path;
        }

        public static bool operator ==(BuildItem lhs, BuildItem rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
                return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(BuildItem lhs, BuildItem rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
                return false;

            return !lhs.Equals(rhs);
        }

        public static BuildItem operator +(BuildItem lhs, BuildItem rhs)
        {
            return CombinePaths(lhs.ToString(), rhs.ToString());
        }

        public static BuildItem operator +(BuildItem lhs, string rhs)
        {
            return CombinePaths(lhs.ToString(), rhs);
        }

        public static BuildItem operator +(string lhs, BuildItem rhs)
        {
            return CombinePaths(lhs, rhs.ToString());
        }

        private static BuildItem CombinePaths(string left, string right)
        {
            return Path.Combine(left, right.TrimStart('\\', '/'));
        }
    }
}