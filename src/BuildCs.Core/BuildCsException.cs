using System;

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
