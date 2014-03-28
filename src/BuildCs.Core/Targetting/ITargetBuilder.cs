using System;

namespace BuildCs.Targetting
{
    public interface ITargetBuilder : ITarget
    {
        ITargetBuilder Describe(string description);

        ITargetBuilder DependsOn(params string[] names);

        ITargetBuilder Do(Action action);

        ITargetBuilder Wrap(Action<Action> action);
    }
}