using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildCs.Tracing;

namespace BuildCs.Processes
{
    public static class Extensions
    {
        public static ProcessHelper ProcessHelper(this IBuildSession session)
        {
            return session.GetService<ProcessHelper>();
        }

        public static int Exec(this IBuildSession session, string filename, string arguments)
        {
            return Exec(session, filename, arguments, Timeout.InfiniteTimeSpan);
        }

        public static int Exec(this IBuildSession session, string filename, string arguments, TimeSpan timeout)
        {
            return Exec(session, config =>
            {
                config.StartInfo.FileName = filename;
                config.StartInfo.Arguments = arguments;
                config.Timeout = timeout;
                config.OnErrorMessage = m => session.Tracer().Error(m);
                config.OnOutputMessage = m => session.Tracer().Log(m);
            });
        }

        public static int Exec(this IBuildSession session, Action<ProcessArgs> config)
        {
            return ProcessHelper(session).Exec(config);
        }
    }
}