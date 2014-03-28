using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Targetting
{
    public enum TargetExecutionStatus
    {
        NotRun = 0,
        Skipped,
        Success,
        Failed
    }
}