using System;

namespace BuildCs.Services.Tracing
{
    public class ConsoleBuildTracerListener : IBuildTraceListener
    {
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


            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message.Message);
            Console.ForegroundColor = old;
        }
    }
}