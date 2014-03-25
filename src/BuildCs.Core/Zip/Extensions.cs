using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Zip
{
    public static class Extensions
    {
        public static ZipHelper ZipHelper(this IBuild build)
        {
            return build.GetService<ZipHelper>();
        }

        public static void Zip(this IBuild build, BuildItem outputPath, Action<ZipArgs> config)
        {
            ZipHelper(build).Zip(outputPath, config);
        }
    }
}