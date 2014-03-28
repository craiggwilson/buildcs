using System;
using System.Linq;

namespace BuildCs.Targetting
{
    public static class Extensions
    {
        public static TargetManager TargetManager(this IBuild build)
        {
            return build.GetService<TargetManager>();
        }

        public static TargetRunner TargetRunner(this IBuild build)
        {
            return build.GetService<TargetRunner>();
        }

        public static void Run(this IBuild build, params string[] targets)
        {
            build.TargetRunner().RunTargets(targets);
        }

        public static void RunTargetOrDefault(this IBuild build, string defaultTarget)
        {
            var targetNames = build.Arguments().TargetNames;
            if (targetNames.Count == 0)
                targetNames = new[] { defaultTarget };

            Run(build, targetNames.ToArray());
        }

        public static ITargetBuilder Target(this IBuild build, string name)
        {
            return build.TargetManager().AddTarget(name);
        }

        public static ITargetBuilder After(this ITargetBuilder builder, Action after)
        {
            return builder.Wrap(next =>
            {
                next();
                after();
            });
        }

        public static ITargetBuilder Before(this ITargetBuilder builder, Action before)
        {
            return builder.Wrap(next =>
            {
                before();
                next();
            });
        }

        public static ITargetBuilder Cleanup(this ITargetBuilder builder, Action cleanup)
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

        public static ITargetBuilder FailIf(this ITargetBuilder builder, Func<bool> predicate, string message = null)
        {
            return builder.Wrap(next =>
            {
                if (predicate())
                    throw new BuildCsFailTargetException(message);
                next();
            });
        }

        public static ITargetBuilder SkipIf(this ITargetBuilder builder, Func<bool> predicate, string message = null)
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