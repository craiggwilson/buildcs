
namespace BuildCs.Tracing
{
    public interface IBuildTraceListener
    {
        void Write(BuildMessage message);
    }
}
