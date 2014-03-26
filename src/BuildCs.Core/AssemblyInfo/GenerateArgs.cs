using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildCs.AssemblyInfo
{
    public class GenerateArgs
    {
        private readonly List<AssemblyInfoAttribute> _attributes;

        public GenerateArgs()
        {
            _attributes = new List<AssemblyInfoAttribute>();
        }

        public IReadOnlyList<AssemblyInfoAttribute> Attributes
        {
            get { return _attributes; }
        }

        public void AddAttribute(string @namespace, string name, params string[] values)
        {
            _attributes.Add(new AssemblyInfoAttribute(@namespace, name, values));
        }

        public void AddBooleanAttribute(string @namespace, string name, bool value)
        {
            AddAttribute(name, value.ToString(), @namespace);
        }

        public void AddStringAttribute(string @namespace, string name, string value)
        {
            AddAttribute(@namespace, name, "\"{0}\"".F(value));
        }

        public void AddAssemblyCompanyAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyCompany", value);
        }

        public void AddAssemblyConfigurationAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyConfiguration", value);
        }

        public void AddAssemblyCopyrightAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyCopyright", value);
        }

        public void AddAssemblyCultureAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyCulture", value);
        }

        public void AddAssemblyDelaySignAttribute(bool value)
        {
            AddBooleanAttribute("System.Reflection", "AssemblyDelaySign", value);
        }

        public void AddAssemblyDescriptionAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyDescription", value);
        }

        public void AddAssemblyFileVersionAttribute(Version value)
        {
            AddStringAttribute("System.Reflection", "AssemblyFileVersion", value.ToString());
        }

        public void AddAssemblyKeyFileAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyKeyFile", value);
        }

        public void AddAssemblyKeyNameAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyKeyName", value);
        }

        public void AddAssemblyInformationalVersionAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyInformationalVersion", value);
        }

        public void AddAssemblyProductAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyProduct", value);
        }

        public void AddAssemblyTitleAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyTitle", value);
        }

        public void AddAssemblyTrademarkAttribute(string value)
        {
            AddStringAttribute("System.Reflection", "AssemblyTrademark", value);
        }

        public void AddAssemblyVersionAttribute(Version value)
        {
            AddStringAttribute("System.Reflection", "AssemblyVersion", value.ToString());
        }

        public void AddCLSCompliantAttribute(bool value = false)
        {
            AddBooleanAttribute("System", "CLSCompliant", value);
        }

        public void AddComVisibleAttribute(bool value = false)
        {
            AddBooleanAttribute("System.Runtime.InteropServices", "ComVisible", value);
        }

        public void AddGuidAttribute(Guid value)
        {
            AddStringAttribute("System.Runtime.InteropServices", "Guid", value.ToString());
        }

        public void AddInternalsVisibleToAttribute(string value)
        {
            AddStringAttribute("System.Runtime.CompilerServices", "InternalsVisibleTo", value);
        }
    }
}