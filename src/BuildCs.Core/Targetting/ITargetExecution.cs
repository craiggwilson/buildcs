using System;

namespace BuildCs.Targetting
{
    public interface ITargetExecution
    {
        IBuildExecution Build { get; }

        string Description { get; }

        TimeSpan Duration { get; }

        Exception Exception { get; }

        string Message { get; }

        string Name { get; }

        TargetExecutionStatus Status { get; }

        void MarkFailed(string message, Exception exception);

        void MarkSkipped(string message);
    }
}