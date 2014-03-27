using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Tracing
{
    public enum BuildEventType
    {
        Message,
        StartBuild,
        StopBuild,
        StartTarget,
        StopTarget,
        StartTask,
        StopTask
    }
}