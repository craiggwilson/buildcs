using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptCs.Contracts;

namespace BuildCs
{
    public class Build : IScriptPackContext
    {
        private readonly BuildCommandLine _commandLine;
        private readonly BuildTargetManager _targetManager;
        private readonly BuildTargetRunner _targetRunner;
        private IBuildTracer _tracer;

        public Build(BuildCommandLine commandLine, BuildTargetManager targetManager, BuildTargetRunner targetRunner)
        {
            _commandLine = commandLine;
            _targetManager = targetManager;
            _targetRunner = targetRunner;
            _tracer = new TextWriterBuildTracer(Console.Out);
        }

        public IBuildTracer Tracer
        {
            get { return _tracer; }
        }

        public BuildCommandLine CommandLine
        {
            get { return _commandLine; }
        }

        public BuildTargetRunner Runner
        {
            get { return _targetRunner; }
        }

        public BuildTargetManager Targets
        {
            get { return _targetManager; }
        }
    }
}