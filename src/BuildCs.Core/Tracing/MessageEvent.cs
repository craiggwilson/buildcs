
namespace BuildCs.Tracing
{
    public class MessageEvent : BuildEvent
    {
        public MessageEvent(MessageLevel level, string message)
        {
            Level = level;
            Message = message;
        }

        public MessageLevel Level { get; private set; }

        public string Message { get; private set; }

        public override BuildEventType Type
        {
            get { return BuildEventType.Message; }
        }
    }
}