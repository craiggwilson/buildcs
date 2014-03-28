using System.Collections.Generic;

namespace BuildCs.Targetting
{
    public interface ITarget
    {
        string Description { get; }

        IReadOnlyList<string> Dependencies { get; }

        string Name { get; }

        void Run();
    }
}