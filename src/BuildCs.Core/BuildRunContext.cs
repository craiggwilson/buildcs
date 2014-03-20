using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public class BuildRunContext : BuildContextBase
    {
        private readonly IEnumerable<BuildTargetRunContext> _targetChain;

        public BuildRunContext(IEnumerable<BuildTargetRunContext> targetChain)
        {
            _targetChain = targetChain;
        }

        public IEnumerable<BuildTargetRunContext> TargetChain
        {
            get { return _targetChain; }
        }
    }
}