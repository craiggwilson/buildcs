using System;
using System.Diagnostics;

namespace BuildCs.Processes
{
    public class ProcessArgs
    {
        internal ProcessArgs(ProcessStartInfo startInfo)
        {
            StartInfo = startInfo;
        }

        public Action KillAction { get; set; }

        public Action<string> OnErrorMessage { get; set; }

        public Action<string> OnOutputMessage { get; set; }

        public ProcessStartInfo StartInfo { get; private set; }

        public TimeSpan Timeout { get; set; }

        public bool TraceOutput { get; set; }
    }
}