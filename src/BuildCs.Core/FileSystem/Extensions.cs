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
        public static AssemblyInfoHelper AssemblyInfoHelper(this IBuildSession session)
        {
            return session.GetService<AssemblyInfoHelper>();
        }

        public static void GenerateCSharpAssemblyInfo(this IBuildSession session, BuildItem outputPath, Action<GenerateArgs> config)
        {
            AssemblyInfoHelper(session).GenerateCSharp(outputPath, config);
        }
    }
}