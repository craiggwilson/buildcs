using System;

namespace BuildCs.Targetting
{
    public class TargetExecution
    {
        public TargetExecution(ITarget target)
        {
            Duration = TimeSpan.Zero;
            Target = target;
        }

        public string Description
        {
            get { return Target.Description; }
        }

        public TimeSpan Duration { get; private set; }

        public Exception Exception { get; private set; }

        public string Name
        {
            get { return Target.Name; }
        }

        public TargetExecutionStatus Status { get; private set; }

        public ITarget Target { get; private set; }

        public void MarkFailed(TimeSpan duration, Exception exception)
        {
            Duration = duration;
            Exception = exception;
            Status = TargetExecutionStatus.Failed;
        }

        public void MarkSkipped(TimeSpan duration)
        {
            Duration = duration;
            Status = TargetExecutionStatus.Skipped;
        }

        public void MarkSuccessful(TimeSpan duration)
        {
            Duration = duration;
            Status = TargetExecutionStatus.Success;
        }
    }
}