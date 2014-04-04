using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Tracing;

namespace BuildCs
{
    /// <summary>
    /// Root object for a build.  Provides access to singleton services.
    /// </summary>
    public interface IBuildSession : IDisposable
    {
        IDictionary<string, string> Parameters { get; }

        MessageLevel Verbosity { get; set; }

        T GetService<T>();
    }
}