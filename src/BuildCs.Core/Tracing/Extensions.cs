
namespace BuildCs.Tracing
{
    public static partial class Extensions
    {
        public static BuildTracer Tracer(this Build build)
        {
            return build.GetService<BuildTracer>();
        }

        public static void Log(this Build build, string message)
        {
            build.Tracer().Log(message);
        }

        public static void Log(this Build build, string format, params object[] args)
        {
            build.Tracer().Log(format, args);
        }
    }
}