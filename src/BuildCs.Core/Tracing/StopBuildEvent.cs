using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Tracing
{
    public class StopBuildEvent : BuildEvent
    {
        public StopBuildEvent(IEnumerable<string> targetNames)
        {
            TargetNames = targetNames.ToList();
        }

        public IEnumerable<string> TargetNames { get; private set; }

        public override BuildEventType Type
        {
            get { return BuildEventType.StopBuild; }
        }
    }
}