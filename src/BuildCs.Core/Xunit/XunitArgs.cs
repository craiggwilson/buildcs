using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.XUnit
{
    public class XUnitArgs
    {
        private readonly List<string> _includedTraits;
        private readonly List<string> _excludedTraits;

        public XUnitArgs()
        {
            _includedTraits = new List<string>();
            _excludedTraits = new List<string>();
        }

        public IReadOnlyList<string> ExcludedTraits
        {
            get { return _excludedTraits; }
        }

        public IReadOnlyList<string> IncludedTraits
        {
            get { return _includedTraits; }
        }

        public BuildItem HtmlOutput { get; set; }

        public BuildItem NUnitXmlOutput { get; set; }

        public BuildItem XmlOutput { get; set; }

        public bool? ShadowCopy { get; set; }

        public TimeSpan? Timeout { get; set; }

        public BuildItem ToolPath { get; set; }

        public bool? Verbose { get; set; }

        public void ExcludeTrait(string trait)
        {
            _excludedTraits.Add(trait);
        }

        public void ExcludeTraits(IEnumerable<string> traits)
        {
            _excludedTraits.AddRange(traits);
        }

        public void IncludeTrait(string trait)
        {
            _includedTraits.Add(trait);
        }

        public void IncludeTraits(IEnumerable<string> traits)
        {
            _includedTraits.AddRange(traits);
        }
    }
}