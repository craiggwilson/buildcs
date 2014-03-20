using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public class BuildTargetRunContext : BuildContextBase
    {
        private readonly IBuildTarget _target;
        private TimeSpan _duration;
        private Exception _exception;
        private bool _hasRun;
        private bool _wasSkipped;

        public BuildTargetRunContext(IBuildTarget target)
        {
            _target = target;
        }

        public TimeSpan Duration
        {
            get { return _duration; }
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public bool HasRun
        {
            get { return _hasRun; }
        }

        public bool WasSkipped
        {
            get { return _wasSkipped; }
        }

        public IBuildTarget Target
        {
            get { return _target; }
        }

        internal void MarkFailed(TimeSpan duration, Exception exception)
        {
            _duration = duration;
            _exception = exception;
            _hasRun = true;
        }

        internal void MarkSkipped()
        {
            _hasRun = true;
        }

        internal void MarkSuccessful(TimeSpan duration)
        {
            _duration = duration;
            _hasRun = true;
        }
    }
}