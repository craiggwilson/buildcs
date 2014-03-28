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
        public static GitHelper GitHelper(this IBuildSession session)
        {
            return session.GetService<GitHelper>();
        }

        public static IEnumerable<string> GitExec(this IBuildSession session, BuildItem repositoryDir, string args, TimeSpan? timeout = null)
        {
            return GitHelper(session).Exec(repositoryDir, args, timeout);
        }
    }
}