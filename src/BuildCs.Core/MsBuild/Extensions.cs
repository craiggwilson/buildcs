using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.MsBuild
{
    public static class Extensions
    {
        public static MsBuildHelper MsBuildHelper(this IBuild build)
        {
            return build.GetService<MsBuildHelper>();
        }

        public static void MsBuild(this IBuild build, BuildItem project, Action<MsBuildArgs> config = null)
        {
            MsBuild(build, new[] { project }, config);
        }

        public static void MsBuild(this IBuild build, IEnumerable<BuildItem> projects, Action<MsBuildArgs> config = null)
        {
            MsBuildHelper(build).Exec(projects, config);
        }
    }
}