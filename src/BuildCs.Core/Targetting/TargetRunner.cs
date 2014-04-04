using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BuildCs.Tracing;

namespace BuildCs.Targetting
{
    public class TargetRunner
    {
        private readonly IBuildSession _session;
        private readonly TargetManager _targetManager;
        private readonly Tracer _tracer;

        public TargetRunner(IBuildSession session, TargetManager targetManager, Tracer tracer)
        {
            _session = session;
            _targetManager = targetManager;
            _tracer = tracer;

            TargetsToRun = new List<string>();
        }

        public IList<string> TargetsToRun { get; private set; }

        public void Run()
        {
            var build = new BuildExecution { Session = _session };
            build.Targets = _targetManager
                .GetBuildChain(TargetsToRun)
                .Select(x => new TargetExecution(build, x))
                .ToList();

            using (_tracer.StartBuild(build))
            {
                foreach (var target in build.Targets)
                {
                    Run((TargetExecution)target);
                    if (target.Status == TargetExecutionStatus.Failed)
                        break;
                }
            }
        }

        private void Run(TargetExecution target)
        {
            using (_tracer.StartTarget(target))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    target.Target.Run(target);
                    if (target.Status == TargetExecutionStatus.NotRun)
                        target.Status = TargetExecutionStatus.Success;
                }
                catch (Exception ex)
                {
                    target.MarkFailed(null, ex);
                }
                finally
                {
                    stopwatch.Stop();
                    target.Duration = stopwatch.Elapsed;
                }
            }
        }

        private class BuildExecution : IBuildExecution
        {
            public TimeSpan Duration
            {
                get
                {
                    return TimeSpan.FromTicks(Targets.Select(x => x.Duration)
                        .DefaultIfEmpty(TimeSpan.Zero)
                        .Sum(x => x.Ticks));
                }
            }

            public Exception Exception
            {
                get
                {
                    return Targets.Select(x => x.Exception)
                        .Where(x => x != null)
                        .FirstOrDefault();
                }
            }

            public IBuildSession Session { get; set; }

            public BuildExecutionStatus Status
            {
                get
                {
                    var maxStatus = Targets.Select(x => x.Status)
                        .DefaultIfEmpty(TargetExecutionStatus.NotRun)
                        .Max();

                    return (BuildExecutionStatus)maxStatus;
                }
            }

            public IReadOnlyList<ITargetExecution> Targets { get; set; }
        }

        private class TargetExecution : ITargetExecution
        {
            public TargetExecution(BuildExecution build, ITarget target)
            {
                Build = build;
                Duration = TimeSpan.Zero;
                Target = target;
            }

            public IBuildExecution Build { get; set; }

            public string Description
            {
                get { return Target.Description; }
            }

            public TimeSpan Duration { get; set; }

            public Exception Exception { get; set; }

            public string Message { get; set; }

            public string Name
            {
                get { return Target.Name; }
            }

            public TargetExecutionStatus Status { get; set; }

            public ITarget Target { get;  set; }

            public void MarkFailed(string message, Exception exception)
            {
                Message = message;
                Exception = exception;
                Status = TargetExecutionStatus.Failed;
            }

            public void MarkSkipped(string message)
            {
                Message = message;
                Status = TargetExecutionStatus.Skipped;
            }
        }
    }
}