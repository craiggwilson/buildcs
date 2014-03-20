using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public static class StringExtensions
    {
        public static string F(this string target, params object[] args)
        {
            return string.Format(target, args);
        }
    }
}