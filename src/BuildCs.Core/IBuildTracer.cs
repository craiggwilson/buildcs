using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public interface IBuildTracer
    {
        IDisposable Prefix(string prefix);

        void Trace(string message);

        void Trace(string message, params object[] args);
    }
}