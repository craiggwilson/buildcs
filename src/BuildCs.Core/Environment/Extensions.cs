using BuildCs;

namespace BuildCs.Environment
{
    public static class Extensions
    {
        public static EnvironmentHelper EnvironmentHelper(this IBuildSession sesion)
        {
            return sesion.GetService<EnvironmentHelper>();
        }

        public static string GetEnvironmentVariable(this IBuildSession session, string name)
        {
            return EnvironmentHelper(session).GetVariable(name);
        }

        public static string GetEnvironmentVariableOrDefault(this IBuildSession session, string name, string defaultValue)
        {
            return EnvironmentHelper(session).GetVariableOrDefault(name, defaultValue);
        }

        public static bool HasEnvironmentVariable(this IBuildSession session, string name)
        {
            return EnvironmentHelper(session).HasVariable(name);
        }

        public static void SetEnvironmentVariable(this IBuildSession session, string name, string value)
        {
            EnvironmentHelper(session).SetVariable(name, value);
        }
    }
}