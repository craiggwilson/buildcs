using System;
using System.Diagnostics;
using BuildCs.Services.Tracing;

namespace BuildCs.Services.Processes
{
    public class BuildProcessRunner
    {
        private readonly BuildEnvironment _environment;
        private readonly BuildTracer _tracer;

        private string _monoPath;
        private string _monoArgs;

        public BuildProcessRunner(BuildTracer tracer, BuildEnvironment environment)
        {
            TraceProcesses = true;
        }

        public bool TraceProcesses { get; set; }

        public int Run(Action<ProcessStartInfo> config)
        {
            return Run(config, null);
        }

        public int Run(Action<ProcessStartInfo> config, ProcessArgs args)
        {
            args = args ?? new ProcessArgs();
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            config(process.StartInfo);

            if (args.CatchMessages)
            {
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (o, e) => args.OutputMessage(e.Data);
                process.ErrorDataReceived += (o, e) => args.ErrorMessage(e.Data);
            }

            AdaptToMonoIfNecessary(process.StartInfo);

            if(TraceProcesses)
                _tracer.Info("{0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments);

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                _tracer.Error("Start of process '{0} {1}' failed. {2}", process.StartInfo.FileName, process.StartInfo.Arguments, ex);
            }

            if(!process.WaitForExit(args.Timeout.Milliseconds))
            {
                try
                {
                    process.Kill();
                }
                catch(Exception ex)
                {
                    _tracer.Error("Could not kill process '{0} {1}' after '{1}' milliseconds. {2}", process.StartInfo.FileName, process.StartInfo.Arguments, args.Timeout.Milliseconds, ex);
                }

                _tracer.Error("Process '{0} {1}' timed out.", process.StartInfo.FileName, process.StartInfo.Arguments);
            }

            return process.ExitCode;
        }

        public void SetMonoPath(string path)
        {
            _monoPath = path;
        }

        public void SetMonoArgs(string args)
        {
            _monoArgs = args;
        }

        private void AdaptToMonoIfNecessary(ProcessStartInfo psi)
        {
            if (_environment.IsMono && psi.FileName.EndsWith(".exe"))
            {
                psi.Arguments = (_monoArgs ?? "") + " " + psi.FileName + " " + (psi.Arguments ?? "");
                psi.FileName = _monoPath ?? "mono";
            }
        }
    }
}