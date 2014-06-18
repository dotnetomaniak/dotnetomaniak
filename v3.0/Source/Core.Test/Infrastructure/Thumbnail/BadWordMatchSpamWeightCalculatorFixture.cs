using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    public class BadWordMatchSpamWeightCalculatorFixture
    {
        private readonly Mock<IFile> _file;
        private readonly BadWordMatchSpamWeightCalculator _calculator;

        public BadWordMatchSpamWeightCalculatorFixture()
        {
            _file = new Mock<IFile>();

            _file.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns("<?xml version=\"1.0\" encoding=\"utf-8\" ?><badWords><item><exp>viagra</exp><value>100</value></item><item><exp>blackjack</exp><value>100</value></item><item><exp>casino</exp><value>100</value></item></badWords>");

            _calculator = new BadWordMatchSpamWeightCalculator(_file.Object, "BadWords");
        }

        [Fact]
        public void Should_Use_File_To_Buld_Bad_Words()
        {
            _file.Verify();
        }

        [Fact]
        public void Calculate_Should_Return_Correct_Value_When_Content_Contains_Bad_Words()
        {
            var value = _calculator.Calculate("Buy viagra and play online casino");

            Assert.Equal(200, value);
        }

        [Fact]
        public void Calculate_Should_Return_Zero_When_Content_Does_Not_Contain_Any_Bad_Word()
        {
            var value = _calculator.Calculate("This is a legetimate text");

            Assert.Equal(0, value);
        }
    }
}