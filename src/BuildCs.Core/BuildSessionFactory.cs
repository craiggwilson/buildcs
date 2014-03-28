using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Targetting;
using BuildCs.Tracing;

namespace BuildCs
{
    public class BuildSessionFactory
    {
        public IBuildSession Create(Arguments arguments)
        {
            return new BuildSession(arguments);
        }

        private class BuildSession : IBuildSession
        {
            private readonly Dictionary<Type, object> _services;

            public BuildSession(Arguments arguments)
            {
                Parameters = arguments.Parameters.ToDictionary(x => x.Key, x => x.Value);
                Verbosity = arguments.Verbosity;

                _services = new Dictionary<Type, object>();
                _services.Add(typeof(IBuildSession), this);

                var tracer = new Tracer(this);
                _services.Add(typeof(Tracer), tracer);
                foreach (var listener in arguments.Listeners)
                {
                    var listenerType = Type.GetType(listener, false, true);
                    if (listenerType == null)
                    {
                        listenerType = Type.GetType("BuildCs.Tracing." + listener, false, true);
                        if (listenerType == null)
                            throw new BuildCsException("Listener of type '{0}' could not be found.".F(listenerType));
                    }

                    tracer.Listeners.Add((IBuildListener)Activator.CreateInstance(listenerType, new object[] { this }));
                }

                var targetManager = new TargetManager();
                _services.Add(typeof(TargetManager), targetManager);
                var targetRunner = new TargetRunner(targetManager, tracer);
                _services.Add(typeof(TargetRunner), targetRunner);
            }

            public IDictionary<string, string> Parameters { get; private set; }

            public MessageLevel Verbosity { get; set; }

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
                foreach (var parameter in parameters)
                    args.Add(LoadService(loadedTypes, parameter.ParameterType));

                service = ctor.Invoke(args.ToArray());
                _services[type] = service;
                return service;
            }
        }
    }
}