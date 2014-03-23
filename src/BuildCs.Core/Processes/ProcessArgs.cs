using System;
using System.Diagnostics;

namespace BuildCs.Processes
{
    public class ProcessArgs
    {
        public ProcessArgs(ProcessStartInfo startInfo)
        {
            TraceOutput = true;
            StartInfo = startInfo;
            Timeout = System.Threading.Timeout.InfiniteTimeSpan;
        }

        public bool TraceOutput { get; set; }

        public Action<string> OnErrorMessage { get; set; }

        public Action<string> OnOutputMessage { get; set; }

        public ProcessStartInfo StartInfo { get; private set; }

        public TimeSpan Timeout { get; set; }
    }
}
