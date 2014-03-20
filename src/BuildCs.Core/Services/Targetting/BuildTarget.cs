using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildCs.Services.Targetting
{
    public class BuildTarget : IBuildTargetBuilder
    {
        private readonly string _name;
        private readonly List<Action<Action>> _wrappers;

        private Action _action;
        private string _description;
        private List<string> _dependencies;

        public BuildTarget(string name)
        {
            _name = name;
            _action = () => { };
            _dependencies = new List<string>();
            _wrappers = new List<Action<Action>>();
        }

        public string Description
        {
            get { return _description; }
        }

        public string Name
        {
            get { return _name; }
        }

        public IReadOnlyList<string> Dependencies
        {
            get { return _dependencies; }
        }

        public IBuildTargetBuilder Describe(string description)
        {
            _description = description;
            return this;
        }

        public IBuildTargetBuilder DependsOn(params string[] names)
        {
            _dependencies.AddRange(names);
            return this;
        }

        public IBuildTargetBuilder Do(Action action)
        {
            _action = action;
            return this;
        }

        public IBuildTargetBuilder Wrap(Action<Action> action)
        {
            _wrappers.Add(action);
            return this;
        }

        public void Run()
        {
            var current = _action;
            foreach(var wrapper in _wrappers.Reverse<Action<Action>>())
                current = () => wrapper(current);

            current();
        }
    }

    internal class TopologicalBuildTargetComparer : IComparer<BuildTarget>
    {
        public int Compare(BuildTarget x, BuildTarget y)
        {
            if(x.Dependencies.Contains(y.Name) && y.Dependencies.Contains(x.Name))
                throw new BuildCsException("Mutual dependencies found between targets '{0}' and '{1}'.".F(x.Name, y.Name));
            if(x.Dependencies.Contains(y.Name))
                return -1;
            if (y.Dependencies.Contains(x.Name))
                return 1;

            return 0;
        }
    }
}