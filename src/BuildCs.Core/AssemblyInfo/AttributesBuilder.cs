using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.AssemblyInfo
{
    public class AttributesBuilder
    {
        private readonly List<AssemblyInfoAttribute> _attributes;

        public AttributesBuilder()
        {
            _attributes = new List<AssemblyInfoAttribute>();
        }

        public IReadOnlyList<AssemblyInfoAttribute> Attributes
        {
            get { return _attributes; }
        }

        public void Add(string @namespace, string name, params string[] values)
        {
            _attributes.Add(new AssemblyInfoAttribute(@namespace, name, values));
        }

        public void AddBoolean(string @namespace, string name, bool value)
        {
            Add(name, value.ToString(), @namespace);
        }

        public void AddString(string @namespace, string name, string value)
        {
            Add(@namespace, name, "\"{0}\"".F(value));
        }

        public void Company(string value)
        {
            AddString("System.Reflection", "AssemblyCompany", value);
        }

        public void Configuration(string value)
        {
            AddString("System.Reflection", "AssemblyConfiguration", value);
        }

        public void Copyright(string value)
        {
            AddString("System.Reflection", "AssemblyCopyright", value);
        }

        public void Culture(string value)
        {
            AddString("System.Reflection", "AssemblyCulture", value);
        }

        public void DelaySign(bool value)
        {
            AddBoolean("System.Reflection", "AssemblyDelaySign", value);
        }

        public void Description(string value)
        {
            AddString("System.Reflection", "AssemblyDescription", value);
        }

        public void FileVersion(Version value)
        {
            AddString("System.Reflection", "AssemblyFileVersion", value.ToString());
        }

        public void KeyFile(string value)
        {
            AddString("System.Reflection", "AssemblyKeyFile", value);
        }

        public void KeyName(string value)
        {
            AddString("System.Reflection", "AssemblyKeyName", value);
        }

        public void InformationalVersion(string value)
        {
            AddString("System.Reflection", "AssemblyInformationalVersion", value);
        }

        public void Product(string value)
        {
            AddString("System.Reflection", "AssemblyProduct", value);
        }

        public void Title(string value)
        {
            AddString("System.Reflection", "AssemblyTitle", value);
        }

        public void Trademark(string value)
        {
            AddString("System.Reflection", "AssemblyTrademark", value);
        }

        public void Version(Version value)
        {
            AddString("System.Reflection", "AssemblyVersion", value.ToString());
        }

        public void CLSCompliant(bool value = false)
        {
            AddBoolean("System", "CLSCompliant", value);
        }

        public void ComVisible(bool value = false)
        {
            AddBoolean("System.Runtime.InteropServices", "ComVisible", value);
        }

        public void Guid(Guid value)
        {
            AddString("System.Runtime.InteropServices", "Guid", value.ToString());
        }

        public void InternalsVisibleTo(string value)
        {
            AddString("System.Runtime.CompilerServices", "InternalsVisibleTo", value);
        }
    }
}