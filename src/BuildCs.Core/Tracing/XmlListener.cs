using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BuildCs.Tracing
{
    public class XmlListener : IBuildListener
    {
        public static readonly string OutputPathParameterName = "XmlListener.filename";

        private readonly BuildContext _context;
        private readonly Stack<Stopwatch> _stopwatchStack;
        private StringBuilder _buffer;
        private XmlWriter _writer;

        public XmlListener(BuildContext context)
        {
            _context = context;
            _stopwatchStack = new Stack<Stopwatch>();
        }

        public void Handle(BuildEvent @event)
        {
            switch (@event.Type)
            {
                case BuildEventType.Message:
                    Handle((MessageEvent)@event);
                    break;
                case BuildEventType.StartBuild:
                    Handle((StartBuildEvent)@event);
                    return;
                case BuildEventType.StopBuild:
                    Handle((StopBuildEvent)@event);
                    return;
                case BuildEventType.StartTarget:
                    Handle((StartTargetEvent)@event);
                    return;
                case BuildEventType.StopTarget:
                    Handle((StopTargetEvent)@event);
                    return;
                case BuildEventType.StartTask:
                    Handle((StartTaskEvent)@event);
                    return;
                case BuildEventType.StopTask:
                    Handle((StopTaskEvent)@event);
                    return;
            }
        }

        private void Handle(MessageEvent @event)
        {
            if(!string.IsNullOrWhiteSpace(@event.Message))
            {
                _writer.WriteStartElement("message");

                string nantLevel = @event.Level.ToString();
                switch(@event.Level)
                {
                    case MessageLevel.Trace:
                        nantLevel = "Debug";
                        break;
                    case MessageLevel.Log:
                        nantLevel = "Verbose";
                        break;
                }

                _writer.WriteAttributeString("level", nantLevel);
                _writer.WriteString(new XText(@event.Message).ToString());
                _writer.WriteEndElement();
            }
        }

        private void Handle(StartBuildEvent @event)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true
            };
            _buffer = new StringBuilder();
            _writer = XmlWriter.Create(_buffer, settings);
            _writer.WriteStartElement("buildresults");
            _stopwatchStack.Push(Stopwatch.StartNew());
        }

        private void Handle(StopBuildEvent @event)
        {
            WriteDuration();
            _writer.WriteEndElement();
            _writer.Flush();
            _writer.Close();
            _writer.Dispose();
            _writer = null;

            string filename;
            if (!_context.Parameters.TryGetValue(OutputPathParameterName, out filename))
                filename = "build-output.xml";

            File.WriteAllText(filename, _buffer.ToString());
        }

        private void Handle(StartTargetEvent @event)
        {
            _writer.WriteStartElement("target");
            _writer.WriteAttributeString("name", @event.Target.Name);
            _writer.Flush();
            _stopwatchStack.Push(Stopwatch.StartNew());
        }

        private void Handle(StopTargetEvent @event)
        {
            WriteDuration();
            _writer.WriteEndElement();
            _writer.Flush();
        }

        private void Handle(StartTaskEvent @event)
        {
            _writer.WriteStartElement("task");
            _writer.WriteAttributeString("name", @event.Name);
            _writer.Flush();
            _stopwatchStack.Push(Stopwatch.StartNew());
        }

        private void Handle(StopTaskEvent @event)
        {
            WriteDuration();
            _writer.WriteEndElement();
            _writer.Flush();
        }

        private void WriteDuration()
        {
            var stopwatch = _stopwatchStack.Pop();
            stopwatch.Stop();
            _writer.WriteElementString("duration", XmlConvert.ToString(stopwatch.Elapsed.TotalMilliseconds));
        }
    }
}