using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildCs.FileSystem;

namespace BuildCs.MongoDB
{
    public class StandAloneArgs : MongoDBArgsBase
    {
        public BuildItem DataDir { get; set; }

        public int? OplogSize { get; set; }

        public bool SmallFiles { get; set; }
    }
}