using System;
using Kigg.DomainObjects;
using Kigg.Infrastructure.DomainRepositoryExtensions;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Repository;
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
            _user.SetupGet(u => u.Email).Returns(settings.Object.DefaultEmailOfOpenIdUser);

            Assert.True(_user.Object.HasDefaultOpenIDEmail());
        }

        [Fact]
        public void IsOpenIDAccount_Should_Be_True_When_Password_Is_Null()
        {
            _user.SetupGet(u => u.Password).Returns((string) null);

            Assert.True(_user.Object.IsOpenIDAccount());
        }

        [Fact]
        public void IsAdministrator_Should_Be_True_When_Role_Contains_Administrator()
        {
            _user.SetupGet(u => u.Role).Returns(Roles.Administrator);

            Assert.True(_user.Object.IsAdministrator());
        }

        [Fact]
        public void IsModerator_Should_Be_True_When_Role_Contains_Modarator()
        {
            _user.SetupGet(u => u.Role).Returns(Roles.Moderator);

            Assert.True(_user.Object.IsModerator());
        }

        [Fact]
        public void IsBot_Should_Be_True_When_Role_Contains_Bot()
        {
            _user.SetupGet(u => u.Role).Returns(Roles.Bot);

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
            _user.SetupGet(u => u.Role).Returns(Roles.Administrator | Roles.Moderator);

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
            _user.SetupGet(u => u.Role).Returns(Roles.Bot);

            Assert.True(_user.Object.ShouldHideCaptcha());
        }

        [Fact]
        public void ShouldHideCaptcha_Should_Be_True_When_User_Score_Is_More_Than_Settings_MaximumUserScoreToShowCaptcha()
        {
            _user.SetupGet(u => u.CurrentScore).Returns(settings.Object.MaximumUserScoreToShowCaptcha + 1);

            Assert.True(_user.Object.ShouldHideCaptcha());
        }

        [Fact]
        public void IsUniqueEmail_Should_Return_False_If_Same_Email_Already_Exists()
        {
            const string Email = "foo@bar.com";
            
            _user.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            _user.SetupGet(u => u.Email).Returns(Email);

            var sameEmailUser = new Mock<IUser>();
            sameEmailUser.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            sameEmailUser.SetupGet(u => u.Email).Returns(Email);

            PrepareFindByEmail(sameEmailUser.Object);
            
            Assert.False(_user.Object.IsUniqueEmail(Email));
        }

        [Fact]
        public void IsUniqueEmail_Should_Return_True_If_Email_Is_Unique()
        {
            const string Email = "foo@bar.com";

            _user.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            _user.SetupGet(u => u.Email).Returns(Email);

            PrepareFindByEmail(null);

            Assert.True(_user.Object.IsUniqueEmail(Email));
        }

        [Fact]
        public void IsUniqueEmail_Should_Use_IUserRepository()
        {
            const string Email = "foo@bar.com";
            var userRepository = SetupResolve<IUserRepository>();
            _user.Object.IsUniqueEmail(Email);
            userRepository.Verify(r => r.FindByEmail(Email), Times.AtMostOnce());
        }

        [Fact]
        public void GetScore_Should_Use_IUserRepository()
        {
            var userRepository = SetupResolve<IUserRepository>();
            var id = Guid.NewGuid();
            var start = SystemTime.Now().AddHours(-6);
            var end = SystemTime.Now();
            _user.SetupGet(u => u.Id).Returns(id);
            _user.Object.GetScoreBetween(start, end);

            userRepository.Verify(r => r.FindScoreById(id, start, end), Times.AtMostOnce());
        }

        private void PrepareFindByEmail(IUser user)
        {
            var repository = SetupResolve<IUserRepository>();
            repository.Setup(r => r.FindByEmail(_user.Object.Email)).Returns(user).Verifiable();
        }
    }
}