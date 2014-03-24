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

        public static void NugetInstall(this Build build, string packageId, Action<InstallPackageArgs> config)
        {
            NugetHelper(build).Install(packageId, config);
        }

        public static void NugetPack(this Build build, BuildItem nuspecFile, Action<PackArgs> config)
        {
            NugetHelper(build).Pack(nuspecFile, config);
        }

        public static void NugetPush(this Build build, BuildItem nupkgFile, Action<PushArgs> config)
        {
            NugetHelper(build).Push(nupkgFile, config);
        }

        public static void NugetRestorePackages(this Build build)
        {
            NugetHelper(build).RestorePackages();
        }
    }
}