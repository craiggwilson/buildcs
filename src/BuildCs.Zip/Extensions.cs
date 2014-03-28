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
        public static ZipHelper ZipHelper(this IBuildSession session)
        {
            return session.GetService<ZipHelper>();
        }

        public static void Zip(this IBuildSession session, BuildItem outputPath, Action<ZipArgs> config)
        {
            ZipHelper(session).Zip(outputPath, config);
        }
    }
}