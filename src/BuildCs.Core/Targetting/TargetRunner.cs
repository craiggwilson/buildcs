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

            TargetsToRun = new List<string>();
        }

        public IList<string> TargetsToRun { get; private set; }

        public void Run()
        {
            var chain = _targetManager
                .GetBuildChain(TargetsToRun)
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
    }
}