using System;
using System.Diagnostics;
using BuildCs.Tracing;

namespace BuildCs.Processes
{
    public class ProcessHelper
    {
        private readonly EnvironmentHelper _environment;
        private readonly BuildTracer _tracer;

        private string _monoPath;
        private string _monoArgs;

        public ProcessHelper(BuildTracer tracer, EnvironmentHelper environment)
        {
            _environment = environment;
            _tracer = tracer;
            TraceProcesses = true;
        }

        public bool TraceProcesses { get; set; }

        public int Exec(Action<ProcessConfig> config)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            var processConfig = new ProcessConfig(process.StartInfo);
            config(processConfig);

            if (processConfig.TraceOutput)
            {
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                if (processConfig.OnOutputMessage != null)
                    process.OutputDataReceived += (_, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                            processConfig.OnOutputMessage(e.Data);
                    };
                if (processConfig.OnErrorMessage != null)
                    process.ErrorDataReceived += (_, e) =>
                    {
                        if(!string.IsNullOrWhiteSpace(e.Data))
                            processConfig.OnErrorMessage(e.Data);
                    };
            }

            AdaptToMonoIfNecessary(process.StartInfo);

            if(TraceProcesses)
                _tracer.Info("{0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments);

            try
            {
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                _tracer.Error("Start of process '{0} {1}' failed. {2}", process.StartInfo.FileName, process.StartInfo.Arguments, ex);
            }

            if(!process.WaitForExit(processConfig.Timeout.Milliseconds))
            {
                try
                {
                    process.Kill();
                }
                catch(Exception ex)
                {
                    _tracer.Error("Could not kill process '{0} {1}' after '{1}' milliseconds. {2}", process.StartInfo.FileName, process.StartInfo.Arguments, processConfig.Timeout.Milliseconds, ex);
                }

                throw new BuildCsException("Process '{0} {1}' timed out.".F(process.StartInfo.FileName, process.StartInfo.Arguments));
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