using System.Collections.Generic;

namespace BuildCs.Services.Targetting
{
    public interface IBuildTarget
    {
        string Description { get; }

        IReadOnlyList<string> Dependencies { get; }

        string Name { get; }

        void Run();
    }
}