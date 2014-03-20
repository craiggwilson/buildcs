using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    internal class TextWriterBuildTracer : IBuildTracer
    {
        private readonly TextWriter _writer;
        private string _currentPrefix;

        public TextWriterBuildTracer(TextWriter writer)
        {
            _writer = writer;
            _currentPrefix = "";
        }

        public IDisposable Prefix(string prefix)
        {
            var prefixer = new Prefixer(this, _currentPrefix);
            _currentPrefix += prefix;
            return prefixer;
        }

        public void Trace(string message)
        {
            _writer.WriteLine(_currentPrefix + message);
        }

        public void Trace(string message, params object[] args)
        {
            _writer.WriteLine(_currentPrefix + message, args);
        }

        private class Prefixer : IDisposable
        {
            private readonly string _previousPrefix;
            private readonly TextWriterBuildTracer _tracer;

            public Prefixer(TextWriterBuildTracer tracer, string previousPrefix)
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