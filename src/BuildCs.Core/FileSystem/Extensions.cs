using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.FileSystem
{
    public static class Extensions
    {
        public static FileSystemHelper FileSystemHelper(this Build build)
        {
            return build.GetService<FileSystemHelper>();
        }

        public static void CreateDirectory(this Build build, string path)
        {
            FileSystemHelper(build).CreateDirectory(path);
        }

        public static BuildItem CurrentDirectory(this Build build)
        {
            return FileSystemHelper(build).CurrentDirectory();
        }

        public static void DeleteDirectory(this Build build, string path)
        {
            FileSystemHelper(build).DeleteDirectory(path);
        }

        public static void DeleteFile(this Build build, string path)
        {
            FileSystemHelper(build).DeleteFile(path);
        }

        public static BuildItem Directory(this Build build, string path)
        {
            return path;
        }

        public static bool DirectoryExists(this Build build, string path)
        {
            return FileSystemHelper(build).DirectoryExists(path);
        }

        public static BuildItem File(this Build build, string path)
        {
            return path;
        }

        public static bool FileExists(this Build build, string path)
        {
            return FileSystemHelper(build).FileExists(path);
        }

        public static BuildGlob Glob(this Build build, string pattern)
        {
            return new BuildGlob().Include(pattern);
        }
    }
}