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

        public void RestorePackages()
        {
            var packageConfigs = new BuildGlob().Include("**/*.sln");
            packageConfigs.Each(pc => RestorePackage(pc, null));
        }

        public void Pack(BuildItem nuspecFile, Action<PackArgs> config)
        {
            var args = new PackArgs();
            if (config != null)
                config(args);

            ExecNuget(
                GetExecutable(args.ToolPath),
                "pack \"{0}\" ".F(nuspecFile) + GetArguments(args),
                args.Timeout);
        }

        public void RestorePackage(BuildItem file, Action<RestorePackageArgs> config)
        {
            var args = new RestorePackageArgs();
            if (config != null)
                config(args);

            ExecNuget(
                GetExecutable(args.ToolPath), 
                "restore \"{0}\" ".F(file) + GetArguments(args),
                args.Timeout);
        }

        private void ExecNuget(string exe, string args, TimeSpan? timeout)
        {
            var exitCode = _processHelper.Exec(p =>
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = args;
                if (timeout.HasValue)
                    p.Timeout = timeout.Value;
                p.OnErrorMessage = m => _tracer.Error(m);
                p.OnOutputMessage = m => _tracer.Log(m);
            });

            if (exitCode != 0)
                throw new BuildCsException("Nuget failed with exit code '{0}'.".F(exitCode));
        }

        private string GetArguments(PackArgs args)
        {
            var list = GetBaseArguments(args);

            if (args.NonInteractive)
                list.Add("-NonInteractive");

            if (args.BasePath != null)
                list.Add("-BasePath \"{0}\"".F(args.BasePath));

            if (args.OutputDirectory != null)
                list.Add("-OutputDirectory \"{0}\"".F(args.OutputDirectory));

            if (args.Symbols.HasValue && args.Symbols.Value)
                list.Add("-Symbols");

            if (args.Version != null)
                list.Add("-Version " + args.Version);

            if(args.Properties.Count > 0)
            {
                list.Add("-Properties");
                foreach (var property in args.Properties)
                    list.Add("{0}=\"{1}\"".F(property.Key, property.Value));
            }

            return string.Join(" ", list);
        }

        private string GetArguments(RestorePackageArgs args)
        {
            var list = GetBaseArguments(args);

            if (args.OutputDirectory != null)
                list.Add("-OutputDirectory \"{0}\"".F(args.OutputDirectory));

            if (args.Sources != null)
                args.Sources.Each(s => list.Add("-Source \"{0}\"".F(s)));

            return string.Join(" ", list);
        }

        private List<string> GetBaseArguments(NugetArgsBase args)
        {
            var list = new List<string>();
            if (args.Verbosity.HasValue)
                list.Add("-Verbosity " + args.Verbosity.ToString().ToLowerInvariant());

            return list;
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