using System.Collections.Generic;

namespace BuildCs.Targetting
{
    public interface IBuildTarget
    {
        string Description { get; }

        IReadOnlyList<string> Dependencies { get; }

        string Name { get; }

        void Run();
    }
}