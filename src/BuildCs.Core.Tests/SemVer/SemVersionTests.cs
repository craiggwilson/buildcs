using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Extensions;

namespace BuildCs.SemVer
{
    public class SemVersionTests
    {
        [Theory]
        [InlineData("1.0.0", 1, 0, 0, null, null)]
        [InlineData("1.2.0", 1, 2, 0, null, null)]
        [InlineData("1.0.3", 1, 0, 3, null, null)]
        [InlineData("1.0.3-rc", 1, 0, 3, "rc", null)]
        [InlineData("1.0.3-rc1", 1, 0, 3, "rc1", null)]
        [InlineData("1.0.3-rc.2.3", 1, 0, 3, "rc.2.3", null)]
        [InlineData("1.0.3+ci.1230", 1, 0, 3, null, "ci.1230")]
        [InlineData("1.0.3-rc.2.3+ci.1230", 1, 0, 3, "rc.2.3", "ci.1230")]
        public void TestParsing(string versionString, int major, int minor, int patch, string preRelease, string buildMetadata)
        {
            var subject = new SemVersion(versionString);
            subject.Major.ShouldBe(major);
            subject.Minor.ShouldBe(minor);
            subject.Patch.ShouldBe(patch);
            subject.PreRelease.ShouldBe(preRelease);
            subject.BuildMetadata.ShouldBe(buildMetadata);

            subject.ToString().ShouldBe(versionString);
        }
    }
}