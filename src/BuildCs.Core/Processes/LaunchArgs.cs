using System;
using System.Diagnostics;

namespace BuildCs.Processes
{
    public class LaunchArgs : ProcessArgsBase
    {
        public LaunchArgs(ProcessStartInfo startInfo)
            : base(startInfo)
        {
            TraceOutput = false;
        }
    }
}