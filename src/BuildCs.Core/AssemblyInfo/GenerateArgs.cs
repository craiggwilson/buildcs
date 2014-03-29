using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildCs.AssemblyInfo
{
    public class GenerateArgs
    {
        public GenerateArgs()
        {
            Attributes = new AttributesBuilder();
        }

        public AttributesBuilder Attributes { get; private set; }
    }
}