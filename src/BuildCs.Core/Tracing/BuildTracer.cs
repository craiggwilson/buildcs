using System;
using System.Collections.Generic;

namespace BuildCs.Tracing
{
    public class BuildTracer
    {
        private readonly List<IBuildTraceListener> _listeners;
        private string _currentPrefix;

        public BuildTracer()
        {
            _currentPrefix = "";
            _listeners = new List<IBuildTraceListener>();
            _listeners.Add(new ConsoleBuildTracerListener());
        }

        public void AddListener(IBuildTraceListener listener)
        {
            _listeners.Add(listener);
        }

        public void ClearListeners()
        {
            _listeners.Clear();
        }

        public IDisposable WithPrefix(string prefix)
        {
            var prefixer = new Prefixer(this, _currentPrefix);
            _currentPrefix += prefix;
            return prefixer;
        }

        public void Log(string message, params object[] args)
        {
            Write(BuildMessageType.Log, message, args);
        }

        public void Info(string message, params object[] args)
        {
            Write(BuildMessageType.Info, message, args);
        }

        public void Important(string message, params object[] args)
        {
            Write(BuildMessageType.Info, message, args);
        }

        public void Success(string message, params object[] args)
        {
            Write(BuildMessageType.Success, message, args);
        }

        public void Error(string message, params object[] args)
        {
            Write(BuildMessageType.Error, message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            Write(BuildMessageType.Fatal, message, args);
        }

        private void Write(BuildMessageType type, string message, object[] args)
        {
            if (args != null && args.Length > 0)
                message = string.Format(message, args);
            _listeners.Each(x => x.Write(new BuildMessage(type, _currentPrefix + message)));
        }

        private class Prefixer : IDisposable
        {
            private readonly string _previousPrefix;
            private readonly BuildTracer _tracer;

            public Prefixer(BuildTracer tracer, string previousPrefix)
            {
                _tracer = tracer;
                _previousPrefix = previousPrefix;
            }

            public void Dispose()
            {
                _tracer._currentPrefix = _previousPrefix;
            }
        }
    }
}