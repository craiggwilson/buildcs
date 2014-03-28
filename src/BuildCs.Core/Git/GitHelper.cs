using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;
using BuildCs.Processes;
using BuildCs.Tracing;

namespace BuildCs.Git
{
    public class GitHelper
    {
        private readonly EnvironmentHelper _environment;
        private readonly FileSystemHelper _fileSystem;
        private readonly ProcessHelper _process;
        private readonly Tracer _tracer;

        public GitHelper(Tracer tracer, EnvironmentHelper environment, FileSystemHelper fileSystem, ProcessHelper process)
        {
            _environment = environment;
            _fileSystem = fileSystem;
            _process = process;
            _tracer = tracer;
            GitSearchPaths = new List<BuildItem>
            {
                @"%ProgramFiles(x86)%\Git\bin\",
                @"%ProgramFiles%\Git\bin\"
            };
        }

        public IList<BuildItem> GitSearchPaths { get; set; }

        public IEnumerable<string> Exec(BuildItem repositoryDir, string args, TimeSpan? timeout = null)
        {
            using (_tracer.StartTask("Git"))
            {
                var results = new List<string>();
                var exitCode = _process.Exec(p =>
                {
                    p.StartInfo.WorkingDirectory = repositoryDir;
                    p.StartInfo.FileName = GetExecutable();
                    p.StartInfo.Arguments = args;
                    if (timeout.HasValue)
                        p.Timeout = timeout.Value;
                    p.OnOutputMessage = m =>
                    {
                        _tracer.Trace(m);
                        results.Add(m);
                    };
                });

                if (exitCode != 0)
                    throw new BuildCsException("Git failed with exit code '{0}'.".F(exitCode));

                return results;
            }
        }

        private string GetExecutable()
        {
            if (_environment.IsLinux)
                return "git";

            var ev = Environment.GetEnvironmentVariable("GIT");
            if (!string.IsNullOrEmpty(ev))
                return ev;

            return _fileSystem.FindFile("git.exe", GitSearchPaths)
                ?? "git";
        }
    }
}