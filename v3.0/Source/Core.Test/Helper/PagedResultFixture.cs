using Xunit;

namespace Kigg.Core.Test
{
    public class PagedResultFixture
    {
        private const int Total = 100;

        private readonly string[] Languages = new[] {"C#", "VB.NET", "Ruby", "Java", "C++", "Python", "Perl", "PHP"};

        private readonly PagedResult<string> _pagedResult;

        public PagedResultFixture()
        {
            _pagedResult = new PagedResult<string>(Languages, Total);
        }

        [Fact]
        public void Result_Should_Contain_All_The_Items_Which_Is_Passed_In_Constructor()
        {
            foreach(var language in Languages)
            {
                Assert.Contains(language, Languages);
            }
        }

        [Fact]
        public void Total_Should_Be_The_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(Total, _pagedResult.Total);
        }

        [Fact]
        public void Result_Should_Be_Empty_When_Nothing_Is_Passed_In_Constructor()
        {
            var pagedResult = new PagedResult<int>();

            Assert.Empty(pagedResult.Result);
        }

        [Fact]
        public void Total_Should_Be_Zero_When_Nothing_Is_Passed_In_Constructor()
        {
            var pagedResult = new PagedResult<int>();

            Assert.Equal(0, pagedResult.Total);
        }

        [Fact]
        public void IsEmpty_Should_Be_True_When_Result_Does_Not_Contain_Any_Item()
        {
            var pagedResult = new PagedResult<string>();

            Assert.True(pagedResult.IsEmpty);
        }
    }
}