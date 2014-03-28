using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.XUnit
{
    public static class Extensions
    {
        public static XUnitHelper XUnitHelper(this IBuildSession session)
        {
            return session.GetService<XUnitHelper>();
        }

        public static void XUnit(this IBuildSession session, BuildItem assembly, Action<XUnitArgs> config = null)
        {
            XUnit(session, new[] { assembly }, config);
        }

        public static void XUnit(this IBuildSession session, IEnumerable<BuildItem> assemblies, Action<XUnitArgs> config = null)
        {
            XUnitHelper(session).Test(assemblies, config);
        }
    }
}