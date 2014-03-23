using System.Linq;
using Shouldly;
using Xunit;

namespace BuildCs.Targetting
{
    public class BuildTargetManagerTests
    {
        private readonly BuildTargetManager _subject;

        public BuildTargetManagerTests()
        {
            _subject = new BuildTargetManager();
            _subject.AddTarget("7").DependsOn("11", "8");
            _subject.AddTarget("5").DependsOn("11");
            _subject.AddTarget("3").DependsOn("8", "10");
            _subject.AddTarget("11").DependsOn("2", "9", "10");
            _subject.AddTarget("8").DependsOn("9");
            _subject.AddTarget("2");
            _subject.AddTarget("9");
            _subject.AddTarget("10");
        }

        [Fact]
        public void Should_contain_added_targets()
        {
            _subject.HasTarget("11").ShouldBe(true);
            _subject.GetTarget("11").ShouldNotBe(null);
        }

        [Fact]
        public void Should_not_allow_duplicate_targets()
        {
            Should.Throw<BuildCsException>(() => _subject.AddTarget("11"));
        }

        [Fact]
        public void Should_resolve_build_chain_without_duplicates()
        {
            var chain = _subject.GetBuildChain(new[] { "7", "5" });

            // TODO: figure out how to assert correct behavior here...
            chain.Count().ShouldBe(7);
        }

        [Fact]
        public void Should_not_resolve_a_chain_with_cycles()
        {
            _subject.GetTarget("9").DependsOn("11");

            Should.Throw<BuildCsException>(() => _subject.GetBuildChain(new[] { "7" }));
        }
    }
}