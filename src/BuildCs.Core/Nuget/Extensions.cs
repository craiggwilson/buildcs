using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Nuget
{
    public static class Extensions
    {
        public static NugetHelper NugetHelper(this Build build)
        {
            return build.GetService<NugetHelper>();
        }

        public static void NugetRestorePackages(this Build build)
        {
            NugetHelper(build).RestorePackages();
        }
    }
}