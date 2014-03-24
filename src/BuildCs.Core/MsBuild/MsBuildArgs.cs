using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.MsBuild
{
    public class MsBuildArgs
    {
        private readonly Dictionary<string, string> _properties;
        private readonly List<string> _targets;

        public MsBuildArgs()
        {
            _properties = new Dictionary<string, string>();
            _targets = new List<string>();
        }

        public IReadOnlyDictionary<string, string> Properties
        {
            get { return _properties; }
        }

        public IReadOnlyList<string> Targets
        {
            get { return _targets; }
        }

        public string ToolsVersion { get; set; }

        public MsBuildVerbosity? Verbosity { get; set; }

        public void AddProperty(string name, string value)
        {
            _properties.Add(name, value);
        }

        public void AddTarget(string target)
        {
            _targets.Add(target);
        }

        public void AddTargets(IEnumerable<string> targets)
        {
            _targets.AddRange(targets);
        }
    }
}