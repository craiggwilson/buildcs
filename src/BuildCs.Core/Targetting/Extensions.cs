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

        public static ITargetBuilder Do(this ITargetBuilder builder, Action action)
        {
            return builder.Do(_ => action());
        }

        public static ITargetBuilder PreCondition(this ITargetBuilder builder, Func<bool> predicate, string message = null)
        {
            return builder.Wrap((target, next) =>
            {
                if (predicate())
                    target.MarkFailed(message, null);
                else
                    next(target);
            });
        }

        public static ITargetBuilder SkipIf(this ITargetBuilder builder, Func<bool> predicate, string message = null)
        {
            return builder.Wrap((target, next) =>
            {
                if (predicate())
                    target.MarkSkipped(message);
                else
                    next(target);
            });
        }
    }
}