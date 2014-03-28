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
        public static FileSystemHelper FileSystemHelper(this IBuildSession session)
        {
            return session.GetService<FileSystemHelper>();
        }

        public static void CreateDirectory(this IBuildSession session, string path)
        {
            FileSystemHelper(session).CreateDirectory(path);
        }

        public static BuildItem CurrentDirectory(this IBuildSession session)
        {
            return FileSystemHelper(session).CurrentDirectory();
        }

        public static void DeleteDirectory(this IBuildSession session, string path)
        {
            FileSystemHelper(session).DeleteDirectory(path);
        }

        public static void DeleteFile(this IBuildSession session, string path)
        {
            FileSystemHelper(session).DeleteFile(path);
        }

        public static BuildItem Directory(this IBuildSession session, string path)
        {
            return path;
        }

        public static bool DirectoryExists(this IBuildSession session, string path)
        {
            return FileSystemHelper(session).DirectoryExists(path);
        }

        public static BuildItem File(this IBuildSession session, string path)
        {
            return path;
        }

        public static bool FileExists(this IBuildSession session, string path)
        {
            return FileSystemHelper(session).FileExists(path);
        }

        public static BuildGlob Glob(this IBuildSession session, string pattern)
        {
            return new BuildGlob().Include(pattern);
        }
    }
}