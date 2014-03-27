using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    /// <summary>
    /// Root object for a build.  Provides access to singleton services.
    /// </summary>
    public interface IBuild
    {
        BuildContext Context { get; }

        T GetService<T>();
    }
}