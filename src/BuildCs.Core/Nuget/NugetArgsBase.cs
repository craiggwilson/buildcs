using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Nuget
{
    public abstract class NugetArgsBase
    {
        public TimeSpan? Timeout { get; set; }

        public BuildItem ToolPath { get; set; }

        public NugetVerbosity? Verbosity { get; set; }
    }
}