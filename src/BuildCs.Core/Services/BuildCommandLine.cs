using System.Collections.Generic;
using System.Linq;

namespace BuildCs.Services
{
    public class BuildCommandLine
    {
        private readonly Dictionary<string, string> _parameters;
        private readonly List<string> _targetNames;

        public BuildCommandLine(IEnumerable<string> args)
        {
            _targetNames = args
                .TakeWhile(s => !s.Contains('='))
                .ToList();

            _parameters = args.SkipWhile(s => !s.Contains('='))
                .Select(s => s.Split('='))
                .ToDictionary(s => s[0], s => s[1]);
        }

        public IReadOnlyList<string> TargetNames
        {
            get { return _targetNames; }
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