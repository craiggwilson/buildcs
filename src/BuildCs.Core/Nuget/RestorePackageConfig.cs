using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Nuget
{
    public class RestorePackageConfig
    {
        private readonly List<string> _sources;

        public RestorePackageConfig()
        {
            _sources = new List<string>();
        }

        public BuildItem OutputPath { get; set; }

        public IReadOnlyList<string> Sources
        {
            get { return _sources; }
        }

        public TimeSpan? Timeout { get; set; }

        public BuildItem ToolPath { get; set; }

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