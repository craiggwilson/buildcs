using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BuildCs;
using BuildCs.Tracing;
using ScriptCs.Contracts;

namespace ScriptCs.BuildCs
{
    public class Build : IBuildSession, IScriptPackContext
    {
        private readonly IBuildSession _inner;

        public Build(IBuildSession inner)
        {
            _inner = inner;
        }

        public IDictionary<string, string> Parameters
        {
            get { return _inner.Parameters; }
        }

        public MessageLevel Verbosity
        {
            get { return _inner.Verbosity; }
            set { _inner.Verbosity = value; }
        }

        public T GetService<T>()
        {
            return _inner.GetService<T>();
        }
    }
}