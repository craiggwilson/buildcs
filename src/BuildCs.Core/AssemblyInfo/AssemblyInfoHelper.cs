﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;
using BuildCs.Tracing;

namespace BuildCs.AssemblyInfo
{
    public class AssemblyInfoHelper
    {
        private readonly FileSystemHelper _fileSystem;
        private readonly Tracer _tracer;

        public AssemblyInfoHelper(Tracer tracer, FileSystemHelper fileSystem)
        {
            _fileSystem = fileSystem;
            _tracer = tracer;
        }

        public void GenerateCSharp(BuildItem outputPath, Action<GenerateArgs> config)
        {
            using (_tracer.StartTask("GenerateAssemblyInfo"))
            {
                _tracer.Info("Generating assembly info at '{0}'.", outputPath);
                var args = new GenerateArgs();
                if (config != null)
                    config(args);

                var lines = new List<string>();
                lines.Add("// <auto-generated />");
                lines.Add("");
                GetDependencies(args.Attributes.Attributes).Each(d => lines.Add("using {0};".F(d)));
                lines.Add("");
                args.Attributes.Attributes.Each(a => lines.Add("[assembly: {0}({1})]".F(a.Name, string.Join(", ", a.Values))));

                using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
                {
                    lines.Each(l => writer.WriteLine(l));
                }
            }
        }

        private IEnumerable<string> GetDependencies(IEnumerable<AssemblyInfoAttribute> attributes)
        {
            return attributes.Select(x => x.Namespace).Distinct();
        }
    }
}