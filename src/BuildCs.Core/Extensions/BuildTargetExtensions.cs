using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Extensions
{
    public static class BuildTargetExtensions
    {
        public static IBuildTargetBuilder Target(this Build build, string name)
        {
            return build.Targets.AddTarget(name);
        }
    }
}