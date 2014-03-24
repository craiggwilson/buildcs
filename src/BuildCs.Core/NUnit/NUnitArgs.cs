using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.NUnit
{
    public class NUnitArgs
    {
        public string ExcludeCategory { get; set;}

        public string Fixture { get; set; }

        public string IncludeCategory { get; set; }

        public bool? Labels { get; set; }

        public bool? NoLogo { get; set; }

        public bool? ShadowCopy { get; set; }

        public TimeSpan? Timeout { get; set; }

        public BuildItem ToolPath { get; set; }

        public BuildItem XmlOutputPath { get; set; }

        public BuildItem XsltTransformPath { get; set; }
    }
}