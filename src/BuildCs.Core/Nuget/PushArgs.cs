using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Nuget
{
    public class PushArgs : NugetArgsBase
    {
        public string ApiKey { get; set; }

        public string Source { get; set; }
    }
}