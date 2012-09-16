using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class StoryContentFixture
    {
        private const string Title = "This a Story";
        private const string Description = "This a Story description";
        private const string TrackBackUrl = "http://aStoryTrackback.com";

        private readonly StoryContent storyContent;

        public StoryContentFixture()
        {
            storyContent = new StoryContent(Title, Description, TrackBackUrl);
        }

        [Fact]
        public void Title_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(Title, storyContent.Title);
        }

        [Fact]
        public void Description_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(Description, storyContent.Description);
        }

        [Fact]
        public void TrackBackUrl_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Equal(TrackBackUrl, storyContent.TrackBackUrl);
        }

        [Fact]
        public void Empty_Should_Return_All_Null_Properties()
        {
            Assert.Null(StoryContent.Empty.Title);
            Assert.Null(StoryContent.Empty.Description);
            Assert.Null(StoryContent.Empty.TrackBackUrl);
        }
    }
}