using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BuildCs.Services.Tracing;

namespace BuildCs.Services.Targetting
{
    public class BuildTargetRunner
    {
        private readonly BuildTargetManager _targetManager;
        private readonly BuildTracer _tracer;

        public BuildTargetRunner(BuildTargetManager targetManager, BuildTracer tracer)
        {
            _targetManager = targetManager;
            _tracer = tracer;
        }

        public void RunTargets(IEnumerable<string> requestedTargets)
        {
            var chain = _targetManager
                .GetBuildChain(requestedTargets)
                .Select(x => new BuildTargetRunContext(x))
                .ToList();

            var names = string.Join(", ", chain.Select(x => x.Target.Name));
            _tracer.Info("Building Targets: {0}", names);
            
            foreach(var context in chain)
            {
                Run(context);
                if(context.Status == BuildTargetStatus.Failed)
                    break;
            }

            WriteSummary(chain);
        }

        private void Run(BuildTargetRunContext context)
        {
            using (_tracer.WithPrefix("{0}: ".F(context.Target.Name)))
            {
                _tracer.Info("Beginning...", context.Target.Name);

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    context.Target.Run();
                    stopwatch.Stop();
                    context.MarkSuccessful(stopwatch.Elapsed);
                    _tracer.Info("Completed in {0}", stopwatch.Elapsed);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    context.MarkFailed(stopwatch.Elapsed, ex);
                    _tracer.Error("Failed...");
                    _tracer.Error("Exception: {0}", ex);
                }
            }
        }

        private void WriteSummary(IEnumerable<BuildTargetRunContext> chain)
        {
            var maxLength = chain.Max(x => x.Target.Name.Length);
            _tracer.Info("");
            _tracer.Info("---------------------------------------------------------------------");
            _tracer.Info("Build Time Report");
            _tracer.Info("---------------------------------------------------------------------");
            _tracer.Info("Target".PadRight(maxLength) + "    Duration");
            _tracer.Info("------".PadRight(maxLength) + "    --------");
            foreach(var context in chain)
            {
                var text = context.Target.Name.PadRight(maxLength + 4) + context.Duration.ToString();
                switch(context.Status)
                {
                    case BuildTargetStatus.Failed:
                        _tracer.Error(text);
                        break;
                    case BuildTargetStatus.NotRun:
                    case BuildTargetStatus.Skipped:
                        _tracer.Log(text);
                        break;
                    case BuildTargetStatus.Success:
                        _tracer.Info(text);
                        break;
                }
            }
            _tracer.Info("---------------------------------------------------------------------");
        }
    }
}