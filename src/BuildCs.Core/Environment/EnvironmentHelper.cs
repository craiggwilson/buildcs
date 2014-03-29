using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuildCs.FileSystem;

namespace BuildCs.Environment
{
    public class EnvironmentHelper
    {
        public bool IsMac
        {
            get { return System.Environment.OSVersion.Platform == PlatformID.MacOSX; }
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
            get { return System.Environment.OSVersion.Platform == PlatformID.Unix; }
        }

        public bool ForceMono { get; set; }

        public string GetVariable(string name)
        {
            foreach(DictionaryEntry entry in System.Environment.GetEnvironmentVariables())
            {
                if((string)entry.Key == name)
                    return (string)entry.Value;
            }

            throw new BuildCsException("Environment variable '{0}' has not been set.".F(name));
        }

        public string GetVariableOrDefault(string name, string defaultValue)
        {
            return HasVariable(name)
                ? GetVariable(name)
                : defaultValue;
        }

        public bool HasVariable(string name)
        {
            foreach (DictionaryEntry entry in System.Environment.GetEnvironmentVariables())
            {
                if ((string)entry.Key == name)
                    return true;
            }

            return false;
        }

        public void SetVariable(string name, string value)
        {
            System.Environment.SetEnvironmentVariable(name, value);
        }
    }
}