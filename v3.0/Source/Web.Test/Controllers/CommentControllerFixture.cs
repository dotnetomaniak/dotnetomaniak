using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Service;
    using Kigg.Test.Infrastructure;

    public class CommentControllerFixture : BaseFixture
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<reCAPTCHAValidator> _reCAPTCHA;

        private readonly Mock<IStoryRepository> _storyRepository;
        private readonly Mock<IStoryService> _storyService;

        private readonly HttpContextMock _httpContext;
        private readonly CommentController _controller;

        public CommentControllerFixture()
        {
            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();

            _userRepository = new Mock<IUserRepository>();

            _reCAPTCHA = new Mock<reCAPTCHAValidator>("http://api-verify.recaptcha.net/verify", "http://api.recaptcha.net", "https://api-secure.recaptcha.net", "bar", "bar", "hello", "world", new Mock<IHttpForm>().Object);

            _storyRepository = new Mock<IStoryRepository>();
            _storyService = new Mock<IStoryService>();

            _controller = new CommentController(_storyRepository.Object, _storyService.Object)
                              {
                                  Settings = settings.Object,
                                  FormsAuthentication = new Mock<IFormsAuthentication>().Object,
                                  UserRepository = _userRepository.Object,
                                  CaptchaValidator = _reCAPTCHA.Object
                              };

            _httpContext = _controller.MockHttpContext();

        }

        [Fact]
        public void Post_Should_Add_New_Comment()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            var result = Post();

            Assert.True(result.isSuccessful);
        }

        [Fact]
        public void Post_Should_Use_reCaptcha()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            Post();

            _reCAPTCHA.Verify();
        }

        [Fact]
        public void Post_Should_Use_StoryRepository()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            Post();

            _storyRepository.Verify();
        }

        [Fact]
        public void Post_Should_Use_StoryService()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            Post();

            _storyService.Verify();
        }

        [Fact]
        public void Post_Should_Log_Exception()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);
            SetupCaptcha();

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<InvalidOperationException>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Post(Guid.NewGuid().Shrink(), "This is a dummy comment", true);

            log.Verify();
        }

        [Fact]
        public void Post_Should_Return_Error_When_Story_Does_Not_Exists()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);
            SetupCaptcha();

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns((IStory) null);

            var result = (JsonViewData) ((JsonResult) _controller.Post(Guid.NewGuid().Shrink(), "This is a dummy comment", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Podany artykuł nie istnieje.", result.errorMessage);
        }

        [Fact]
        public void Post_Should_Return_Error_When_Captcha_Does_Not_Match()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);
            SetupCaptcha(false);

            var result = (JsonViewData)((JsonResult)_controller.Post(Guid.NewGuid().Shrink(), "This is a dummy comment", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Weryfikacja Captcha nieudana.", result.errorMessage);
        }

        [Fact]
        public void Post_Should_Return_Error_When_User_Is_Not_Authenticated()
        {
            SetupCaptcha();

            var result = (JsonViewData)((JsonResult)_controller.Post(Guid.NewGuid().Shrink(), "This is a dummy comment", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", result.errorMessage);
        }

        [Fact]
        public void Post_Should_Return_Error_When_Captcha_Response_Is_Blank()
        {
            SetupCaptcha(false, null);

            var result = (JsonViewData)((JsonResult)_controller.Post(Guid.NewGuid().Shrink(), "This is a dummy comment", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Pole Captcha nie może być puste.", result.errorMessage);
        }

        [Fact]
        public void Post_Should_Return_Error_When_Captcha_Challenge_Is_Blank()
        {
            SetupCaptcha(false, "");

            var result = (JsonViewData)((JsonResult)_controller.Post(Guid.NewGuid().Shrink(), "This is a dummy comment", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Pole Captcha nie może być puste.", result.errorMessage);
        }

        [Fact]
        public void Post_Should_Return_Error_When_Content_Is_Blank()
        {
            SetupCaptcha();
            var result = (JsonViewData)((JsonResult)_controller.Post(Guid.NewGuid().Shrink(), string.Empty, true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Komentarz nie może być pusty.", result.errorMessage);
        }

        [Fact]
        public void Post_Should_Return_Error_When_StoryId_Is_Invalid()
        {
            SetupCaptcha();

            var result = (JsonViewData)((JsonResult)_controller.Post("foobar", "This is a dummy comment", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator artykułu.", result.errorMessage);
        }

        [Fact]
        public void Post_Should_Return_Error_When_StoryId_Is_Blank()
        {
            SetupCaptcha();

            var result = (JsonViewData)((JsonResult)_controller.Post(string.Empty, "This is a dummy comment", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Identyfikator artykułu nie może być pusty.", result.errorMessage);
        }

        [Fact]
        public void ConfirmSpam_Should_Approve_The_Comment_As_Spam()
        {
            var result = ConfirmSpam(new Mock<IStory>());

            Assert.True(result.isSuccessful);
        }

        [Fact]
        public void ConfirmSpam_Should_Use_StoryRepository()
        {
            ConfirmSpam(new Mock<IStory>());

            _storyRepository.Verify();
        }

        [Fact]
        public void ConfirmSpam_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            ConfirmSpam(story);

            story.Verify();
        }

        [Fact]
        public void ConfirmSpam_Should_Use_StoryService()
        {
            ConfirmSpam(new Mock<IStory>());

            _storyService.Verify();
        }

        [Fact]
        public void ConfirmSpam_Should_Log_Exception()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<InvalidOperationException>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.ConfirmSpam(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink());

            log.Verify();
        }

        [Fact]
        public void ConfirmSpam_Should_Return_Error_When_Comment_Does_Not_Exist()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);

            var story = new Mock<IStory>();

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(story.Object);
            story.Setup(s => s.FindComment(It.IsAny<Guid>())).Returns((IComment) null);

            var result = (JsonViewData) ((JsonResult) _controller.ConfirmSpam(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Podany komentarz nie istnieje.", result.errorMessage);
        }

        [Fact]
        public void ConfirmSpam_Should_Return_Error_When_Story_Does_Not_Exist()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns((IStory) null);

            var result = (JsonViewData)((JsonResult)_controller.ConfirmSpam(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Podany artykuł nie istnieje.", result.errorMessage);
        }

        [Fact]
        public void ConfirmSpam_Should_Return_Error_When_User_Cannot_Moderate()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            var result = (JsonViewData)((JsonResult)_controller.ConfirmSpam(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Nie masz uprawnień do wołania tej metody.", result.errorMessage);
        }

        [Fact]
        public void ConfirmSpam_Should_Return_Error_When_User_Is_Not_Authenticated()
        {
            var result = (JsonViewData)((JsonResult)_controller.ConfirmSpam(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", result.errorMessage);
        }

        [Fact]
        public void ConfirmSpam_Should_Return_Error_When_Invalid_CommentId_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.ConfirmSpam(Guid.NewGuid().Shrink(), "foobar")).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator komentarza.", result.errorMessage);
        }

        [Fact]
        public void ConfirmSpam_Should_Return_Error_When_Blank_CommentId_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.ConfirmSpam(Guid.NewGuid().Shrink(), string.Empty)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Identyfikator komentarza nie może być pusty.", result.errorMessage);
        }

        [Fact]
        public void ConfirmSpam_Should_Return_Error_When_Invalid_StoryId_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.ConfirmSpam("foobar", Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator artykułu.", result.errorMessage);
        }

        [Fact]
        public void ConfirmSpam_Should_Return_Error_When_Blank_StoryId_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.ConfirmSpam(string.Empty, Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Identyfikator artykułu nie może być pusty.", result.errorMessage);
        }

        [Fact]
        public void MarkAsOffended_Should_Mark_The_Comment_As_Offended()
        {
            var result = MarkAsOffended(new Mock<IStory>());

            Assert.True(result.isSuccessful);
        }

        [Fact]
        public void MarkAsOffended_Should_Use_StoryRepository()
        {
            MarkAsOffended(new Mock<IStory>());

            _storyRepository.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Use_Story()
        {
            var story = new Mock<IStory>();

            MarkAsOffended(story);

            story.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Use_StoryService()
        {
            MarkAsOffended(new Mock<IStory>());

            _storyService.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Log_Exception()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<InvalidOperationException>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.MarkAsOffended(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink());

            log.Verify();
        }

        [Fact]
        public void MarkAsOffended_Should_Return_Error_When_Comment_Does_Not_Exist()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);

            var story = new Mock<IStory>();

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(story.Object);
            story.Setup(s => s.FindComment(It.IsAny<Guid>())).Returns((IComment)null);

            var result = (JsonViewData)((JsonResult)_controller.MarkAsOffended(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Podany komentarz nie istnieje.", result.errorMessage);
        }

        [Fact]
        public void MarkAsOffended_Should_Return_Error_When_Story_Does_Not_Exist()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns((IStory)null);

            var result = (JsonViewData)((JsonResult)_controller.MarkAsOffended(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Podany artykuł nie istnieje.", result.errorMessage);
        }

        [Fact]
        public void MarkAsOffended_Should_Return_Error_When_User_Cannot_Moderate()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            var result = (JsonViewData)((JsonResult)_controller.MarkAsOffended(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Nie masz uprawnień do wywoływania tej metody.", result.errorMessage);
        }

        [Fact]
        public void MarkAsOffended_Should_Return_Error_When_User_Is_Not_Authenticated()
        {
            var result = (JsonViewData)((JsonResult)_controller.MarkAsOffended(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", result.errorMessage);
        }

        [Fact]
        public void MarkAsOffended_Should_Return_Error_When_Invalid_CommentId_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.MarkAsOffended(Guid.NewGuid().Shrink(), "foobar")).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator komentarza.", result.errorMessage);
        }

        [Fact]
        public void MarkAsOffended_Should_Return_Error_When_Blank_CommentId_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.MarkAsOffended(Guid.NewGuid().Shrink(), string.Empty)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Identyfikator komentarza nie może być pusty.", result.errorMessage);
        }

        [Fact]
        public void MarkAsOffended_Should_Return_Error_When_Invalid_StoryId_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.MarkAsOffended("foobar", Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator artykułu.", result.errorMessage);
        }

        [Fact]
        public void MarkAsOffended_Should_Return_Error_When_Blank_StoryId_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.MarkAsOffended(string.Empty, Guid.NewGuid().Shrink())).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Identyfikator artykułu nie może być pusty.", result.errorMessage);
        }

        private void SetupCaptcha(bool captchaResult = true, string captchaResponse = "bar")
        {
            var nameValue = new NameValueCollection();
            nameValue.Add("g-recaptcha-response", captchaResponse);
            _httpContext.HttpRequest.SetupGet(r => r.Params).Returns(nameValue);
            _httpContext.HttpRequest.SetupGet(r => r.UserHostAddress).Returns("192.168.0.1");
            _controller.CaptchaValidatorFunc = s => captchaResult;
        }

        private JsonViewData Post()
        {
            SetupCaptcha();

            var story = new Mock<IStory>();

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(story.Object).Verifiable();
            _storyService.Setup(s => s.Comment(It.IsAny<IStory>(), It.IsAny<string>(), It.IsAny<IUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NameValueCollection>())).Returns(new CommentCreateResult()).Verifiable();

            return (JsonViewData) ((JsonResult) _controller.Post(Guid.NewGuid().Shrink(), "This is a dummy comment", true)).Data;
        }

        private JsonViewData ConfirmSpam(Mock<IStory> story)
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);

            var comment = new Mock<IComment>();

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(story.Object).Verifiable();
            story.Setup(s => s.FindComment(It.IsAny<Guid>())).Returns(comment.Object).Verifiable();
            _storyService.Setup(s => s.Spam(It.IsAny<IComment>(), It.IsAny<string>(), It.IsAny<IUser>())).Verifiable();

            return (JsonViewData)((JsonResult)_controller.ConfirmSpam(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;
        }

        private JsonViewData MarkAsOffended(Mock<IStory> story)
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);

            var comment = new Mock<IComment>();

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(story.Object).Verifiable();
            story.Setup(s => s.FindComment(It.IsAny<Guid>())).Returns(comment.Object).Verifiable();
            _storyService.Setup(s => s.MarkAsOffended(It.IsAny<IComment>(), It.IsAny<string>(), It.IsAny<IUser>())).Verifiable();

            return (JsonViewData)((JsonResult)_controller.MarkAsOffended(Guid.NewGuid().Shrink(), Guid.NewGuid().Shrink())).Data;
        }

        private void SetCurrentUser(Mock<IUser> user, Roles role)
        {
            const string UserName = "DummyUser";
            var userId = Guid.NewGuid();

            user.SetupGet(u => u.Id).Returns(userId);
            user.SetupGet(u => u.UserName).Returns(UserName);
            user.SetupGet(u => u.Role).Returns(role);

            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);
            _httpContext.User.Identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(UserName)).Returns(user.Object);
        }
    }
}