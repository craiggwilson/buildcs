using System;

namespace BuildCs.Targetting
{
    public class BuildTargetRunContext
    {
        private readonly IBuildTarget _target;
        private TimeSpan _duration;
        private Exception _exception;
        private BuildTargetStatus _status;

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

        public BuildTargetStatus Status
        {
            get { return _status; }
        }

        public IBuildTarget Target
        {
            get { return _target; }
        }

        internal void MarkFailed(TimeSpan duration, Exception exception)
        {
            _duration = duration;
            _exception = exception;
            _status = BuildTargetStatus.Failed;
        }

        internal void MarkSkipped()
        {
            _status = BuildTargetStatus.Skipped;
        }

        internal void MarkSuccessful(TimeSpan duration)
        {
            _duration = duration;
            _status = BuildTargetStatus.Success;
        }
    }
}