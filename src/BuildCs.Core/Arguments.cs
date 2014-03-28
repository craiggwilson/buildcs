using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BuildCs.Tracing;

namespace BuildCs
{
    public class Arguments
    {
        private readonly List<string> _listeners;
        private readonly Dictionary<string, string> _parameters;
        private readonly List<string> _targetNames;

        public Arguments(IEnumerable<string> args)
        {
            _listeners = new List<string>();
            _parameters = new Dictionary<string, string>();
            _targetNames = new List<string>();
            ParseArguments(args);
        }

        public IReadOnlyList<string> Listeners
        {
            get { return _listeners; }
        }

        public IReadOnlyDictionary<string, string> Parameters
        {
            get { return _parameters; }
        }

        public IReadOnlyList<string> TargetNames
        {
            get { return _targetNames; }
        }

        public MessageLevel Verbosity { get; set; }

        public void AddListener(string listener)
        {
            _listeners.Add(listener);
        }

        public void AddParameter(string name, string value)
        {
            _parameters.Add(name, value);
        }

        public void AddTarget(string name)
        {
            _targetNames.Add(name);
        }

        private void ParseArguments(IEnumerable<string> args)
        {
            _targetNames.AddRange(args.TakeWhile(s => !s.StartsWith("-")));
            var arguments = new Queue<string>(args.SkipWhile(s => !s.StartsWith("-")));

            while(arguments.Count > 0)
            {
                var argument = arguments.Dequeue();

                var endTypeIndex = argument.IndexOf(":");
                var type = argument.Substring(1, endTypeIndex - 1);

                string name;
                string value;
                switch(type)
                {
                    case "parameter":
                    case "p":
                        var endNameIndex = argument.IndexOf("=", endTypeIndex + 1);
                        if(endNameIndex == -1)
                            throw new BuildCsException("Parameter argument is mal-formed. Correct formation is -p:<name>=<value>");

                        name = argument.Substring(endTypeIndex + 1, endNameIndex - 1);
                        value = argument.Substring(endNameIndex + 1);
                        _parameters.Add(name, value);
                        break;
                    case "verbosity":
                    case "v":
                        value = argument.Substring(endTypeIndex + 1);
                        MessageLevel level;
                        if(!Enum.TryParse<MessageLevel>(value, out level))
                            throw new BuildCsException("Unknown logLevel '{0}'.".F(value));
                        Verbosity = level;
                        break;
                    case "listener":
                        value = argument.Substring(endTypeIndex + 1);
                        _listeners.Add(value);
                        break;
                    default:
                        throw new BuildCsException("Unknown argument type: {0}".F(type));
                }
            }
        }
    }
}