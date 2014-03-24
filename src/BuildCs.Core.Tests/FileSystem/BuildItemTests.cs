using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Extensions;

namespace BuildCs.FileSystem
{
    public class BuildItemTests
    {
        [Fact]
        public void Should_implicitly_convert_to_string()
        {
            var item = new BuildItem(".\\test");

            string path = item;

            path.ShouldBe(".\\test");
        }

        [Fact]
        public void Should_implicitly_convert_from_string()
        {
            var path = ".\\test";

            BuildItem item = path;

            item.ToString().ShouldBe(".\\test");
        }

        [Theory]
        [InlineData(".\\test", "funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test\\", "funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test", "\\funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test\\", "\\funny\\x.txt", ".\\test\\funny\\x.txt")]
        public void Should_combine_with_another_build_item(string first, string second, string combined)
        {
            var result = new BuildItem(first) + new BuildItem(second);

            result.ToString().ShouldBe(combined);
        }

        [Theory]
        [InlineData(".\\test", "funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test\\", "funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test", "\\funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test\\", "\\funny\\x.txt", ".\\test\\funny\\x.txt")]
        public void Should_combine_with_a_string(string first, string second, string combined)
        {
            var result = new BuildItem(first) + second;

            result.ToString().ShouldBe(combined);
        }

        [Theory]
        [InlineData(".\\test", "funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test\\", "funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test", "\\funny\\x.txt", ".\\test\\funny\\x.txt")]
        [InlineData(".\\test\\", "\\funny\\x.txt", ".\\test\\funny\\x.txt")]
        public void Should_combine_a_string_with_a_build_item(string first, string second, string combined)
        {
            var result = first + new BuildItem(second);

            result.ToString().ShouldBe(combined);
        }

        [Fact]
        public void Should_test_as_equal_when_left_and_right_side_are_null()
        {
            BuildItem item1 = null;
            (item1 == null).ShouldBe(true);
            (item1 != null).ShouldBe(false);
        }

        [Fact]
        public void Should_test_as_not_equal_when_right_side_is_null()
        {
            var item1 = new BuildItem("\\test");

            item1.Equals(null).ShouldBe(false);
            (item1 == null).ShouldBe(false);
            (item1 != null).ShouldBe(true);
        }

        [Fact]
        public void Should_test_as_equal_when_items_are_equal()
        {
            var item1 = new BuildItem("\\test");
            var item2 = new BuildItem("\\test");

            item1.Equals(item2).ShouldBe(true);
            (item1 == item2).ShouldBe(true);
        }

        [Fact]
        public void Should_test_as_not_equal_when_items_are_not_equal()
        {
            var item1 = new BuildItem("\\test");
            var item2 = new BuildItem("\\test2");

            item1.Equals(item2).ShouldBe(false);
            (item1 != item2).ShouldBe(true);
        }
    }
}