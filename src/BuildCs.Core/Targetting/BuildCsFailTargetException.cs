using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Targetting
{
    public class BuildCsFailTargetException : Exception
    {
        public BuildCsFailTargetException() 
            : base("(No reason provided)")
        { }

        public BuildCsFailTargetException(string message) 
            : base(message ?? "")
        { }
    }
}