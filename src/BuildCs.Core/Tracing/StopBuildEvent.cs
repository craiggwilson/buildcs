using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Targetting;

namespace BuildCs.Tracing
{
    public class StopBuildEvent : BuildEvent
    {
        public StopBuildEvent(IBuildExecution build)
        {
            Build = build;
        }

        public IBuildExecution Build { get; private set; }

        public override BuildEventType Type
        {
            get { return BuildEventType.StopBuild; }
        }
    }
}