using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildCs.FileSystem;

namespace BuildCs.MongoDB
{
    public abstract class MongoDBArgsBase
    {
        public BuildItem BinDir { get; set; }

        public BuildItem LogPath { get; set; }

        public int Port { get; set; }
    }
}