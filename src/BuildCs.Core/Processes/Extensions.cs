using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildCs.Processes;

namespace BuildCs
{
    public static partial class Extensions
    {
        public static ProcessHelper ProcessHelper(this Build build)
        {
            return build.GetService<ProcessHelper>();
        }

        public static int Exec(this Build build, string filename, string arguments)
        {
            return Exec(build, filename, arguments, Timeout.InfiniteTimeSpan);
        }

        public static int Exec(this Build build, string filename, string arguments, TimeSpan timeout)
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

        public static int Exec(this Build build, Action<ProcessConfig> config)
        {
            return ProcessHelper(build).Exec(config);
        }
    }
}