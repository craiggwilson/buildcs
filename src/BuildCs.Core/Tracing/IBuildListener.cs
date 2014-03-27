
namespace BuildCs.Tracing
{
    public interface IBuildListener
    {
        void Handle(BuildEvent @event);
    }
}