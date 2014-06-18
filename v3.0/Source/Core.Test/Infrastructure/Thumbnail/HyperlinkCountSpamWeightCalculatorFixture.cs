using Kigg.Infrastructure;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class HyperlinkCountSpamWeightCalculatorFixture
    {
        private const int MatchValue = 1;

        private readonly HyperlinkCountSpamWeightCalculator _calculator;

        public HyperlinkCountSpamWeightCalculatorFixture()
        {
            _calculator = new HyperlinkCountSpamWeightCalculator(MatchValue);
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Value_When_Content_Contains_Hyperlink()
        {
            var value = _calculator.Calculate("This is a content which contains hyper link. 1. <a hre=\"http://dummy.com\"><dummy1</a> 2. <a href=\"https://dummy2.com\">dummy2</a> 3. <a href=\"mailto:dummy@user.com\">dummy 3</a>");

            Assert.Equal(MatchValue * 3, value);
        }

        [Fact]
        public void Calculate_Should_Return_Zero_When_Content_Does_Not_Contains_Any_Hyperlink()
        {
            var value = _calculator.Calculate("Content with no hyperlink");

            Assert.Equal(0, value);
        }
    }
}