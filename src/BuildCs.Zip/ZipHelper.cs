using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;
using BuildCs.Tracing;
using Ionic.Zip;

namespace BuildCs.Zip
{
    public class ZipHelper
    {
        private Tracer _tracer;

        public ZipHelper(Tracer tracer)
        {
            _tracer = tracer;
        }

        public void Zip(BuildItem outputPath, Action<ZipArgs> config)
        {
            var args = new ZipArgs();
            if(config != null)
                config(args);

            using(var zipFile = new ZipFile(outputPath))
            {
                zipFile.SaveProgress += (o, e) =>
                {
                    switch(e.EventType)
                    {
                        case ZipProgressEventType.Saving_Started:
                            _tracer.Log("Creating zip file '{0}'.", e.ArchiveName);
                            break;
                        case ZipProgressEventType.Saving_AfterWriteEntry:
                            _tracer.Log("Added {0}.", e.CurrentEntry);
                            break;
                        case ZipProgressEventType.Saving_Completed:
                            _tracer.Log("Added {0} entries to '{1}'.", args.Items.Count, e.ArchiveName);
                            break;
                    }
                };

                foreach(var item in args.Items)
                {
                    if (item.DestinationDir == null)
                        zipFile.AddItem(item.Path);
                    else
                        zipFile.AddItem(item.Path, item.DestinationDir);
                }

                zipFile.Save();
            }
        }
    }
}