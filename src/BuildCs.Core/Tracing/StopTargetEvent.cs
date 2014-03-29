using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Targetting;

namespace BuildCs.Tracing
{
    public class StopTargetEvent : BuildEvent
    {
        public StopTargetEvent(ITargetExecution target)
        {
            Target = target;
        }

        public ITargetExecution Target { get; private set; }

        public override BuildEventType Type
        {
            get { return BuildEventType.StopTarget; }
        }
    }
}