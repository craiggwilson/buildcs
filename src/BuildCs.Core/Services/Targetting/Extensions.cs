using System;
using System.Linq;
using BuildCs.Services.Targetting;

namespace BuildCs
{
    public static partial class Extensions
    {
        public static BuildTargetManager TargetManager(this Build build)
        {
            return build.GetService<BuildTargetManager>();
        }

        public static BuildTargetRunner TargetRunner(this Build build)
        {
            return build.GetService<BuildTargetRunner>();
        }

        public static void Run(this Build build, params string[] targets)
        {
            build.TargetRunner().RunTargets(targets);
        }

        public static void RunTargetOrDefault(this Build build, string defaultTarget)
        {
            var targetNames = build.CommandLine().TargetNames;
            if (targetNames.Count == 0)
                targetNames = new[] { defaultTarget };

            Run(build, targetNames.ToArray());
        }

        public static IBuildTargetBuilder Target(this Build build, string name)
        {
            return build.TargetManager().AddTarget(name);
        }

        public static IBuildTargetBuilder PreCondition(this IBuildTargetBuilder builder, Func<bool> predicate)
        {
            return builder.Wrap(a =>
            {
                if (!predicate())
                    throw new BuildCsSkipTargetException();
                a();
            });
        }
    }
}