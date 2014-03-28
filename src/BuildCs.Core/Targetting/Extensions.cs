using System;
using System.Linq;

namespace BuildCs.Targetting
{
    public static class Extensions
    {
        public static TargetManager TargetManager(this IBuildSession session)
        {
            return session.GetService<TargetManager>();
        }

        public static TargetRunner TargetRunner(this IBuildSession session)
        {
            return session.GetService<TargetRunner>();
        }

        public static void Run(this IBuildSession session, params string[] targets)
        {
            session.TargetRunner().Run();
        }

        public static void RunTargetOrDefault(this IBuildSession session, string defaultTarget)
        {
            var runner = session.TargetRunner();
            if (runner.TargetsToRun.Count == 0)
                runner.TargetsToRun.Add(defaultTarget);

            Run(session);
        }

        public static ITargetBuilder Target(this IBuildSession session, string name)
        {
            return session.TargetManager().AddTarget(name);
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