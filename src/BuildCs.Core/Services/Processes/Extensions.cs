using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildCs.Services.Processes;

namespace BuildCs
{
    public static partial class Extensions
    {
        public static BuildProcessRunner ProcessHelper(this Build build)
        {
            return build.GetService<BuildProcessRunner>();
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
            });
        }

        public static int Exec(this Build build, Action<ProcessArgs> config)
        {
            return ProcessHelper(build).Run(config);
        }
    }
}