using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BuildCs;
using ScriptCs.Contracts;

namespace ScriptCs.BuildCs
{
    public class Build : IBuild, IScriptPackContext
    {
        private readonly BuildContext _context;
        private readonly Dictionary<Type, object> _services;

        public Build(BuildContext context)
        {
            _context = context;
            _services = new Dictionary<Type, object>();
            _services.Add(typeof(Build), this);
            _services.Add(typeof(BuildContext), _context);
        }

        public BuildContext Context
        {
            get { return _context; }
        }

        public T GetService<T>()
        {
            object service;
            if (!_services.TryGetValue(typeof(T), out service))
                service = LoadService(new List<Type>(), typeof(T));

            return (T)service;
        }

        private object LoadService(List<Type> loadedTypes, Type type)
        {
            object service;
            if (_services.TryGetValue(type, out service))
                return service;

            if (loadedTypes.Contains(type))
                throw new BuildCsException("Cannot resolve service '{0}' because it has a circular dependency on '{1}'.".F(loadedTypes[0], type));

            var ctor = type.UnderlyingSystemType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .OrderByDescending(x => x.GetParameters().Length)
                .First();

            loadedTypes.Add(type);
            var parameters = ctor.GetParameters();
            var args = new List<object>();
            foreach(var parameter in parameters)
                args.Add(LoadService(loadedTypes, parameter.ParameterType));

            service = ctor.Invoke(args.ToArray());
            _services[type] = service;
            return service;
        }
    }
}