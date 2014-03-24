using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Nuget
{
    public class PackArgs : NugetArgsBase
    {
        private readonly Dictionary<string, string> _properties;

        public PackArgs()
        {
            _properties = new Dictionary<string, string>();
        }

        public BuildItem BasePath { get; set; }

        public bool? NonInteractive { get; set; }

        public BuildItem OutputDirectory { get; set; }

        public IReadOnlyDictionary<string, string> Properties
        {
            get { return _properties; }
        }

        public bool? Symbols { get; set; }

        public string Version { get; set; }

        public void AddProperty(string name, string value)
        {
            _properties.Add(name, value);
        }
    }
}