using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BuildCs.Tracing
{
    public class ConsoleListener : IBuildListener
    {
        private string _currentPrefix;

        public ConsoleListener()
        {
            _currentPrefix = "";
        }

        public void Handle(BuildEvent @event)
        {
            switch(@event.Type)
            {
                case BuildEventType.StartTarget:
                    var name = ((StartTargetEvent)@event).Target.Name;
                    _currentPrefix = "[{0}] ".F(name);
                    Write(ConsoleColor.Green, "Starting at {0:HH:mm:ss}".F(DateTime.UtcNow));
                    return;
                case BuildEventType.StopTarget:
                    var duration = ((StopTargetEvent)@event).Target.Duration;
                    Write(ConsoleColor.Green, "Finished in {0}".F(duration));
                    _currentPrefix = "";
                    return;
                case BuildEventType.StartTask:
                case BuildEventType.StopTask:
                case BuildEventType.StartBuild:
                case BuildEventType.StopBuild:
                    return;
            }

            var message = (MessageEvent)@event;

            var color = ConsoleColor.DarkGray;
            switch (message.Level)
            {
                case MessageLevel.Log:
                    color = ConsoleColor.Gray;
                    break;
                case MessageLevel.Info:
                    color = ConsoleColor.White;
                    break;
                case MessageLevel.Warning:
                    color = ConsoleColor.Yellow;
                    break;
                case MessageLevel.Error:
                    color = ConsoleColor.Red;
                    break;
            }

            Write(color, message.Message);
        }

        private void Write(ConsoleColor color, string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(_currentPrefix + message);
            Console.ForegroundColor = oldColor;
        }
    }
}