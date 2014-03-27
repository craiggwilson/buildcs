using BuildCs;

namespace BuildCs
{
    public static partial class Extensions
    {
        public static BuildContext Arguments(this IBuild build)
        {
            return build.GetService<BuildContext>();
        }

        public static string GetParameter(this IBuild build, string name)
        {
            var parameters = build.Arguments().Parameters;
            string value;
            if (!parameters.TryGetValue(name, out value))
                throw new BuildCsException("Build parameter '{0}' was not supplied.".F(name));

            return value;
        }

        public static string GetParameterOrDefault(this IBuild build, string name, string defaultValue)
        {
            var parameters = build.Arguments().Parameters;
            string value;
            return parameters.TryGetValue(name, out value) ? value : defaultValue;
        }

        public static bool HasParameter(this IBuild build, string name)
        {
            return build.Arguments().Parameters.ContainsKey(name);
        }
    }
}