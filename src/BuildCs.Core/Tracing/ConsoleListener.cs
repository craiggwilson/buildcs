using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BuildCs.Targetting;

namespace BuildCs.Tracing
{
    public class ConsoleListener : IBuildListener
    {
        private string _currentPrefix;

        public ConsoleListener(IBuildSession session) // required ctor...
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
                    return;
                case BuildEventType.StopBuild:
                    PrintSummary((StopBuildEvent)@event);
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

        private void PrintSummary(StopBuildEvent @event)
        {
            var maxLength = @event.Build.Targets.Max(x => x.Target.Name.Length);
            var anyFailed = @event.Build.Targets.Any(x => x.Status == TargetExecutionStatus.Failed);
            var color = ConsoleColor.Green;
            if (anyFailed)
                color = ConsoleColor.Red;

            Write(color, "");
            Write(color, "---------------------------------------------------------------------");
            Write(color, "Build Time Report");
            Write(color, "---------------------------------------------------------------------");
            Write(color, "Target".PadRight(maxLength) + "    Duration");
            Write(color, "------".PadRight(maxLength) + "    --------");
            foreach(var context in @event.Build.Targets)
            {
                var text = context.Target.Name.PadRight(maxLength + 4) + context.Duration.ToString();
                switch(context.Status)
                {
                    case TargetExecutionStatus.Failed:
                        Write(ConsoleColor.Red, context.Target.Name.PadRight(maxLength + 4) + "Failed");
                        break;
                    case TargetExecutionStatus.NotRun:
                        Write(ConsoleColor.DarkGray, context.Target.Name.PadRight(maxLength + 4) + "Not Run");
                        break;
                    case TargetExecutionStatus.Skipped:
                        Write(ConsoleColor.Gray, context.Target.Name.PadRight(maxLength + 4) + "Skipped");
                        break;
                    case TargetExecutionStatus.Success:
                        Write(ConsoleColor.Green, context.Target.Name.PadRight(maxLength + 4) + context.Duration.ToString());
                        break;
                }
            }
            Write(color, "------".PadRight(maxLength) + "    --------");
            var status = anyFailed
                ? "Failed"
                : "Ok";
            Write(color, "Result".PadRight(maxLength + 4) + status);
            Write(color, "---------------------------------------------------------------------");
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