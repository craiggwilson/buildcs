using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BuildCs.Sample.Tests
{
    [TestFixture]
    public class SampleNUnitTests
    {
        [Test]
        public void ToString_should_return_Hello()
        {
            Assert.AreEqual("Hello", new Sample().ToString());
        }
    }
}
