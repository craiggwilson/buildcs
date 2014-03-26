using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.AssemblyInfo
{
    public class AssemblyInfoAttribute
    {
        public AssemblyInfoAttribute(string @namespace, string name, params string[] values)
        {
            Name = name;
            Values = values ?? Enumerable.Empty<string>();
            Namespace = @namespace;
        }

        public string Name { get; private set; }

        public IEnumerable<string> Values { get; private set; }

        public string Namespace { get; private set; }
    }
}