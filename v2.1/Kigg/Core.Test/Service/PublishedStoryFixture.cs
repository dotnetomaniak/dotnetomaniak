using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Service;

    public class PublishedStoryFixture
    {
        private readonly PublishedStory _publishedStory;

        public PublishedStoryFixture()
        {
            _publishedStory = new PublishedStory(new Mock<IStory>().Object);
        }

        [Fact]
        public void TotalScore_Should_Be_Sum_Of_Other_Properties()
        {
            _publishedStory.Weights.Add("View", 10);
            _publishedStory.Weights.Add("Vote", 300);
            _publishedStory.Weights.Add("Comment", 130);
            _publishedStory.Weights.Add("User-Score", 450);
            _publishedStory.Weights.Add("Known-Source", 5);
            _publishedStory.Weights.Add("Freshness", 50);

            Assert.Equal(945, _publishedStory.TotalScore);
        }
    }
}