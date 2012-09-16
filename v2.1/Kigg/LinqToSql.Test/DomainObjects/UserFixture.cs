using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;

    public class UserFixture : LinqToSqlBaseFixture
    {
        private readonly User _user;

        public UserFixture()
        {
            _user = new User();
        }

        [Fact]
        public void CurrentScore_Should_Use_UserRepository()
        {
            userRepository.Expect(r => r.FindScoreById(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(0).Verifiable();

            Assert.Equal(0, _user.CurrentScore);

            userRepository.Verify();
        }

        [Fact]
        public void Tags_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            Assert.Empty(_user.Tags);
        }

        [Fact]
        public void TagCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            Assert.Equal(0, _user.TagCount);
        }

        [Fact]
        public void ChangeEmail_Should_Update_Email()
        {
            const string CurrentEmail = "foo@bar.com";

            _user.Email = CurrentEmail;

            _user.ChangeEmail("hello@world.com");

            Assert.NotEqual(_user.Email, CurrentEmail);
        }

        [Fact]
        public void ChangeEmail_Should_Update_LastActivityAt()
        {
            DateTime currentLastActivity = _user.LastActivityAt;

            _user.ChangeEmail("hello@world.com");

            Assert.NotEqual(_user.LastActivityAt, currentLastActivity);
        }

        [Fact]
        public void ChangeEmail_Should_Use_UserRepository()
        {
            userRepository.Expect(r => r.FindByEmail(It.IsAny<string>())).Returns((IUser) null).Verifiable();

            _user.ChangeEmail("foo@bar.com");

            userRepository.Verify();
        }

        [Fact]
        public void ChangeEmail_Should_Throw_Exception_If_Same_Email_Already_Exists()
        {
            const string Email = "foo@bar.com";

            _user.Id = Guid.NewGuid();

            var sameEmailUser = new User{ Id = Guid.NewGuid(), Email = Email };

            userRepository.Expect(r => r.FindByEmail(Email)).Returns(sameEmailUser);

            Assert.Throws<InvalidOperationException>(() => _user.ChangeEmail(Email));
        }

        [Fact]
        public void ChangePassword_Should_Update_Email()
        {
            const string CurrentPassword = "helloWorld";
            DateTime currentLastActivity = _user.LastActivityAt;

            _user.Password = CurrentPassword.Hash();

            _user.ChangePassword(CurrentPassword, "foobar");

            Assert.NotEqual(_user.LastActivityAt, currentLastActivity);
        }

        [Fact]
        public void ChangePassword_Should_Update_Password()
        {
            const string CurrentPassword = "helloWorld";

            _user.Password = CurrentPassword.Hash();

            _user.ChangePassword(CurrentPassword, "foobar");

            Assert.NotEqual(_user.Password, CurrentPassword);
        }

        [Fact]
        public void ChangePassword_Should_Update_LastActivityAt()
        {
            const string CurrentPassword = "helloWorld";
            DateTime currentLastActivity = _user.LastActivityAt;

            _user.Password = CurrentPassword.Hash();

            _user.ChangePassword(CurrentPassword, "foobar");

            Assert.NotEqual(_user.LastActivityAt, currentLastActivity);
        }

        [Fact]
        public void ChangePassword_Should_Throw_Exception_When_User_Account_Type_Is_OpenID()
        {
            Assert.Throws<InvalidOperationException>(() => _user.ChangePassword("helloWorld", "foobar"));
        }

        [Fact]
        public void ChangePassword_Should_Throw_Exception_When_OldPassword_Does_Not_Match()
        {
            _user.Password = "apassword";

            Assert.Throws<InvalidOperationException>(() => _user.ChangePassword("helloWorld", "foobar"));
        }

        [Fact]
        public void ResetPassword_Should_Return_New_Password()
        {
            _user.Password = "apassword";

            string password = _user.ResetPassword();

            Assert.True(password.Length > 0);
        }

        [Fact]
        public void ResetPassword_Should_Update_Password()
        {
            _user.Password = "apassword";

            string password = _user.ResetPassword();

            Assert.Equal(_user.Password, password.Hash());
        }

        [Fact]
        public void ResetPassword_Should_Throw_Exception_When_User_Account_Type_Is_OpenID()
        {
            Assert.Throws<InvalidOperationException>(() => _user.ResetPassword());
        }

        [Fact]
        public void Lock_Should_Update_IsLockedOut_To_True()
        {
            _user.IsLockedOut = false;

            _user.Lock();

            Assert.True(_user.IsLockedOut);
        }

        [Fact]
        public void Unlock_Should_Update_IsLockedOut_To_False()
        {
            _user.IsLockedOut = true;

            _user.Unlock();

            Assert.False(_user.IsLockedOut);
        }

        [Fact]
        public void GetScoreBetween_Should_Use_UserRepository()
        {
            userRepository.Expect(r => r.FindScoreById(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(0).Verifiable();

            _user.GetScoreBetween(SystemTime.Now().AddHours(-6), SystemTime.Now());

            userRepository.Verify();
        }

        [Fact]
        public void IncreaseScoreBy_Should_Add_New_Item_In_UserScore_Collection()
        {
            _user.UserScores.Clear();

            _user.IncreaseScoreBy(10, UserAction.AccountActivated);

            Assert.Equal(1, _user.UserScores.Count);
        }

        [Fact]
        public void DecreaseScoreBy_Should_Add_New_Item_In_UserScore_Collection()
        {
            _user.UserScores.Clear();

            _user.DecreaseScoreBy(20, UserAction.SpamStorySubmitted);

            Assert.Equal(1, _user.UserScores.Count);
        }

        [Fact]
        public void AddTag_Should_Increase_Tags_Collection()
        {
            _user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.Equal(1, _user.Tags.Count);
        }

        [Fact]
        public void RemoveTag_Should_Decrease_Tags_Collection()
        {
            _user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.Equal(1, _user.Tags.Count);

            _user.RemoveTag(new Tag { Name = "Dummy" });

            Assert.Equal(0, _user.Tags.Count);
        }

        [Fact]
        public void RemoveAllTag_Should_Clear_Tags_Collection()
        {
            _user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy1" });
            _user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy2" });
            _user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy3" });

            Assert.Equal(3, _user.Tags.Count);

            _user.RemoveAllTags();

            Assert.Empty(_user.Tags);
        }

        [Fact]
        public void ContainsTag_Should_Return_When_Tag_Exists_In_Tags_Collection()
        {
            _user.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.True(_user.ContainsTag(new Tag { Name = "Dummy" }));
        }
    }
}