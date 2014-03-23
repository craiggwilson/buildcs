using System;

namespace BuildCs.Services.Tracing
{
    public class ConsoleBuildTracerListener : IBuildTraceListener
    {
        private ConsoleColor _current;

        public void Write(BuildMessage message)
        {
            var color = ConsoleColor.DarkGray;
            switch(message.Type)
            {
                case BuildMessageType.Info:
                    color = ConsoleColor.White;
                    break;
                case BuildMessageType.Important:
                    color = ConsoleColor.Yellow;
                    break;
                case BuildMessageType.Success:
                    color = ConsoleColor.Green;
                    break;
                case BuildMessageType.Error:
                    color = ConsoleColor.DarkRed;
                    break;
                case BuildMessageType.Fatal:
                    color = ConsoleColor.Red;
                    break;
            }

            if (_current != color)
                _current = Console.ForegroundColor = color;
            Console.WriteLine(message.Message);
        }
    }
}