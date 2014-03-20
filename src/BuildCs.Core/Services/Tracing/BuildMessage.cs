
namespace BuildCs.Services.Tracing
{
    public enum BuildMessageType
    {
        Log,
        Info,
        Important,
        Error
    }

    public class BuildMessage
    {
        private readonly string _message;
        private readonly BuildMessageType _type;

        public BuildMessage(BuildMessageType type, string message)
        {
            _message = message;
            _type = type;
        }

        public string Message
        {
            get { return _message; }
        }

        public BuildMessageType Type
        {
            get { return _type; }
        }
    }
}