using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public interface IBuildTargetBuilder : IBuildTarget
    {
        IBuildTargetBuilder Describe(string description);

        IBuildTargetBuilder DependsOn(params string[] names);

        IBuildTargetBuilder Do(Action action);
    }
}