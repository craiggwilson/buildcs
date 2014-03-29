using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.SemVer
{
    public static class Extensions
    {
        public static SemVersion SemVer(this IBuildSession session, string versionString)
        {
            return new SemVersion(versionString);
        }
    }
}