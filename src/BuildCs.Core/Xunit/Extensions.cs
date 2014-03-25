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
        public static XUnitHelper XUnitHelper(this IBuild build)
        {
            return build.GetService<XUnitHelper>();
        }

        public static void XUnit(this IBuild build, BuildItem assembly, Action<XUnitArgs> config = null)
        {
            XUnit(build, new[] { assembly }, config);
        }

        public static void XUnit(this IBuild build, IEnumerable<BuildItem> assemblies, Action<XUnitArgs> config = null)
        {
            XUnitHelper(build).Test(assemblies, config);
        }
    }
}