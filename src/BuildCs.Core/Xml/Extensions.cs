using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using BuildCs.FileSystem;

namespace BuildCs.Xml
{
    public static class Extensions
    {
        public static XmlHelper XmlHelper(this IBuildSession session)
        {
            return session.GetService<XmlHelper>();
        }

        public static void XmlUpdate(this IBuildSession session, BuildItem file, Action<XDocument> doc)
        {
            XmlHelper(session).Update(file, doc);
        }

        public static void XmlUpdate(this IBuildSession session, IEnumerable<BuildItem> files, Action<XDocument> doc)
        {
            files.Each(x => XmlUpdate(session, x, doc));
        }
    }
}