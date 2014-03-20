using System;

namespace BuildCs.Services.Tracing
{
    public class ConsoleBuildTracerListener : IBuildTraceListener
    {
        private ConsoleColor _currentColor;

        public void Write(BuildMessage message)
        {
            var color = ConsoleColor.White;
            switch(message.Type)
            {
                case BuildMessageType.Info:
                    color = ConsoleColor.Green;
                    break;
                case BuildMessageType.Important:
                    color = ConsoleColor.Yellow;
                    break;
                case BuildMessageType.Error:
                    color = ConsoleColor.Red;
                    break;
            }

            if (_currentColor != color)
                Console.ForegroundColor = _currentColor = color;

            Console.WriteLine(message.Message);
        }
    }
}