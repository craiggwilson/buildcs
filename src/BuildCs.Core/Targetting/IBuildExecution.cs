using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Targetting
{
    public interface IBuildExecution
    {
        TimeSpan Duration { get; }

        Exception Exception { get; }

        BuildExecutionStatus Status { get; }

        IReadOnlyList<ITargetExecution> Targets { get; }
    }
}