using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;
using BuildCs.Processes;
using BuildCs.Tracing;

namespace BuildCs.Nuget
{
    public class NugetHelper
    {
        private readonly FileSystemHelper _fileSystem;
        private readonly ProcessHelper _processHelper;
        private readonly BuildTracer _tracer;

        public NugetHelper(BuildTracer tracer, FileSystemHelper fileSystem, ProcessHelper processHelper)
        {
            _fileSystem = fileSystem;
            _processHelper = processHelper;
            _tracer = tracer;
        }

        public BuildItem DefaultOutputPath { get; set; }

        public void RestorePackages()
        {
            var packageConfigs = new BuildGlob().Include("**/*.sln");
            packageConfigs.Each(pc => RestorePackage(pc, null));
        }

        public void RestorePackage(BuildItem file, Action<RestorePackageConfig> config)
        {
            var rpConfig = new RestorePackageConfig();
            if (config != null)
                config(rpConfig);

            if (rpConfig.OutputPath == null)
                rpConfig.OutputPath = DefaultOutputPath;

            var exe = GetExecutable(rpConfig.ToolPath);
            var args = "restore \"{0}\" ".F(file) + GetArguments(rpConfig);

            var exitCode = _processHelper.Exec(p =>
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = args;
                if (rpConfig.Timeout.HasValue)
                    p.Timeout = rpConfig.Timeout.Value;
                p.OnErrorMessage = m => _tracer.Error(m);
                p.OnOutputMessage = m => _tracer.Log(m);
            });

            if (exitCode != 0)
                throw new BuildCsException("Nuget failed with exit code '{0}'.".F(exitCode));
        }

        private string GetArguments(RestorePackageConfig config)
        {
            var list = new List<string>();

            if (config.OutputPath != null)
                list.Add("-OutputDirectory \"{0}\"".F(config.OutputPath));

            if (config.Sources != null)
                config.Sources.Each(s => list.Add("-Source \"{0}\"".F(s)));

            return string.Join(" ", list);
        }

        private string GetExecutable(BuildItem toolPath)
        {
            if (toolPath != null)
                return toolPath;

            toolPath = new BuildGlob().Include("**/nuget.exe").FirstOrDefault();
            if (toolPath != null)
                return toolPath;

            return new BuildGlob().Include("**/NuGet.exe").FirstOrDefault();
        }
    }
}