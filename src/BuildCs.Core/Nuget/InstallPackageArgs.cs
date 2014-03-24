using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Nuget
{
    public class InstallPackageArgs : NugetArgsBase
    {
        private readonly List<string> _sources;

        public InstallPackageArgs()
        {
            _sources = new List<string>();
        }

        public bool? ExcludeVersion { get; set; }

        public bool? NonInteractive { get; set; }

        public BuildItem OutputDirectory { get; set; }

        public bool? PreRelease { get; set; }

        public string Version { get; set; }

        public IReadOnlyList<string> Sources
        {
            get { return _sources; }
        }

        public void AddSource(string source)
        {
            _sources.Add(source);
        }

        public void AddSources(IEnumerable<string> sources)
        {
            _sources.AddRange(sources);
        }
    }
}