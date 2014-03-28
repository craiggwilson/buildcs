using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BuildCs.Tracing;

namespace BuildCs.Targetting
{
    public class TargetRunner
    {
        private readonly TargetManager _targetManager;
        private readonly Tracer _tracer;

        public TargetRunner(TargetManager targetManager, Tracer tracer)
        {
            _targetManager = targetManager;
            _tracer = tracer;
        }

        public void RunTargets(IEnumerable<string> requestedTargets)
        {
            var chain = _targetManager
                .GetBuildChain(requestedTargets)
                .Select(x => new TargetExecution(x))
                .ToList();

            var build = new BuildExecution(chain);

            using (_tracer.StartBuild(build))
            {
                foreach (var target in chain)
                {
                    Run(build, target);
                    if (target.Status == TargetExecutionStatus.Failed)
                        break;
                }
                WriteSummary(chain);
            }
        }

        private void Run(BuildExecution build, TargetExecution target)
        {
            using (_tracer.StartTarget(build, target))
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    target.Target.Run();
                    stopwatch.Stop();
                    target.MarkSuccessful(stopwatch.Elapsed);
                }
                catch(BuildCsFailTargetException ex)
                {
                    stopwatch.Stop();
                    target.MarkFailed(stopwatch.Elapsed, ex);
                    _tracer.Error("Failed. {0}", ex.Message);
                }
                catch(BuildCsSkipTargetException ex)
                {
                    stopwatch.Stop();
                    target.MarkSkipped(stopwatch.Elapsed);
                    _tracer.Info("Skipped. {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    target.MarkFailed(stopwatch.Elapsed, ex);
                    _tracer.Error("Failed. {0}", ex);
                }
            }
        }

        private void WriteSummary(IEnumerable<TargetExecution> chain)
        {
            var maxLength = chain.Max(x => x.Target.Name.Length);
            var anyFailed = chain.Any(x => x.Status == TargetExecutionStatus.Failed);
            var type = MessageLevel.Info;
            if (anyFailed)
                type = MessageLevel.Error;

            _tracer.Write(type, "");
            _tracer.Write(type, "---------------------------------------------------------------------");
            _tracer.Write(type, "Build Time Report");
            _tracer.Write(type, "---------------------------------------------------------------------");
            _tracer.Write(type, "Target".PadRight(maxLength) + "    Duration");
            _tracer.Write(type, "------".PadRight(maxLength) + "    --------");
            foreach(var context in chain)
            {
                var text = context.Target.Name.PadRight(maxLength + 4) + context.Duration.ToString();
                switch(context.Status)
                {
                    case TargetExecutionStatus.Failed:
                        _tracer.Error(context.Target.Name.PadRight(maxLength + 4) + "Failed");
                        break;
                    case TargetExecutionStatus.NotRun:
                        _tracer.Info(context.Target.Name.PadRight(maxLength + 4) + "Not Run");
                        break;
                    case TargetExecutionStatus.Skipped:
                        _tracer.Info(context.Target.Name.PadRight(maxLength + 4) + "Skipped");
                        break;
                    case TargetExecutionStatus.Success:
                        _tracer.Info(context.Target.Name.PadRight(maxLength + 4) + context.Duration.ToString());
                        break;
                }
            }
            _tracer.Write(type, "------".PadRight(maxLength) + "    --------");
            var status = anyFailed
                ? "Failed"
                : "Ok";
            _tracer.Write(type, "Result".PadRight(maxLength + 4) + status);
            _tracer.Write(type, "---------------------------------------------------------------------");
        }
    }
}