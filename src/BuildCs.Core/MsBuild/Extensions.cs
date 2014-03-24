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
        public static MsBuildHelper MsBuildHelper(this Build build)
        {
            return build.GetService<MsBuildHelper>();
        }

        public static void MsBuild(this Build build, BuildItem project, Action<MsBuildConfig> config = null)
        {
            MsBuild(build, new[] { project }, config);
        }

        public static void MsBuild(this Build build, IEnumerable<BuildItem> projects, Action<MsBuildConfig> config = null)
        {
            MsBuildHelper(build).Exec(projects, config);
        }
    }
}