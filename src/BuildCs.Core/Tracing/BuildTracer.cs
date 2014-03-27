using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildCs.Tracing
{
    public class BuildTracer
    {
        private readonly BuildContext _context;

        public BuildTracer(BuildContext context)
        {
            _context = context;
        }

        public IDisposable StartBuild(IEnumerable<string> targetNames)
        {
            Publish(new StartBuildEvent(targetNames));
            return new StartStop(this, new StopBuildEvent(targetNames));
        }

        public IDisposable StartTarget(string name)
        {
            Publish(new StartTargetEvent(name));
            return new StartStop(this, new StopTargetEvent(name));
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
            private readonly BuildTracer _tracer;

            public StartStop(BuildTracer tracer, BuildEvent @event)
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