
namespace BuildCs.Tracing
{
    public static partial class Extensions
    {
        public static Tracer Tracer(this IBuildSession session)
        {
            return session.GetService<Tracer>();
        }

        public static void Log(this IBuildSession session, string message)
        {
            session.Tracer().Log(message);
        }

        public static void Log(this IBuildSession session, string format, params object[] args)
        {
            session.Tracer().Log(format, args);
        }
    }
}