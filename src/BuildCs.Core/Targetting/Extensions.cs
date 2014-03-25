using System;
using System.Linq;

namespace BuildCs.Targetting
{
    public static class Extensions
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

        public static IBuildTargetBuilder After(this IBuildTargetBuilder builder, Action after)
        {
            return builder.Wrap(next =>
            {
                next();
                after();
            });
        }

        public static IBuildTargetBuilder Before(this IBuildTargetBuilder builder, Action before)
        {
            return builder.Wrap(next =>
            {
                before();
                next();
            });
        }

        public static IBuildTargetBuilder Cleanup(this IBuildTargetBuilder builder, Action cleanup)
        {
            return builder.Wrap(next =>
            {
                try
                {
                    next();
                }
                finally
                {
                    cleanup();
                }
            });
        }

        public static IBuildTargetBuilder FailIf(this IBuildTargetBuilder builder, Func<bool> predicate, string message = null)
        {
            return builder.Wrap(next =>
            {
                if (predicate())
                    throw new BuildCsFailTargetException(message);
                next();
            });
        }

        public static IBuildTargetBuilder SkipIf(this IBuildTargetBuilder builder, Func<bool> predicate, string message = null)
        {
            return builder.Wrap(next =>
            {
                if (predicate())
                    throw new BuildCsSkipTargetException(message);
                next();
            });
        }
    }
}