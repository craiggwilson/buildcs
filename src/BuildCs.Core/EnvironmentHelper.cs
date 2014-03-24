using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuildCs.FileSystem;

namespace BuildCs
{
    public class EnvironmentHelper
    {
        public bool IsMono
        {
            get
            {
                return ForceMono
                    || Environment.OSVersion.Platform == PlatformID.Unix
                    || Environment.OSVersion.Platform == PlatformID.MacOSX;
            }
        }

        public bool ForceMono { get; set; }
    }
}