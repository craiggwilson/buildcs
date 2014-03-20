using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public class BuildCommandLine
    {
        private readonly Dictionary<string, string> _parameters;
        private readonly List<string> _requestedTargetNames;

        public BuildCommandLine(IEnumerable<string> args)
        {
            _requestedTargetNames = args
                .TakeWhile(s => !s.Contains('='))
                .ToList();

            _parameters = args.SkipWhile(s => !s.Contains('='))
                .Select(s => s.Split('='))
                .ToDictionary(s => s[0], s => s[1]);
        }

        public IReadOnlyList<string> RequestedTargetNames
        {
            get { return _requestedTargetNames; }
        }

        public string GetParameter(string name)
        {
            string value;
            if (!_parameters.TryGetValue(name, out value))
                throw new BuildCsException("Parameter '{0}' was not provided.".F(name));

            return value;
        }

        public bool HasParameter(string name)
        {
            return _parameters.ContainsKey(name);
        }
    }
}