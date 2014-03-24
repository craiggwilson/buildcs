using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Xunit
{
    public static class Extensions
    {
        public static XunitHelper XunitHelper(this Build build)
        {
            return build.GetService<XunitHelper>();
        }

        public static void Xunit(this Build build, BuildItem assembly, Action<XunitArgs> config = null)
        {
            Xunit(build, new[] { assembly }, config);
        }

        public static void Xunit(this Build build, IEnumerable<BuildItem> assemblies, Action<XunitArgs> config = null)
        {
            XunitHelper(build).Test(assemblies, config);
        }
    }
}