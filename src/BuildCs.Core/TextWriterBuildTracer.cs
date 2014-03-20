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

        public TextWriterBuildTracer(TextWriter writer)
        {
            _writer = writer;
        }

        public void Trace(string message)
        {
            _writer.WriteLine(message);
        }

        public void Trace(string message, params object[] args)
        {
            _writer.WriteLine(message, args);
        }
    }
}