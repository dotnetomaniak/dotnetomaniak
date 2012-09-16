using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;
    using Service;

    public class KnownSourceWeightFixture
    {
        private readonly Mock<IKnownSourceRepository> _repository;
        private readonly KnownSourceWeight _strategy;

        public KnownSourceWeightFixture()
        {
            _repository = new Mock<IKnownSourceRepository>();
            _strategy = new KnownSourceWeight(_repository.Object);
        }

        [Fact]
        public void Calculate_Should_Return_Known_Source_Grade_As_Weight_When_Url_Matched()
        {
            var story = new Mock<IStory>();
            story.SetupGet(s => s.Url).Returns("http://dummystory.com");

            var knownSource = new Mock<IKnownSource>();
            knownSource.SetupGet(ks => ks.Grade).Returns(KnownSourceGrade.A);

            _repository.Setup(r => r.FindMatching(It.IsAny<string>())).Returns(knownSource.Object);

            var weight = _strategy.Calculate(SystemTime.Now(), story.Object);

            Assert.Equal((int) KnownSourceGrade.A, weight);
        }

        [Fact]
        public void Calculate_Should_Return_Zero_When_Url_Do_Not_Match()
        {
            var story = new Mock<IStory>();
            story.SetupGet(s => s.Url).Returns("http://dummystory.com");

            _repository.Setup(r => r.FindMatching(It.IsAny<string>())).Returns((IKnownSource) null);

            var weight = _strategy.Calculate(SystemTime.Now(), story.Object);

            Assert.Equal(0, weight);
        }
    }
}