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
        public StopBuildEvent(BuildExecution build)
        {
            Build = build;
        }

        public BuildExecution Build { get; private set; }

        public override BuildEventType Type
        {
            get { return BuildEventType.StopBuild; }
        }
    }
}