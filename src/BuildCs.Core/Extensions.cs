using BuildCs;

namespace BuildCs
{
    public static partial class Extensions
    {
        public static string GetParameter(this IBuildSession session, string name)
        {
            string value;
            if (!session.Parameters.TryGetValue(name, out value))
                throw new BuildCsException("Build parameter '{0}' was not supplied.".F(name));

            return value;
        }

        public static string GetParameterOrDefault(this IBuildSession session, string name, string defaultValue)
        {
            if (session.HasParameter(name))
                return session.GetParameter(name);

            return defaultValue;
        }

        public static bool HasParameter(this IBuildSession session, string name)
        {
            return session.Parameters.ContainsKey(name);
        }
    }
}