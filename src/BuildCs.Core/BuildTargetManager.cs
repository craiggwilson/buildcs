using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public class BuildTargetManager
    {
        private readonly Dictionary<string, IBuildTargetBuilder> _targets;

        public BuildTargetManager()
        {
            _targets = new Dictionary<string, IBuildTargetBuilder>(StringComparer.InvariantCultureIgnoreCase);
        }

        public IReadOnlyCollection<IBuildTargetBuilder> Targets
        {
            get { return _targets.Values.ToList(); }
        }

        public IBuildTargetBuilder AddTarget(string name)
        {
            if(_targets.ContainsKey(name))
                throw new BuildCsException("The target '{0}' has already been defined.".F(name));

            var target = new BuildTarget(name);
            _targets.Add(name, target);
            return target;
        }

        public IBuildTargetBuilder GetTarget(string name)
        {
            IBuildTargetBuilder target;
            if (!_targets.TryGetValue(name, out target))
                throw new BuildCsException("The target '{0}' does not exist.".F(name));

            return target;
        }

        public IEnumerable<IBuildTarget> GetBuildChain(IEnumerable<string> targetNames)
        {
            HashSet<string> targets = new HashSet<string>();
            targetNames.Each(t => GatherBuildTargets(t, targets));
            var targetsToBuild = targets.Select(x => _targets[x]).ToList();

            var nodes = targetsToBuild
                .Select(x => new Node
                {
                    Name = x,
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

                foreach(var m in nodes.Where(x => x.RequiredBy.Contains(n.Name)).ToList())
                {
                    m.RequiredBy.Remove(n.Name);
                    if (m.RequiredBy.Count == 0)
                    {
                        S.Enqueue(m);
                        nodes.Remove(m);
                    }
                }
            }

            if (nodes.Any())
                throw new BuildCsException("A cycle exists in the dependency chain.");

            return L.Select(x => x.Name).Reverse();
        }

        public bool HasTarget(string name)
        {
            return _targets.ContainsKey(name);
        }

        private void GatherBuildTargets(string targetName, ISet<string> resolved)
        {
            IBuildTargetBuilder target;
            if (!_targets.TryGetValue(targetName, out target))
                throw new BuildCsException("Target '{0}' has not been defined.".F(targetName));

            resolved.Add(target.Name);
            target.Dependencies
                .Except(resolved)
                .Each(d => GatherBuildTargets(d, resolved));
        }

        private class Node
        {
            public IBuildTargetBuilder Name;
            public List<IBuildTargetBuilder> RequiredBy;
        }
    }
}