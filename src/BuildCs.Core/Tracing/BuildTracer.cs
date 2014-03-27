using System;
using System.Collections.Generic;

namespace BuildCs.Tracing
{
    public class BuildTracer
    {
        private readonly List<IBuildListener> _listeners;

        public BuildTracer()
        {
            _listeners = new List<IBuildListener>();
            _listeners.Add(new ConsoleBuildListener());
        }

        public void AddListener(IBuildListener listener)
        {
            _listeners.Add(listener);
        }

        public void ClearListeners()
        {
            _listeners.Clear();
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
            _listeners.Each(x => x.Handle(@event));
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