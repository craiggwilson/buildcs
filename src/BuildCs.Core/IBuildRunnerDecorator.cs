using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public interface IBuildRunnerDecorator
    {
        void After(BuildRunContext context);

        void After(BuildTargetRunContext context);

        void Before(BuildRunContext context);

        void Before(BuildTargetRunContext context);
    }
}