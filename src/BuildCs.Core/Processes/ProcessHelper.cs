using System;
using System.Diagnostics;
using System.Threading;
using BuildCs.Environment;
using BuildCs.Tracing;

namespace BuildCs.Processes
{
    public class ProcessHelper
    {
        private readonly EnvironmentHelper _environment;
        private readonly Tracer _tracer;

        private string _monoPath;
        private string _monoArgs;

        public ProcessHelper(Tracer tracer, EnvironmentHelper environment)
        {
            _environment = environment;
            _tracer = tracer;
            TraceProcesses = true;
        }

        public bool TraceProcesses { get; set; }

        public int Exec(Action<ExecArgs> config)
        {
            TimeSpan timeout = Timeout.InfiniteTimeSpan;
            var launchResult = (LaunchResult)Launch(args =>
            {
                var execArgs = new ExecArgs(args.StartInfo);
                config(execArgs);
                timeout = execArgs.Timeout;
            });

            return launchResult.Dispose(timeout);
        }

        public IDisposable Launch(Action<LaunchArgs> config)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            var args = new LaunchArgs(process.StartInfo);
            args.OnErrorMessage = m => _tracer.Error(m);
            args.OnOutputMessage = m => _tracer.Trace(m);
            config(args);

            if (args.TraceOutput)
            {
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                if (args.OnOutputMessage != null)
                    process.OutputDataReceived += (_, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                            args.OnOutputMessage(e.Data);
                    };
                if (args.OnErrorMessage != null)
                    process.ErrorDataReceived += (_, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                            args.OnErrorMessage(e.Data);
                    };
            }

            AdaptToMonoIfNecessary(process.StartInfo);

            var task = _tracer.StartTask("Exec");
            if (TraceProcesses)
                _tracer.Log("{0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments);

            try
            {
                process.Start();
                if (args.TraceOutput)
                {
                    process.BeginErrorReadLine();
                    process.BeginOutputReadLine();
                }
            }
            catch (Exception ex)
            {
                _tracer.Error("Start of process '{0} {1}' failed. {2}", process.StartInfo.FileName, process.StartInfo.Arguments, ex);
            }

            return new LaunchResult(process, _tracer, task);
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

        private class LaunchResult : IDisposable
        {
            private readonly Process _process;
            private readonly Tracer _tracer;
            private readonly IDisposable _tracerTask;
            private bool _disposed;

            public LaunchResult(Process process, Tracer tracer, IDisposable tracerTask)
            {
                _process = process;
                _tracer = tracer;
                _tracerTask = tracerTask;
            }

            public void Dispose()
            {
                Dispose(TimeSpan.Zero);
            }

            public int Dispose(TimeSpan timeout)
            {
                if (_disposed)
                    return _process.ExitCode;

                using(_tracerTask)
                {
                    if (!_process.WaitForExit(timeout.Milliseconds))
                    {
                        try
                        {
                            _process.Kill();
                        }
                        catch (Exception ex)
                        {
                            _tracer.Error("Could not kill process '{0} {1}' after '{1}' milliseconds. {2}", _process.StartInfo.FileName, _process.StartInfo.Arguments, timeout.Milliseconds, ex);
                        }

                        if(timeout != TimeSpan.Zero)
                            throw new BuildCsException("Process '{0} {1}' timed out.".F(_process.StartInfo.FileName, _process.StartInfo.Arguments));
                    }
                }

                _disposed = true;
                return _process.ExitCode;
            }
        }
    }
}