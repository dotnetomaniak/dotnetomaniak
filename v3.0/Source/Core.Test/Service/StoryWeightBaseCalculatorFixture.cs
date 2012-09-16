using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Service;

    public class StoryWeightBaseCalculatorFixture
    {
        private const string Name = "Dummy";

        private readonly Mock<StoryWeightBaseCalculator> _strategy;

        public StoryWeightBaseCalculatorFixture()
        {
            _strategy = new Mock<StoryWeightBaseCalculator>(Name);
        }

        [Fact]
        public void Name_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(Name, _strategy.Object.Name);
        }
    }
}