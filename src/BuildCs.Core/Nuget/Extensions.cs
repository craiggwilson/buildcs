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
        public static NugetHelper NugetHelper(this IBuild build)
        {
            return build.GetService<NugetHelper>();
        }

        public static void NugetInstall(this IBuild build, string packageId, Action<InstallPackageArgs> config)
        {
            NugetHelper(build).Install(packageId, config);
        }

        public static void NugetPack(this IBuild build, BuildItem nuspecFile, Action<PackArgs> config)
        {
            NugetPack(build, new BuildItem[] { nuspecFile }, config);
        }

        public static void NugetPack(this IBuild build, IEnumerable<BuildItem> nuspecFiles, Action<PackArgs> config)
        {
            nuspecFiles.Each(nuspecFile => NugetHelper(build).Pack(nuspecFile, config));
        }

        public static void NugetPush(this IBuild build, BuildItem nupkgFile, Action<PushArgs> config)
        {
            NugetPush(build, new BuildItem[] { nupkgFile }, config);
        }

        public static void NugetPush(this IBuild build, IEnumerable<BuildItem> nupkgFiles, Action<PushArgs> config)
        {
            nupkgFiles.Each(nupkgFile => NugetHelper(build).Push(nupkgFile, config));
        }

        public static void NugetRestorePackages(this IBuild build)
        {
            NugetHelper(build).RestorePackages();
        }
    }
}