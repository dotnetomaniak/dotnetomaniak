using Xunit;

namespace Kigg.Web.Test
{
    public class UserListViewDataFixture
    {
        private readonly UserListViewData _viewData;

        public UserListViewDataFixture()
        {
            _viewData = new UserListViewData();
        }

        [Fact]
        public void Users_Should_Not_Be_Null()
        {
            Assert.NotNull(_viewData.Users);
        }

        [Fact]
        public void PageCount_Should_Return_Ten_When_UserPerPage_Is_Ten_And_TotalUserCount_Is_Hundred()
        {
            _viewData.UserPerPage = 10;
            _viewData.TotalUserCount = 100;

            Assert.Equal(10, _viewData.PageCount);
        }
    }
}