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
        public static ProcessHelper ProcessHelper(this IBuild build)
        {
            return build.GetService<ProcessHelper>();
        }

        public static int Exec(this IBuild build, string filename, string arguments)
        {
            return Exec(build, filename, arguments, Timeout.InfiniteTimeSpan);
        }

        public static int Exec(this IBuild build, string filename, string arguments, TimeSpan timeout)
        {
            return Exec(build, config =>
            {
                config.StartInfo.FileName = filename;
                config.StartInfo.Arguments = arguments;
                config.Timeout = timeout;
                config.OnErrorMessage = m => build.Tracer().Error(m);
                config.OnOutputMessage = m => build.Tracer().Log(m);
            });
        }

        public static int Exec(this IBuild build, Action<ProcessArgs> config)
        {
            return ProcessHelper(build).Exec(config);
        }
    }
}