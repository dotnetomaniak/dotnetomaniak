using Xunit;

namespace Kigg.Web.Test
{
    public class StoryListViewDataFixture
    {
        private readonly StoryListViewData _viewData;

        public StoryListViewDataFixture()
        {
            _viewData = new StoryListViewData();
        }

        [Fact]
        public void Stories_Should_Not_Be_Null()
        {
            Assert.NotNull(_viewData.Stories);
        }

        [Fact]
        public void PageCount_Should_Return_Ten_When_StoryPerPage_Is_Ten_And_TotalStoryCount_Is_Hundred()
        {
            _viewData.StoryPerPage = 10;
            _viewData.TotalStoryCount = 100;

            Assert.Equal(10, _viewData.PageCount);
        }
    }
}