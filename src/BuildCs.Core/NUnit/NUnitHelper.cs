using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;
using BuildCs.Processes;
using BuildCs.Tracing;

namespace BuildCs.NUnit
{
    public class NUnitHelper
    {
        private readonly FileSystemHelper _fileSystem;
        private readonly ProcessHelper _process;
        private readonly BuildTracer _tracer;

        public NUnitHelper(BuildTracer tracer, FileSystemHelper fileSystem, ProcessHelper process)
        {
            _fileSystem = fileSystem;
            _process = process;
            _tracer = tracer;
        }

        public void Test(IEnumerable<BuildItem> assemblies, Action<NUnitArgs> config)
        {
            var args = new NUnitArgs();
            if (config != null)
                config(args);

            var exitCode = _process.Exec(p =>
            {
                p.StartInfo.FileName = GetExecutable(args.ToolPath);
                p.StartInfo.Arguments = GetArguments(args) + " " + string.Join(" ", assemblies.Select(a => "\"{0}\"".F(a)));
                p.OnErrorMessage = m => _tracer.Error(m);
                p.OnOutputMessage = m => _tracer.Log(m);
            });

            if (exitCode != 0)
                throw new BuildCsException("NUnit failed with exit code '{0}'.".F(exitCode));
        }

        private string GetArguments(NUnitArgs args)
        {
            var list = new List<string>();

            if (args.ExcludeCategory != null)
                list.Add("-exclude:{0}".F(args.ExcludeCategory));

            if (args.IncludeCategory != null)
                list.Add("-include:{0}".F(args.IncludeCategory));

            if (args.Fixture != null)
                list.Add("-fixture:\"{0}\"".F(args.Fixture));

            if (args.Labels.HasValue && args.Labels.Value)
                list.Add("-labels");

            if (args.NoLogo.HasValue && args.NoLogo.Value)
                list.Add("-noshadow");

            if (args.ShadowCopy.HasValue && !args.ShadowCopy.Value)
                list.Add("-noshadow");

            if (args.XmlOutputPath != null)
                list.Add("-xml:\"{0}\"".F(args.XmlOutputPath));

            if (args.XsltTransformPath != null)
                list.Add("-transform:\"{0}\"".F(args.XsltTransformPath));

            return string.Join(" ", list);
        }

        private string GetExecutable(BuildItem toolPath)
        {
            const string toolName = "nunit-console.exe";
            if (toolPath != null)
                return toolPath;

            toolPath = new BuildGlob().Include("**/" + toolName).FirstOrDefault();
            if (toolPath == null)
                throw new BuildCsException("Unable to find " + toolName);

            return toolPath;
        }
    }
}