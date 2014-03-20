using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var commandLine = new BuildCommandLine(session.ScriptArgs);
            var targetManager = new BuildTargetManager();
            var targetRunner = new BuildTargetRunner(targetManager);

            _build = new Build(commandLine, targetManager, targetRunner);

            session.ImportNamespace("BuildCs");
        }

        public void Terminate()
        {
            
        }
    }
}