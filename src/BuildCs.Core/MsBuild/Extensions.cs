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
        public static MsBuildHelper MsBuildHelper(this IBuildSession session)
        {
            return session.GetService<MsBuildHelper>();
        }

        public static void MsBuild(this IBuildSession session, BuildItem project, Action<MsBuildArgs> config = null)
        {
            MsBuild(session, new[] { project }, config);
        }

        public static void MsBuild(this IBuildSession session, IEnumerable<BuildItem> projects, Action<MsBuildArgs> config = null)
        {
            MsBuildHelper(session).Build(projects, config);
        }
    }
}