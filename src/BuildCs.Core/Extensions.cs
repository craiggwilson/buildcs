using BuildCs;

namespace BuildCs
{
    public static partial class Extensions
    {
        public static CommandLineHelper CommandLine(this IBuild build)
        {
            return build.GetService<CommandLineHelper>();
        }

        public static string GetParameter(this IBuild build, string name)
        {
            return build.CommandLine().GetParameter(name);
        }

        public static string GetParameterOrDefault(this IBuild build, string name, string defaultValue)
        {
            if (build.CommandLine().HasParameter(name))
                return build.GetParameter(name);

            return defaultValue;
        }

        public static bool HasParameter(this IBuild build, string name)
        {
            return build.CommandLine().HasParameter(name);
        }
    }
}