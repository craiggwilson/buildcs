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

            _build = new Build(session.ScriptArgs);
        }

        public void Terminate()
        {
            
        }
    }
}