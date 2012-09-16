using Xunit;

namespace Kigg.Web.Test
{
    public class PageCalculatorFixture
    {
        [Fact]
        public void TotalPage_Should_Return_Ten_When_RowPerPage_Is_Ten_And_Total_Is_Hundred()
        {
            Assert.Equal(10, PageCalculator.TotalPage(100, 10));
        }

        [Fact]
        public void TotalPage_Should_Return_Five_When_RowPerPage_Is_Ten_And_Total_Is_FortyTwo()
        {
            Assert.Equal(5, PageCalculator.TotalPage(42, 10));
        }

        [Fact]
        public void TotalPage_Should_Return_One_When_RowPerPage_Is_Ten_And_Total_Is_Seven()
        {
            Assert.Equal(1, PageCalculator.TotalPage(7, 10));
        }

        [Fact]
        public void TotalPage_Should_Return_One_When_RowPerPage_Is_Zero_And_Total_Is_Zero()
        {
            Assert.Equal(1, PageCalculator.TotalPage(0, 0));
        }

        [Fact]
        public void StartIndex_Should_Return_Zero_When_Page_Is_One_And_RowPerPage_Is_Ten()
        {
            Assert.Equal(0, PageCalculator.StartIndex(1, 10));
        }

        [Fact]
        public void StartIndex_Should_Return_Ten_When_Page_Is_Two_And_RowPerPage_Is_Ten()
        {
            Assert.Equal(10, PageCalculator.StartIndex(2, 10));
        }

        [Fact]
        public void StartIndex_Should_Return_Zero_When_Page_Is_Zero_And_RowPerPage_Is_Ten()
        {
            Assert.Equal(0, PageCalculator.StartIndex(0, 10));
        }
    }
}