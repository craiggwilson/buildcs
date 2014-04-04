using BuildCs;
using BuildCs.Tracing;
using ScriptCs.Contracts;

namespace ScriptCs.BuildCs
{
    public class BuildScriptPack : IScriptPack
    {
        private Build _build;

        public IScriptPackContext GetContext()
        {
            return _build;
        }

        public void Initialize(IScriptPackSession session)
        {
            session.ImportNamespace("System.Xml.Linq");
            session.ImportNamespace("System.Xml.XPath");
            session.ImportNamespace("ScriptCs.BuildCs");
            session.ImportNamespace("BuildCs");
            session.ImportNamespace("BuildCs.AssemblyInfo");
            session.ImportNamespace("BuildCs.Environment");
            session.ImportNamespace("BuildCs.FileSystem");
            session.ImportNamespace("BuildCs.Git");
            session.ImportNamespace("BuildCs.MsBuild");
            session.ImportNamespace("BuildCs.Nuget");
            session.ImportNamespace("BuildCs.NUnit");
            session.ImportNamespace("BuildCs.Processes");
            session.ImportNamespace("BuildCs.SemVer");
            session.ImportNamespace("BuildCs.Targetting");
            session.ImportNamespace("BuildCs.Tracing");
            session.ImportNamespace("BuildCs.Xml");
            session.ImportNamespace("BuildCs.XUnit");
            session.ImportNamespace("BuildCs.Zip");

            var arguments = new Arguments(session.ScriptArgs);
            _build = new Build(new BuildSessionFactory().Create(arguments));
            var tracer = _build.GetService<Tracer>();
            if (tracer.Listeners.Count == 0)
                tracer.Listeners.Add(new ConsoleListener(_build));
        }

        public void Terminate()
        {
            _build.Dispose();
        }
    }
}