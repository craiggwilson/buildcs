﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Services.Tracing
{
    public enum BuildMessageType
    {
        Log,
        Info,
        Important,
        Success,
        Error,
        Fatal
    }
}
