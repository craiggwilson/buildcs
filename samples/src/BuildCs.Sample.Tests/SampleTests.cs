using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BuildCs.Sample.Tests
{
    public class SampleTests
    {
        [Fact]
        public void ToString_should_return_Hello()
        {
            Assert.Equal("Hello", new Sample().ToString());
        }
    }
}