﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Tracing
{
    public class StartTaskEvent : BuildEvent
    {
        public StartTaskEvent(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override BuildEventType Type
        {
            get { return BuildEventType.StartTask; }
        }
    }
}