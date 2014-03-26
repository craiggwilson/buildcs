using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.AssemblyInfo
{
    public static class Extensions
    {
        public static AssemblyInfoHelper AssemblyInfoHelper(this IBuild build)
        {
            return build.GetService<AssemblyInfoHelper>();
        }

        public static void GenerateCSharpAssemblyInfo(this IBuild build, BuildItem outputPath, Action<GenerateArgs> config)
        {
            AssemblyInfoHelper(build).GenerateCSharp(outputPath, config);
        }
    }
}