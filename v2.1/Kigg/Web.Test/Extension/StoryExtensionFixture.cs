using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;

    public class StoryExtensionFixture
    {
        private readonly Mock<IStory> _story;

        public StoryExtensionFixture()
        {
            _story = new Mock<IStory>();
        }

        [Fact]
        public void StripDescription_Should_Return_Trimmed_Description()
        {
            _story.ExpectGet(s => s.TextDescription).Returns(new string('x', 515));

            var description = _story.Object.StrippedDescription();

            Assert.Equal(string.Concat(new string('x', 512), "..."), description);
        }
    }
}