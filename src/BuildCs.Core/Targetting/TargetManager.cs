using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildCs.Targetting
{
    public class TargetManager
    {
        private readonly Dictionary<string, ITargetBuilder> _targets;

        public TargetManager()
        {
            _targets = new Dictionary<string, ITargetBuilder>(StringComparer.InvariantCultureIgnoreCase);
        }

        public IReadOnlyCollection<ITargetBuilder> Targets
        {
            get { return _targets.Values.ToList(); }
        }

        public ITargetBuilder AddTarget(string name)
        {
            if(_targets.ContainsKey(name))
                throw new BuildCsException("The target '{0}' has already been defined.".F(name));

            var target = new Target(name);
            _targets.Add(name, target);
            return target;
        }

        public ITargetBuilder GetTarget(string name)
        {
            ITargetBuilder target;
            if (!_targets.TryGetValue(name, out target))
                throw new BuildCsException("The target '{0}' does not exist.".F(name));

            return target;
        }

        public IEnumerable<ITarget> GetBuildChain(IEnumerable<string> targetNames)
        {
            HashSet<string> targets = new HashSet<string>();
            targetNames.Each(t => GatherBuildTargets(t, targets));
            var targetsToBuild = targets.Select(x => _targets[x]).ToList();

            var nodes = targetsToBuild
                .Select(x => new Node
                {
                    Target = x,
                    RequiredBy = targetsToBuild.Where(y => y.Dependencies.Contains(x.Name)).ToList()
                })
                .ToList();

            // Need to sort the targets topologically
            var L = new List<Node>();
            var S = new Queue<Node>(nodes.Where(x => x.RequiredBy.Count == 0));
            nodes.RemoveAll(x => S.Contains(x));

            while(S.Count > 0)
            {
                var n = S.Dequeue();
                L.Add(n);

                foreach(var m in nodes.Where(x => x.RequiredBy.Contains(n.Target)).ToList())
                {
                    m.RequiredBy.Remove(n.Target);
                    if (m.RequiredBy.Count == 0)
                    {
                        S.Enqueue(m);
                        nodes.Remove(m);
                    }
                }
            }

            if (nodes.Any())
                throw new BuildCsException("A cycle exists in the dependency chain.");

            return L.Select(x => x.Target).Reverse();
        }

        public bool HasTarget(string name)
        {
            return _targets.ContainsKey(name);
        }

        private void GatherBuildTargets(string targetName, ISet<string> resolved)
        {
            ITargetBuilder target;
            if (!_targets.TryGetValue(targetName, out target))
                throw new BuildCsException("Target '{0}' has not been defined.".F(targetName));

            resolved.Add(target.Name);
            target.Dependencies
                .Except(resolved)
                .Each(d => GatherBuildTargets(d, resolved));
        }

        private class Node
        {
            public ITargetBuilder Target;
            public List<ITargetBuilder> RequiredBy;
        }

        private class Target : ITargetBuilder
        {
            private readonly string _name;
            private readonly List<Action<ITargetExecution, Action<ITargetExecution>>> _wrappers;

            private Action<ITargetExecution> _action;
            private string _description;
            private List<string> _dependencies;

            public Target(string name)
            {
                _name = name;
                _action = _ => { };
                _dependencies = new List<string>();
                _wrappers = new List<Action<ITargetExecution, Action<ITargetExecution>>>();
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

            public ITargetBuilder Describe(string description)
            {
                _description = description;
                return this;
            }

            public ITargetBuilder DependsOn(params string[] names)
            {
                _dependencies.AddRange(names);
                return this;
            }

            public ITargetBuilder Do(Action<ITargetExecution> action)
            {
                _action = action;
                return this;
            }

            public ITargetBuilder Wrap(Action<ITargetExecution, Action<ITargetExecution>> action)
            {
                _wrappers.Add(action);
                return this;
            }

            public void Run(ITargetExecution target)
            {
                _wrappers.Aggregate(_action, (prev, next) => t => next(t, prev))(target);
            }
        }
    }
}