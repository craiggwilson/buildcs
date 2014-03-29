using System;

namespace BuildCs.Targetting
{
    public interface ITargetBuilder : ITarget
    {
        ITargetBuilder Describe(string description);

        ITargetBuilder DependsOn(params string[] names);

        ITargetBuilder Do(Action<ITargetExecution> action);

        ITargetBuilder Wrap(Action<ITargetExecution, Action<ITargetExecution>> action);
    }
}