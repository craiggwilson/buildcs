using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Targetting
{
    public class BuildExecution
    {
        public BuildExecution(IEnumerable<TargetExecution> targets)
        {
            Targets = targets.ToList();
        }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromTicks(Targets.Select(x => x.Duration)
                    .DefaultIfEmpty(TimeSpan.Zero)
                    .Sum(x => x.Ticks));
            }
        }

        public Exception Exception
        {
            get
            {
                return Targets.Select(x => x.Exception)
                    .Where(x => x != null)
                    .FirstOrDefault();
            }
        }

        public BuildExecutionStatus Status
        {
            get
            {
                var maxStatus = Targets.Select(x => x.Status)
                    .DefaultIfEmpty(TargetExecutionStatus.NotRun)
                    .Max();

                return (BuildExecutionStatus)maxStatus;
            }
        }

        public IReadOnlyList<TargetExecution> Targets { get; private set; }
    }
}