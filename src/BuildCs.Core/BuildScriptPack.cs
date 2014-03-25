using ScriptCs.Contracts;

namespace BuildCs
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
            session.ImportNamespace("BuildCs");
            session.ImportNamespace("BuildCs.FileSystem");
            session.ImportNamespace("BuildCs.MsBuild");
            session.ImportNamespace("BuildCs.Nuget");
            session.ImportNamespace("BuildCs.NUnit");
            session.ImportNamespace("BuildCs.Processes");
            session.ImportNamespace("BuildCs.Targetting");
            session.ImportNamespace("BuildCs.Tracing");
            session.ImportNamespace("BuildCs.XUnit");
            session.ImportNamespace("BuildCs.Zip");

            _build = new Build(session.ScriptArgs);
        }

        public void Terminate()
        {
            
        }
    }
}