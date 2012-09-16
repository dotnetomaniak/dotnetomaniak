using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;

    public class BaseViewDataFixture
    {
        private readonly BaseViewData _viewData;

        public BaseViewDataFixture()
        {
            _viewData = new BaseViewDataTestDouble();
        }

        [Fact]
        public void CanCurrentUserModerate_Should_Return_True_When_User_Is_Aauthenticated_And_Role_Is_Moderator_Or_Above()
        {
            var user = new Mock<IUser>();
            user.ExpectGet(u => u.Role).Returns(Roles.Moderator);

            _viewData.IsCurrentUserAuthenticated = true;
            _viewData.CurrentUser = user.Object;

            Assert.True(_viewData.CanCurrentUserModerate);
        }
    }

    public class BaseViewDataTestDouble: BaseViewData
    {
    }
}