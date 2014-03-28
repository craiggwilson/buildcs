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
            session.ImportNamespace("ScriptCs.BuildCs");
            session.ImportNamespace("BuildCs");
            session.ImportNamespace("BuildCs.AssemblyInfo");
            session.ImportNamespace("BuildCs.FileSystem");
            session.ImportNamespace("BuildCs.Git");
            session.ImportNamespace("BuildCs.MsBuild");
            session.ImportNamespace("BuildCs.Nuget");
            session.ImportNamespace("BuildCs.NUnit");
            session.ImportNamespace("BuildCs.Processes");
            session.ImportNamespace("BuildCs.Targetting");
            session.ImportNamespace("BuildCs.Tracing");
            session.ImportNamespace("BuildCs.XUnit");
            session.ImportNamespace("BuildCs.Zip");

            var arguments = new Arguments(session.ScriptArgs);
            if (arguments.Listeners.Count == 0)
                arguments.AddListener(typeof(ConsoleListener).FullName);

            _build = new Build(new BuildSessionFactory().Create(arguments));
        }

        public void Terminate()
        {
            
        }
    }
}