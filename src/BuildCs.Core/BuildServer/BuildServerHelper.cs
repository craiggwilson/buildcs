using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Tracing;

namespace BuildCs.BuildServer
{
    public class BuildServerHelper
    {
        private readonly EnvironmentHelper _environment;
        private readonly BuildTracer _tracer;

        public BuildServerHelper(EnvironmentHelper environment, BuildTracer tracer)
        {
            _environment = environment;
            _tracer = tracer;

            Initialize();
        }

        public string BuildNumber { get; set; }

        public BuildServerType Type { get; set; }

        public void ReportStartTarget(string name)
        {

        }

        public void ReportStopTarget(string name)
        {

        }

        private void Initialize()
        {
            BuildNumber = _environment.GetEnvironmentVariable("BUILD_NUMBER");
            if (BuildNumber != null)
            {
                if (_environment.GetEnvironmentVariable("jenkins_home") != null)
                    Type = BuildServerType.Jenkins;
                else
                    Type = BuildServerType.TeamCity;

                return;
            }

            BuildNumber = _environment.GetEnvironmentVariable("TRAVIS_BUILD_NUMBER");
            if(BuildNumber != null)
            {
                Type = BuildServerType.Travis;
                return;
            }

            BuildNumber = _environment.GetEnvironmentVariable("CCNETLABEL");
            if(BuildNumber != null)
            {
                Type = BuildServerType.CCNet;
                return;
            }

            BuildNumber = null;
            Type = BuildServerType.Unknown;
        }
    }
}