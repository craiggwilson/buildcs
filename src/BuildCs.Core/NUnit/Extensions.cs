using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.NUnit
{
    public static class Extensions
    {
        public static NUnitHelper NUnitHelper(this IBuildSession session)
        {
            return session.GetService<NUnitHelper>();
        }

        public static void NUnit(this IBuildSession session, BuildItem assembly, Action<NUnitArgs> config = null)
        {
            NUnit(session, new[] { assembly }, config);
        }

        public static void NUnit(this IBuildSession session, IEnumerable<BuildItem> assemblies, Action<NUnitArgs> config = null)
        {
            NUnitHelper(session).Test(assemblies, config);
        }
    }
}