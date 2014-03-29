using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Targetting;

namespace BuildCs.Tracing
{
    public class StartTargetEvent : BuildEvent
    {
        public StartTargetEvent(ITargetExecution target)
        {
            Target = target;
        }

        public ITargetExecution Target { get; private set; }

        public override BuildEventType Type
        {
            get { return BuildEventType.StartTarget; }
        }
    }
}