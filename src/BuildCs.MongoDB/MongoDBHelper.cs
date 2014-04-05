using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BuildCs.Environment;
using BuildCs.FileSystem;
using BuildCs.Processes;
using BuildCs.Tracing;
using MongoDB.Driver;

namespace BuildCs.MongoDB
{
    public class MongoDBHelper
    {
        private readonly EnvironmentHelper _environment;
        private readonly FileSystemHelper _fileSystem;
        private readonly ProcessHelper _process;
        private readonly Tracer _tracer;

        public MongoDBHelper(EnvironmentHelper environment, FileSystemHelper fileSystem, ProcessHelper process, Tracer tracer)
        {
            _environment = environment;
            _fileSystem = fileSystem;
            _process = process;
            _tracer = tracer;
        }

        public IDisposable LaunchStandalone(Action<StandAloneArgs> config)
        {
            var args = new StandAloneArgs();
            args.DataDir = "./data";
            args.LogPath = "./data/log.txt";
            args.Port = 27017;
            args.SmallFiles = true;
            args.OplogSize = 100;
            config(args);

            return _process.Launch(la =>
            {
                la.TraceOutput = false;
                la.StartInfo.FileName = GetExecutable(args.BinDir, "mongod");
                la.StartInfo.Arguments = GetMongodArguments(args);
                la.KillAction = () => ShutdownMongod("mongodb://localhost:{0}".F(args.Port));
            });
        }

        private List<string> GetCommonArgs(MongoDBArgsBase args)
        {
            var list = new List<string>();

            list.Add("--port " + args.Port);

            if(args.LogPath != null)
            {
                var dir = _fileSystem.GetDirectoryOfFile(args.LogPath);
                _fileSystem.CreateDirectory(dir);
                list.Add("--logpath {0}".F(args.LogPath));
            }

            return list;
        }

        private string GetMongodArguments(StandAloneArgs args)
        {
            var list = GetCommonArgs(args);

            if(args.DataDir != null)
            {
                _fileSystem.CreateDirectory(args.DataDir);
                list.Add("--dbpath {0}".F(args.DataDir));
            }

            if (args.OplogSize.HasValue)
                list.Add("--oplogSize {0}".F(args.OplogSize));

            if(args.SmallFiles)
                list.Add("--smallfiles");

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

        private void ShutdownMongod(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            try
            {
                server.Shutdown();
            }
            catch { }
        }
    }
}