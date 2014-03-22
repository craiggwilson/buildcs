using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Services.FileSystem;

namespace BuildCs
{
    public static partial class Extensions
    {
        public static void CreateDirectory(this Build build, string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        public static BuildItem CurrentDirectory(this Build build)
        {
            return System.IO.Directory.GetCurrentDirectory();
        }

        public static void DeleteDirectory(this Build build, string path)
        {
            System.IO.Directory.Delete(path, true);
        }

        public static void DeleteFile(this Build build, string path)
        {
            System.IO.File.Delete(path);
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