using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Zip
{
    public class ZipBuildItem
    {
        public ZipBuildItem(BuildItem path, BuildItem destinationDir)
        {
            Path = path;
            DestinationDir = destinationDir;
        }

        public BuildItem DestinationDir { get; private set; }

        public BuildItem Path { get; private set; }
    }
}