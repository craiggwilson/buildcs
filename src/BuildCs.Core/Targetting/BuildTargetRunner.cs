using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BuildCs.Tracing;

namespace BuildCs.Targetting
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
            using (_tracer.WithPrefix("[{0}] ".F(context.Target.Name)))
            {
                _tracer.Info("Beginning at {0:HH:mm:ss}", DateTime.Now);

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    context.Target.Run();
                    stopwatch.Stop();
                    context.MarkSuccessful(stopwatch.Elapsed);
                    _tracer.Info("Completed in {0}", stopwatch.Elapsed);
                }
                catch(BuildCsFailTargetException ex)
                {
                    stopwatch.Stop();
                    context.MarkFailed(stopwatch.Elapsed, ex);
                    _tracer.Fatal("Failed. {0}", ex.Message);
                }
                catch(BuildCsSkipTargetException ex)
                {
                    stopwatch.Stop();
                    context.MarkSkipped();
                    _tracer.Info("Skipped. {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    context.MarkFailed(stopwatch.Elapsed, ex);
                    _tracer.Fatal("Failed. {0}", ex);
                }
            }
        }

        private void WriteSummary(IEnumerable<BuildTargetRunContext> chain)
        {
            var maxLength = chain.Max(x => x.Target.Name.Length);
            var anyFailed = chain.Any(x => x.Status == BuildTargetStatus.Failed);
            var type = BuildMessageType.Success;
            if (anyFailed)
                type = BuildMessageType.Error;

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
                    case BuildTargetStatus.Failed:
                        _tracer.Fatal(context.Target.Name.PadRight(maxLength + 4) + "Failed");
                        break;
                    case BuildTargetStatus.NotRun:
                        _tracer.Info(context.Target.Name.PadRight(maxLength + 4) + "Not Run");
                        break;
                    case BuildTargetStatus.Skipped:
                        _tracer.Info(context.Target.Name.PadRight(maxLength + 4) + "Skipped");
                        break;
                    case BuildTargetStatus.Success:
                        _tracer.Success(context.Target.Name.PadRight(maxLength + 4) + context.Duration.ToString());
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