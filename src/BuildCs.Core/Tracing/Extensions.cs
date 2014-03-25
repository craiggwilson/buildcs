
namespace BuildCs.Tracing
{
    public static partial class Extensions
    {
        public static BuildTracer Tracer(this IBuild build)
        {
            return build.GetService<BuildTracer>();
        }

        public static void Log(this IBuild build, string message)
        {
            build.Tracer().Log(message);
        }

        public static void Log(this IBuild build, string format, params object[] args)
        {
            build.Tracer().Log(format, args);
        }
    }
}