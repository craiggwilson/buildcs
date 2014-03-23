using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;
using D = System.IO.Directory;
using F = System.IO.File;

namespace BuildCs
{
    public static partial class Extensions
    {
        public static void CreateDirectory(this Build build, string path)
        {
            if (!D.Exists(path))
                D.CreateDirectory(path);
        }

        public static BuildItem CurrentDirectory(this Build build)
        {
            return D.GetCurrentDirectory();
        }

        public static void DeleteDirectory(this Build build, string path)
        {
            if (D.Exists(path))
                D.Delete(path, true);
        }

        public static void DeleteFile(this Build build, string path)
        {
            if(F.Exists(path))
                F.Delete(path);
        }

        public static BuildItem Directory(this Build build, string path)
        {
            return path;
        }

        public static BuildItem File(this Build build, string path)
        {
            return path;
        }

        public static BuildGlob Glob(this Build build, string pattern)
        {
            return new BuildGlob().Include(pattern);
        }
    }
}