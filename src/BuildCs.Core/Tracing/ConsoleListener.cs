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
                case BuildEventType.Message:
                    HandleMessage((MessageEvent)@event);
                    break;
                case BuildEventType.StartTarget:
                    HandleStartTarget(((StartTargetEvent)@event).Target);
                    break;
                case BuildEventType.StopTarget:
                    HandleStopTarget(((StopTargetEvent)@event).Target);
                    break;
                case BuildEventType.StartTask:
                case BuildEventType.StopTask:
                case BuildEventType.StartBuild:
                    return;
                case BuildEventType.StopBuild:
                    PrintSummary((StopBuildEvent)@event);
                    return;
            }
        }

        private void HandleStartTarget(ITargetExecution target)
        {
            _currentPrefix = "[{0}] ".F(target.Name);
            Write(ConsoleColor.Green, "Starting at {0:HH:mm:ss}".F(DateTime.UtcNow));
        }

        private void HandleStopTarget(ITargetExecution target)
        {
            var duration = target.Duration;
            switch (target.Status)
            {
                case TargetExecutionStatus.Skipped:
                    Write(ConsoleColor.Yellow, "Skipped. {0}".F(target.Message ?? "(no reason provided)"));
                    break;
                case TargetExecutionStatus.Failed:
                    var message = target.Message;
                    if (message != null && target.Exception != null)
                        message += " " + target.Exception.ToString();
                    else if (message == null && target.Exception != null)
                        message = target.Exception.ToString();
                    else if(message == null)
                        message = "(no reason provided)";
                    Write(ConsoleColor.Red, "Failed. {0}".F(message));
                    break;
                default:
                    Write(ConsoleColor.Green, "Completed in {0}.".F(target.Duration));
                    break;
            }

            _currentPrefix = "";
        }

        private void HandleMessage(MessageEvent @event)
        {
            var color = ConsoleColor.DarkGray;
            switch (@event.Level)
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

            Write(color, @event.Message);
        }

        private void PrintSummary(StopBuildEvent @event)
        {
            var maxLength = @event.Build.Targets.Max(x => x.Name.Length);
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
            foreach(var target in @event.Build.Targets)
            {
                var text = target.Name.PadRight(maxLength + 4) + target.Duration.ToString();
                switch(target.Status)
                {
                    case TargetExecutionStatus.Failed:
                        Write(ConsoleColor.Red, target.Name.PadRight(maxLength + 4) + "Failed");
                        break;
                    case TargetExecutionStatus.NotRun:
                        Write(ConsoleColor.DarkGray, target.Name.PadRight(maxLength + 4) + "Not Run");
                        break;
                    case TargetExecutionStatus.Skipped:
                        Write(ConsoleColor.Yellow, target.Name.PadRight(maxLength + 4) + "Skipped");
                        break;
                    case TargetExecutionStatus.Success:
                        Write(ConsoleColor.Green, target.Name.PadRight(maxLength + 4) + target.Duration.ToString());
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