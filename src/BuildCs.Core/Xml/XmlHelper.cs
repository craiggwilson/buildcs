using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BuildCs.FileSystem;

namespace BuildCs.Xml
{
    public class XmlHelper
    {
        public void Update(BuildItem file, Action<XDocument> config)
        {
            var doc = XDocument.Load(file);
            config(doc);
            doc.Save(file);
        }
    }
}