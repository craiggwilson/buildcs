using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.BuildServer
{
    public enum BuildServerType
    {
        Unknown,
        TeamCity,
        Jenkins,
        CCNet,
        Travis
    }
}