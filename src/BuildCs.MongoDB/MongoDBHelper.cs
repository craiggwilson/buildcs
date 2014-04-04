using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BuildCs.Environment;
using BuildCs.FileSystem;
using BuildCs.Processes;
using BuildCs.Tracing;

namespace BuildCs.MongoDB
{
    public class MongoDBHelper
    {
        private readonly EnvironmentHelper _environment;
        private readonly ProcessHelper _process;
        private readonly Tracer _tracer;

        public MongoDBHelper(EnvironmentHelper environment, ProcessHelper process, Tracer tracer)
        {
            _environment = environment;
            _process = process;
            _tracer = tracer;
        }

        public IDisposable LaunchStandalone(Action<StandAloneArgs> config)
        {
            var args = new StandAloneArgs();
            config(args);

            return _process.Launch(la =>
            {
                la.TraceOutput = false;
                la.StartInfo.FileName = GetExecutable(args.BinDir, "mongod");
                la.StartInfo.Arguments = GetMongodArguments(args);
            });
        }

        private List<string> GetCommonArgs(MongoDBArgsBase args)
        {
            var list = new List<string>();

            if (args.Port.HasValue)
                list.Add("-port " + args.Port.Value);

            return list;
        }

        private string GetMongodArguments(StandAloneArgs args)
        {
            var list = GetCommonArgs(args);

            return string.Join(" ", list);
        }

        private string GetExecutable(BuildItem binDir, string toolName)
        {
            if (!_environment.IsUnix)
                toolName += ".exe";

            if (binDir != null)
                return binDir + toolName;

            var ev = _environment.GetVariableOrDefault("MONGODB", null);
            if (!string.IsNullOrEmpty(ev))
                return new BuildItem(ev) + toolName;

            // hopefully it's in the path...
            return toolName;
        }
    }
}