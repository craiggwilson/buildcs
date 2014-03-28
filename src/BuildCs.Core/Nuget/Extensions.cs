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
        public static NugetHelper NugetHelper(this IBuildSession session)
        {
            return session.GetService<NugetHelper>();
        }

        public static void NugetInstall(this IBuildSession session, string packageId, Action<InstallPackageArgs> config)
        {
            NugetHelper(session).Install(packageId, config);
        }

        public static void NugetPack(this IBuildSession session, BuildItem nuspecFile, Action<PackArgs> config)
        {
            NugetPack(session, new BuildItem[] { nuspecFile }, config);
        }

        public static void NugetPack(this IBuildSession session, IEnumerable<BuildItem> nuspecFiles, Action<PackArgs> config)
        {
            nuspecFiles.Each(nuspecFile => NugetHelper(session).Pack(nuspecFile, config));
        }

        public static void NugetPush(this IBuildSession session, BuildItem nupkgFile, Action<PushArgs> config)
        {
            NugetPush(session, new BuildItem[] { nupkgFile }, config);
        }

        public static void NugetPush(this IBuildSession session, IEnumerable<BuildItem> nupkgFiles, Action<PushArgs> config)
        {
            nupkgFiles.Each(nupkgFile => NugetHelper(session).Push(nupkgFile, config));
        }

        public static void NugetRestorePackages(this IBuildSession session)
        {
            NugetHelper(session).RestorePackages();
        }
    }
}