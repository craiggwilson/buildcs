using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public static class BuildTracingExtensions
    {
        public static void Trace(this Build build, string message)
        {
            build.Tracer.Trace(message);
        }

        public static void Trace(this Build build, string format, params object[] args)
        {
            build.Tracer.Trace(format, args);
        }
    }
}