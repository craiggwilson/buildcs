using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public static class BuildParametersExtensions
    {
        public static string GetParameter(this Build build, string name)
        {
            return build.CommandLine.GetParameter(name);
        }

        public static string GetParameterOrDefault(this Build build, string name, string defaultValue)
        {
            if (build.CommandLine.HasParameter(name))
                return build.GetParameter(name);

            return defaultValue;
        }

        public static bool HasParameter(this Build build, string name)
        {
            return build.CommandLine.HasParameter(name);
        }
    }
}
