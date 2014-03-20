using System;

namespace BuildCs.Services
{
    public class BuildEnvironment
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