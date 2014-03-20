using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCs
{
    public class BuildCsException : Exception
    {
        public BuildCsException(string message) 
            : base(message) 
        { }
        
        public BuildCsException(string message, Exception inner) 
            : base(message, inner) 
        { }
    }
}
