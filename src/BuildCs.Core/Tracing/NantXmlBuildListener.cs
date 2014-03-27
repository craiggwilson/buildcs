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
    public class NantXmlBuildListener : IBuildListener
    {
        private readonly Stack<Stopwatch> _stopwatchStack;
        private readonly string _filename;
        private readonly MessageLevel _threshold;
        private XmlWriter _writer;

        public NantXmlBuildListener(string filename, MessageLevel threshold)
        {
            _filename = filename;
            _stopwatchStack = new Stack<Stopwatch>();
            _threshold = threshold;
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
            if(@event.Level >= _threshold && !string.IsNullOrWhiteSpace(@event.Message))
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
            _writer = XmlWriter.Create(_filename, settings);
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
        }

        private void Handle(StartTargetEvent @event)
        {
            _writer.WriteStartElement("target");
            _writer.WriteAttributeString("name", @event.Name);
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