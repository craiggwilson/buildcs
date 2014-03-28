using System;
using System.Collections.Generic;
using System.Linq;
using BuildCs.Targetting;

namespace BuildCs.Tracing
{
    public class Tracer
    {
        private readonly BuildContext _context;

        public Tracer(BuildContext context)
        {
            _context = context;
        }

        public IDisposable StartBuild(BuildExecution build)
        {
            Publish(new StartBuildEvent(build));
            return new StartStop(this, new StopBuildEvent(build));
        }

        public IDisposable StartTarget(BuildExecution build, TargetExecution target)
        {
            Publish(new StartTargetEvent(build, target));
            return new StartStop(this, new StopTargetEvent(build, target));
        }

        public IDisposable StartTask(string name)
        {
            Publish(new StartTaskEvent(name));
            return new StartStop(this, new StopTaskEvent(name));
        }

        public void Trace(string message, params object[] args)
        {
            Write(MessageLevel.Trace, message, args);
        }

        public void Log(string message, params object[] args)
        {
            Write(MessageLevel.Log, message, args);
        }

        public void Info(string message, params object[] args)
        {
            Write(MessageLevel.Info, message, args);
        }

        public void Important(string message, params object[] args)
        {
            Write(MessageLevel.Info, message, args);
        }

        public void Error(string message, params object[] args)
        {
            Write(MessageLevel.Error, message, args);
        }

        public void Write(MessageLevel type, string message, params object[] args)
        {
            if (_context.Verbosity > type)
                return;

            if (args != null && args.Length > 0)
                message = string.Format(message, args);
            Publish(new MessageEvent(type, message));
        }

        private void Publish(BuildEvent @event)
        {
            _context.Listeners.Each(x => x.Handle(@event));
        }

        private class StartStop : IDisposable
        {
            private readonly BuildEvent _event;
            private readonly Tracer _tracer;

            public StartStop(Tracer tracer, BuildEvent @event)
            {
                _tracer = tracer;
                _event = @event;
            }

            public void Dispose()
            {
                _tracer.Publish(_event);
            }
        }
    }
}