using System;
using System.Diagnostics;

namespace BuildCs.Processes
{
    public abstract class ProcessArgsBase
    {
        protected ProcessArgsBase(ProcessStartInfo startInfo)
        {
            StartInfo = startInfo;
        }

        public bool TraceOutput { get; set; }

        public Action<string> OnErrorMessage { get; set; }

        public Action<string> OnOutputMessage { get; set; }

        public ProcessStartInfo StartInfo { get; private set; }
    }
}
