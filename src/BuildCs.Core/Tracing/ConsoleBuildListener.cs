using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BuildCs.Tracing
{
    public class ConsoleBuildListener : IBuildListener
    {
        private readonly Stack<Context> _prefixes;
        private string _currentPrefix;

        public ConsoleBuildListener()
        {
            _prefixes = new Stack<Context>();
        }

        public void Handle(BuildEvent @event)
        {
            switch(@event.Type)
            {
                case BuildEventType.StartTarget:
                    var name = ((StartTargetEvent)@event).Name;
                    _prefixes.Push(new Context { Name = name, Stopwatch = Stopwatch.StartNew() });
                    _currentPrefix = string.Join("", _prefixes.Reverse().Select(x => "[{0}] ".F(x.Name)));
                    Write(ConsoleColor.Green, "Starting at {0:HH:mm:ss}".F(DateTime.UtcNow));
                    return;
                case BuildEventType.StopTarget:
                    var stopped = _prefixes.Pop();
                    stopped.Stopwatch.Stop();
                    Write(ConsoleColor.Green, "Finished in {0}".F(stopped.Stopwatch.Elapsed));
                    _currentPrefix = string.Join("", _prefixes.Reverse().Select(x => "[{0}] ".F(x.Name)));
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

        private class Context
        {
            public string Name;
            public Stopwatch Stopwatch;
        }
    }
}