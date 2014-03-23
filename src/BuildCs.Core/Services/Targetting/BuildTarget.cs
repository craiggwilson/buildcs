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
            _wrappers.Aggregate(_action, (prev, next) => () => next(prev))();
        }
    }
}