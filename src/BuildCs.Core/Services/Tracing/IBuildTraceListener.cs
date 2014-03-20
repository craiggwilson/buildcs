
namespace BuildCs.Services.Tracing
{
    public interface IBuildTraceListener
    {
        void Write(BuildMessage message);
    }
}
