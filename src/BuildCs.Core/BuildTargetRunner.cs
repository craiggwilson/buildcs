using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public class BuildTargetRunner
    {
        private readonly BuildTargetManager _targetManager;

        public BuildTargetRunner(BuildTargetManager targetManager)
        {
            _targetManager = targetManager;
        }

        public void RunTargets(IBuildTracer tracer, IEnumerable<string> requestedTargets)
        {
            var chain = _targetManager.GetBuildChain(requestedTargets);
            foreach(var target in chain)
            {
                Run(tracer, new BuildTargetRunContext(target));
            }
        }

        private void Run(IBuildTracer tracer, BuildTargetRunContext context)
        {
            tracer.Trace("Running Target '{0}'", context.Target.Name);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                context.Target.Action();
                stopwatch.Stop();
                context.MarkSuccessful(stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                context.MarkFailed(stopwatch.Elapsed, ex);
            }
        }
    }
}