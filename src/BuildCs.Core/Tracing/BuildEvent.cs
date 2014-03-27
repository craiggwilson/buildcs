using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Tracing
{
    public abstract class BuildEvent
    {
        public abstract BuildEventType Type { get; }
    }
}