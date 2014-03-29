using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Environment;
using BuildCs.FileSystem;
using BuildCs.Processes;
using BuildCs.Tracing;

namespace BuildCs.MsBuild
{
    public class MsBuildHelper
    {
        private readonly EnvironmentHelper _environment;
        private readonly FileSystemHelper _fileSystem;
        private readonly ProcessHelper _process;
        private readonly Tracer _tracer;

        public MsBuildHelper(Tracer tracer, EnvironmentHelper environment, FileSystemHelper fileSystem, ProcessHelper process)
        {
            _environment = environment;
            _fileSystem = fileSystem;
            _process = process;
            _tracer = tracer;
            MsBuildSearchPaths = new List<BuildItem>
            {
                @"%ProgramFiles(x86)%\MSBuild\12.0\bin",
                @"%ProgramFiles(x86)%\MSBuild\12.0\bin\amd64",
                @"%SystemRoot%\Microsoft.NET\Framework\v4.0.30319",
                @"%SystemRoot%\Microsoft.NET\Framework\v4.0.30128",
                @"%SystemRoot%\Microsoft.NET\Framework\v3.5"
            };
        }

        public IList<BuildItem> MsBuildSearchPaths { get; set; }

        public void Build(IEnumerable<BuildItem> projects, Action<MsBuildArgs> config)
        {
            if (projects == null)
                throw new ArgumentNullException("projects");

            var args = new MsBuildArgs();
            if(config != null)
                config(args);

            projects.Each(p => BuildProject(p, args));
        }

        private void BuildProject(BuildItem project, MsBuildArgs args)
        {
            using (_tracer.StartTask("MsBuild"))
            {
                _tracer.Info("Building '{0}'.", project);
                var exitCode = _process.Exec(p =>
                {
                    p.StartInfo.FileName = GetExecutable(args.ToolPath);
                    p.StartInfo.Arguments = GetArguments(args) + " \"{0}\"".F(project);
                });

                if (exitCode != 0)
                    throw new BuildCsException("MsBuild failed with exit code '{0}'.".F(exitCode));
            }
        }

        private string GetArguments(MsBuildArgs config)
        {
            var list = new List<string>();

            if (config.Targets != null && config.Targets.Count > 0)
                list.Add("/t:" + string.Join(";", config.Targets));

            if (config.NoLogo.HasValue && config.NoLogo.Value)
                list.Add("/nologo");

            if(config.Verbosity.HasValue)
            {
                switch(config.Verbosity.Value)
                {
                    case MsBuildVerbosity.Quiet:
                        list.Add("/v:q");
                        break;
                    case MsBuildVerbosity.Minimal:
                        list.Add("/v:m");
                        break;
                    case MsBuildVerbosity.Normal:
                        list.Add("/v:n");
                        break;
                    case MsBuildVerbosity.Detailed:
                        list.Add("/v:d");
                        break;
                    case MsBuildVerbosity.Diagnostic:
                        list.Add("/v:diag");
                        break;
                }
            }

            foreach (var property in config.Properties ?? Enumerable.Empty<KeyValuePair<string, string>>())
                list.Add("/p:{0}=\"{1}\"".F(property.Key, property.Value));

            if (config.ToolsVersion != null)
                list.Add("/tv:" + config.ToolsVersion);

            return string.Join(" ", list);
        }

        private string GetExecutable(BuildItem toolPath)
        {
            if (toolPath != null)
                return toolPath;

            if(_environment.IsMono)
                return "xbuild";

            var ev = _environment.GetVariableOrDefault("MSBuild", null);
            if(!string.IsNullOrEmpty(ev))
                return ev;

            return _fileSystem.FindFile("msbuild.exe", MsBuildSearchPaths) 
                ?? "msbuild";
        }
    }
}