using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public abstract class BuildContextBase
    {
        private readonly Dictionary<string, object> _properties;

        protected BuildContextBase()
        {
            _properties = new Dictionary<string, object>();
        }

        public T GetProperty<T>(string name)
        {
            return (T)Convert.ChangeType(_properties[name], typeof(T));
        }

        public T GetPropertyOrDefault<T>(string name, T defaultValue)
        {
            object value;
            return _properties.TryGetValue(name, out value)
                ? (T)Convert.ChangeType(value, typeof(T))
                : defaultValue;
        }

        public bool HasProperty(string name)
        {
            return _properties.ContainsKey(name);
        }

        public void SetProperty(string name, object value)
        {
            _properties[name] = value;
        }
    }
}
