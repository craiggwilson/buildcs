using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public static class BuildRunExtensions
    {
        public static void Run(this Build build, IEnumerable<string> targets)
        {
            build.Runner.RunTargets(build.Tracer, targets);
        }

        public static void RunTargetOrDefault(this Build build, string defaultTarget)
        {
            var targetNames = build.CommandLine.RequestedTargetNames;
            if (targetNames.Count == 0)
                targetNames = new[] { defaultTarget };

            Run(build, targetNames);
        }
    }
}