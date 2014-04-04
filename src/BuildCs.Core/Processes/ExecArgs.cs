using System;
using System.Diagnostics;

namespace BuildCs.Processes
{
    public class ExecArgs : ProcessArgsBase
    {
        public ExecArgs(ProcessStartInfo startInfo)
            : base(startInfo)
        {
            TraceOutput = true;
            Timeout = System.Threading.Timeout.InfiniteTimeSpan;
        }

        public TimeSpan Timeout { get; set; }
    }
}
