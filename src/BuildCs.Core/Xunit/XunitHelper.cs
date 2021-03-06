﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;
using BuildCs.Processes;
using BuildCs.Tracing;

namespace BuildCs.XUnit
{
    public class XUnitHelper
    {
        private readonly FileSystemHelper _fileSystem;
        private readonly ProcessHelper _process;
        private readonly Tracer _tracer;

        public XUnitHelper(Tracer tracer, FileSystemHelper fileSystem, ProcessHelper process)
        {
            _fileSystem = fileSystem;
            _process = process;
            _tracer = tracer;
        }

        public void Test(IEnumerable<BuildItem> assemblies, Action<XUnitArgs> config)
        {
            using (_tracer.StartTask("Xunit"))
            {
                var args = new XUnitArgs();
                if (config != null)
                    config(args);

                _tracer.Info("Testing '{0}'.", string.Join(", ", assemblies));

                var exitCode = _process.Exec(p =>
                {
                    p.StartInfo.FileName = GetExecutable(args.ToolPath);
                    p.StartInfo.Arguments = string.Join(" ", assemblies.Select(a => "\"{0}\"".F(a))) + " " + GetArguments(args);
                });

                if (exitCode != 0)
                    throw new BuildCsException("XUnit failed with exit code '{0}'.".F(exitCode));
            }
        }

        private string GetArguments(XUnitArgs args)
        {
            var list = new List<string>();

            if(args.ShadowCopy.HasValue && !args.ShadowCopy.Value)
                list.Add("/noshadow");

            if (args.Verbose.HasValue && !args.Verbose.Value)
                list.Add("/silent");

            if (args.HtmlOutputPath != null)
                list.Add("/html \"{0}\"".F(args.HtmlOutputPath));

            if(args.NUnitXmlOutputPath != null)
                list.Add("/nunit \"{0}\"".F(args.NUnitXmlOutputPath));

            if (args.XmlOutputPath != null)
                list.Add("/xml \"{0}\"".F(args.XmlOutputPath));

            if (args.IncludedTraits.Count > 0)
                args.IncludedTraits.Each(t => list.Add("/trait=\"{0}\"".F(t)));

            if (args.ExcludedTraits.Count > 0)
                args.ExcludedTraits.Each(t => list.Add("/-trait=\"{0}\"".F(t)));

            return string.Join(" ", list);
        }

        private string GetExecutable(BuildItem toolPath)
        {
            const string toolName = "xunit.console.clr4.exe";
            if (toolPath != null)
                return toolPath;

            toolPath = new BuildGlob().Include("**/" + toolName).FirstOrDefault();
            if (toolPath == null)
                throw new BuildCsException("Unable to find " + toolName);

            return toolPath;
        }
    }
}