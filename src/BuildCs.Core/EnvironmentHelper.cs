using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuildCs.FileSystem;

namespace BuildCs
{
    public class EnvironmentHelper
    {
        public bool IsMac
        {
            get { return Environment.OSVersion.Platform == PlatformID.MacOSX; }
        }

        public bool IsLinux
        {
            get { return IsMac || IsUnix; }
        }

        public bool IsMono
        {
            get { return ForceMono || IsLinux; }
        }

        public bool IsUnix
        {
            get { return Environment.OSVersion.Platform == PlatformID.Unix; }
        }

        public bool ForceMono { get; set; }

        public string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }

        public string GetEnvironmentVariableOrDefault(string name, string defaultValue)
        {
            return GetEnvironmentVariable(name) ?? defaultValue;
        }
    }
}