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
        private readonly ProcessHelper _process;
        private readonly BuildTracer _tracer;

        public NugetHelper(BuildTracer tracer, FileSystemHelper fileSystem, ProcessHelper process)
        {
            _fileSystem = fileSystem;
            _process = process;
            _tracer = tracer;
        }

        public void Install(string packageId, Action<InstallPackageArgs> config)
        {
            using (_tracer.StartTask("Nuget - Install"))
            {
                var args = new InstallPackageArgs();
                if (config != null)
                    config(args);

                _tracer.Info("Installing '{0}'.", packageId);

                ExecNuget(
                    GetExecutable(args.ToolPath),
                    "install {0} ".F(packageId) + GetArguments(args),
                    args.Timeout);
            }
        }

        public void Pack(BuildItem nuspecFile, Action<PackArgs> config)
        {
            using (_tracer.StartTask("Nuget - Pack"))
            {
                var args = new PackArgs();
                if (config != null)
                    config(args);

                _tracer.Info("Packing '{0}'.", nuspecFile);
                ExecNuget(
                    GetExecutable(args.ToolPath),
                    "pack \"{0}\" ".F(nuspecFile) + GetArguments(args),
                    args.Timeout);
            }
        }

        public void Push(BuildItem nupkgFile, Action<PushArgs> config)
        {
            using (_tracer.StartTask("Nuget - Push"))
            {
                var args = new PushArgs();
                if (config != null)
                    config(args);

                _tracer.Info("Pushing '{0}' to '{1}'.", nupkgFile, args.Source);
                ExecNuget(
                    GetExecutable(args.ToolPath),
                    "push \"{0}\" ".F(nupkgFile) + GetArguments(args),
                    args.Timeout);
            }
        }

        public void RestorePackages()
        {
            var packageConfigs = new BuildGlob().Include("**/*.sln");
            packageConfigs.Each(pc => RestorePackage(pc, null));
        }

        public void RestorePackage(BuildItem file, Action<RestorePackageArgs> config)
        {
            using (_tracer.StartTask("Nuget - Restore Package"))
            {
                var args = new RestorePackageArgs();
                if (config != null)
                    config(args);

                _tracer.Info("Restoring packages in '{0}'.", file);

                ExecNuget(
                    GetExecutable(args.ToolPath),
                    "restore \"{0}\" ".F(file) + GetArguments(args),
                    args.Timeout);
            }
        }

        private void ExecNuget(string exe, string args, TimeSpan? timeout)
        {
            var exitCode = _process.Exec(p =>
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = args;
                if (timeout.HasValue)
                    p.Timeout = timeout.Value;
            });

            if (exitCode != 0)
                throw new BuildCsException("Nuget failed with exit code '{0}'.".F(exitCode));
        }

        private string GetArguments(InstallPackageArgs args)
        {
            var list = GetBaseArguments(args);

            if (args.ExcludeVersion.HasValue && args.ExcludeVersion.Value)
                list.Add("-ExcludeVersion");

            if (!args.NonInteractive.HasValue || args.NonInteractive.Value)
                list.Add("-NonInteractive");

            if (args.OutputDirectory != null)
                list.Add("-OutputDirectory \"{0}\"".F(args.OutputDirectory));

            if (args.PreRelease.HasValue && args.PreRelease.Value)
                list.Add("-Prerelease");

            args.Sources.Each(s => list.Add("-Source \"{0}\"".F(s)));

            return string.Join(" ", list);
        }

        private string GetArguments(PackArgs args)
        {
            var list = GetBaseArguments(args);

            if (!args.NonInteractive.HasValue || args.NonInteractive.Value)
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

        private string GetArguments(PushArgs args)
        {
            var list = GetBaseArguments(args);

            if (args.ApiKey != null)
                list.Add(args.ApiKey);

            if (args.Source != null)
                list.Add("-Source \"{0}\"".F(args.Source));

            if(args.Timeout.HasValue)
            {
                list.Add("-Timeout {0}".F(args.Timeout.Value.Seconds));
                args.Timeout = null; // timeout handled by nuget...
            }

            return string.Join(" ", list);
        }

        private string GetArguments(RestorePackageArgs args)
        {
            var list = GetBaseArguments(args);

            if (args.OutputDirectory != null)
                list.Add("-OutputDirectory \"{0}\"".F(args.OutputDirectory));

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

            toolPath = new BuildGlob().Include("**/NuGet.exe").FirstOrDefault();
            if (toolPath == null)
                throw new BuildCsException("Unabled to find nuget.exe");

            return toolPath;
        }
    }
}