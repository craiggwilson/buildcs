using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.Targetting;

namespace BuildCs.MongoDB
{
    public static class Extensions
    {
        public static MongoDBHelper MongoDBHelper(this IBuildSession session)
        {
            return session.GetService<MongoDBHelper>();
        }

        public static IDisposable LaunchStandAloneMongoDB(this IBuildSession session, Action<StandAloneArgs> config)
        {
            return MongoDBHelper(session).LaunchStandalone(config);
        }

        public static ITargetBuilder RequiresStandaloneMongoDB(this ITargetBuilder target, Action<StandAloneArgs> config)
        {
            return target.Wrap((ex, next) =>
            {
                Console.WriteLine("Here");
                using(ex.Build.Session.LaunchStandAloneMongoDB(config))
                {
                    next(ex);
                }
                Console.WriteLine("There");
            });
        }
    }
}