using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Service;

    public class FreshnessWeightFixture
    {
        private const float FreshNessThresholdInDays = 24f;
        private readonly FreshnessWeight _strategy;

        public FreshnessWeightFixture()
        {
            _strategy = new FreshnessWeight(FreshNessThresholdInDays, 4);
        }

        [Fact]
        public void Calculate_Should_Return_Same_Weight_When_Both_Stories_Age_Is_More_Than_Specified_Days()
        {
            var story1 = new Mock<IStory>();
            var story2 = new Mock<IStory>();
            var now = SystemTime.Now();

            story1.SetupGet(s => s.CreatedAt).Returns(now.AddDays(-FreshNessThresholdInDays).AddHours(-6));
            story2.SetupGet(s => s.CreatedAt).Returns(now.AddDays(-FreshNessThresholdInDays).AddHours(-12));

            Assert.Equal(_strategy.Calculate(now, story1.Object), _strategy.Calculate(now, story2.Object));
        }

        [Fact]
        public void Calculate_Should_Return_Different_Weight_When_Either_Of_Stories_Age_Is_Less_Than_Specified_Days()
        {
            var story1 = new Mock<IStory>();
            var story2 = new Mock<IStory>();
            var now = SystemTime.Now();

            story1.SetupGet(s => s.CreatedAt).Returns(now.AddDays(-FreshNessThresholdInDays).AddHours(-6));
            story2.SetupGet(s => s.CreatedAt).Returns(now.AddDays(-FreshNessThresholdInDays).AddHours(12));

            Assert.NotEqual(_strategy.Calculate(now, story1.Object), _strategy.Calculate(now, story2.Object));
        }
    }
}