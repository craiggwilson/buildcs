using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs.Services.Targetting
{
    public class BuildCsSkipTargetException : Exception
    {
        public BuildCsSkipTargetException() 
            : base("(No reason provided)")
        { }
        
        public BuildCsSkipTargetException(string message) 
            : base(message) 
        { }
    }
}
