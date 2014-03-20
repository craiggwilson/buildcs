using System;

namespace BuildCs.Services.Processes
{
    public class ProcessArgs
    {
        public ProcessArgs()
        {
            Timeout = System.Threading.Timeout.InfiniteTimeSpan;
        }

        public bool CatchMessages { get; set; }

        public Action<string> OutputMessage { get; set; }

        public Action<string> ErrorMessage { get; set; }

        public TimeSpan Timeout { get; set; }
    }
}
