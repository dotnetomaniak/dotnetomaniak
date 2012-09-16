using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;

    public class UserExtensionFixture
    {
        [Fact]
        public void GravatarUrl_Should_Return_Correct_Avatar_Url()
        {
            Mock<IUser> user = new Mock<IUser>();

            user.ExpectGet(u => u.Email).Returns("dummy@user.com");

            string url = user.Object.GravatarUrl(50);

            Assert.Equal("http://www.gravatar.com/avatar/19bc7253102000b3a64c290f74f3ae83?r=G&s=50&d=wavatar", url);
        }
    }
}