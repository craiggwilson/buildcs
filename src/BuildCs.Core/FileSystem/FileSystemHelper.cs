using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Tracing;

namespace BuildCs.FileSystem
{
    public class FileSystemHelper
    {
        private EnvironmentHelper _environment;
        private BuildTracer _tracer;

        public FileSystemHelper(EnvironmentHelper environment, BuildTracer tracer)
        {
            _environment = environment;
            _tracer = tracer;
        }

        public void CreateDirectory(BuildItem item)
        {
            if (!Directory.Exists(item))
            {
                _tracer.Info("Creating directory '{0}'.", item);
                Directory.CreateDirectory(item);
            }
        }

        public BuildItem CurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public void DeleteDirectory(BuildItem item)
        {
            if (DirectoryExists(item))
            {
                _tracer.Info("Deleting directory '{0}'.", item);
                Directory.Delete(item, true);
            }
        }

        public void DeleteFile(BuildItem item)
        {
            if (File.Exists(item))
            {
                _tracer.Info("Deleting file '{0}'.", item);
                File.Delete(item);
            }
        }

        public bool DirectoryExists(BuildItem item)
        {
            return Directory.Exists(item);
        }

        public bool FileExists(BuildItem item)
        {
            return File.Exists(item);
        }

        public BuildItem FindFile(string name, IEnumerable<BuildItem> directories)
        {
            return directories
                .Select(dir => dir + name)
                .FirstOrDefault(file => FileExists(file));
        }
    }
}