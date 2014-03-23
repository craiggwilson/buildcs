using System;

namespace BuildCs.Targetting
{
    public interface IBuildTargetBuilder : IBuildTarget
    {
        IBuildTargetBuilder Describe(string description);

        IBuildTargetBuilder DependsOn(params string[] names);

        IBuildTargetBuilder Do(Action action);

        IBuildTargetBuilder Wrap(Action<Action> action);
    }
}