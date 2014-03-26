using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Git
{
    public static class Extensions
    {
        public static GitHelper GitHelper(this IBuild build)
        {
            return build.GetService<GitHelper>();
        }

        public static IEnumerable<string> GitExec(this IBuild build, BuildItem repositoryDir, string args, TimeSpan? timeout = null)
        {
            return GitHelper(build).Exec(repositoryDir, args, timeout);
        }
    }
}