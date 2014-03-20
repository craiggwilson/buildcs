using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public interface IBuildTarget
    {
        Action Action { get; }

        string Description { get; }

        IReadOnlyList<string> Dependencies { get; }

        string Name { get; }
    }
}