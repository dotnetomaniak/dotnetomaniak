using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Kigg.Test.Infrastructure;

    public class UserExtensionFixture : BaseFixture
    {
        private readonly Mock<IUser> _user;

        public UserExtensionFixture()
        {
            _user = new Mock<IUser>();
        }

        [Fact]
        public void HasDefaultOpenIDEmail_Should_Be_True_When_User_Email_Is_Same_As_The_Settings_DefaultOpenIDEmail()
        {
            _user.ExpectGet(u => u.Email).Returns(settings.Object.DefaultEmailOfOpenIdUser);

            Assert.True(_user.Object.HasDefaultOpenIDEmail());
        }

        [Fact]
        public void IsOpenIDAccount_Should_Be_True_When_Password_Is_Null()
        {
            _user.ExpectGet(u => u.Password).Returns((string) null);

            Assert.True(_user.Object.IsOpenIDAccount());
        }

        [Fact]
        public void IsAdministrator_Should_Be_True_When_Role_Contains_Administrator()
        {
            _user.ExpectGet(u => u.Role).Returns(Roles.Administrator);

            Assert.True(_user.Object.IsAdministrator());
        }

        [Fact]
        public void IsModerator_Should_Be_True_When_Role_Contains_Modarator()
        {
            _user.ExpectGet(u => u.Role).Returns(Roles.Moderator);

            Assert.True(_user.Object.IsModerator());
        }

        [Fact]
        public void IsBot_Should_Be_True_When_Role_Contains_Bot()
        {
            _user.ExpectGet(u => u.Role).Returns(Roles.Bot);

            Assert.True(_user.Object.IsBot());
        }

        [Fact]
        public void IsPublicUser_Should_Be_True_When_Role_Does_Not_Contain_Administrator_Modarator_Or_Bot()
        {
            Assert.True(_user.Object.IsPublicUser());
        }

        [Fact]
        public void CanModerate_Should_Be_True_When_User_Is_Either_Administrator_Or_Moderator()
        {
            _user.ExpectGet(u => u.Role).Returns(Roles.Administrator | Roles.Moderator);

            Assert.True(_user.Object.CanModerate());
        }

        [Fact]
        public void ShouldHideCaptcha_Should_Be_False_When_Use_Is_Null()
        {
            Assert.False(((IUser) null).ShouldHideCaptcha());
        }

        [Fact]
        public void ShouldHideCaptcha_Should_Be_True_When_Use_Is_Not_Public_User()
        {
            _user.ExpectGet(u => u.Role).Returns(Roles.Bot);

            Assert.True(_user.Object.ShouldHideCaptcha());
        }

        [Fact]
        public void ShouldHideCaptcha_Should_Be_True_When_User_Score_Is_More_Than_Settings_MaximumUserScoreToShowCaptcha()
        {
            _user.ExpectGet(u => u.CurrentScore).Returns(settings.Object.MaximumUserScoreToShowCaptcha + 1);

            Assert.True(_user.Object.ShouldHideCaptcha());
        }
    }
}