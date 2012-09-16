using Xunit;

namespace Kigg.Web.Test
{
    public class FeedViewDataFixture
    {
        private readonly FeedViewData _viewData;

        public FeedViewDataFixture()
        {
            _viewData = new FeedViewData();
        }

        [Fact]
        public void Stories_Should_Not_Be_Null()
        {
            Assert.NotNull(_viewData.Stories);
        }
    }
}