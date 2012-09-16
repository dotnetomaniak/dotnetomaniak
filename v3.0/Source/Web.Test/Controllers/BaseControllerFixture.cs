using System;
using System.Web;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class BaseControllerFixture : BaseFixture
    {
        private const string UserName = "DummyUser";

        private readonly HttpContextMock _httpContext;

        private readonly Mock<IFormsAuthentication> _formsAuthentication;
        private readonly Mock<IUserRepository> _userRepository;

        private readonly BaseController _controller;

        public BaseControllerFixture()
        {
            _formsAuthentication = new Mock<IFormsAuthentication>();
            _userRepository = new Mock<IUserRepository>();

            _controller = new BaseControllerTestDouble
                                  {
                                      Settings = settings.Object,
                                      FormsAuthentication = _formsAuthentication.Object,
                                      UserRepository = _userRepository.Object
                                  };

            _httpContext = _controller.MockHttpContext();
        }

        [Fact]
        public void CurrentUserName_Should_Return_The_Current_User_Name()
        {
            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);

            Assert.Equal(UserName, _controller.CurrentUserName);
        }

        [Fact]
        public void CurrentUser_Should_Return_Non_Null_User_When_CurrentUserName_Is_Not_Blank()
        {
            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);
            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(new Mock<IUser>().Object);

            Assert.NotNull(_controller.CurrentUser);
        }

        [Fact]
        public void CurrentUser_Should_Return_Null_User_When_CurrentUserName_Is_Blank()
        {
            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(string.Empty);

            Assert.Null(_controller.CurrentUser);
        }

        [Fact]
        public void CurrentUser_Should_Update_LastActivityAt_When_User_Is_Not_Lockedout()
        {
            var user = new Mock<IUser>();

            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);
            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            user.SetupSet(u => u.LastActivityAt = It.IsAny<DateTime>()).Verifiable();
            unitOfWork.Setup(uow => uow.Commit()).Verifiable();

            #pragma warning disable 168
            var currentUser = _controller.CurrentUser;
            #pragma warning restore 168

            user.Verify();
            unitOfWork.Verify();
        }

        [Fact]
        public void CurrentUser_Should_Log_Exception_When_Exception_Occurrs()
        {
            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);
            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(new Mock<IUser>().Object);
            unitOfWork.Setup(uow => uow.Commit()).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            #pragma warning disable 168
            var currentUser = _controller.CurrentUser;
            #pragma warning restore 168

            log.Verify();
        }

        [Fact]
        public void IsCurrentUserAuthenticated_Should_Return_True_When_User_Is_Authenticated()
        {
            var user = new Mock<IUser>();

            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);
            _httpContext.User.Identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            Assert.True(_controller.IsCurrentUserAuthenticated);
        }

        [Fact]
        public void IsCurrentUserAuthenticated_Should_Return_False_When_User_Is_Not_Authenticated()
        {
            var user = new Mock<IUser>();

            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);
            _httpContext.User.Identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            Assert.True(_controller.IsCurrentUserAuthenticated);
        }

        [Fact]
        public void IsCurrentUserAuthenticated_Should_Logout_User_If_User_Is_LockedOut()
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.IsLockedOut).Returns(true);

            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);
            _httpContext.User.Identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);
            _formsAuthentication.Setup(fa => fa.SignOut()).Verifiable();

            Assert.False(_controller.IsCurrentUserAuthenticated);

            _formsAuthentication.Verify();
        }

        [Fact]
        public void CurrentUserIPAddress_Should_Return_User_IPAddress()
        {
            _httpContext.HttpRequest.SetupGet(r => r.UserHostAddress).Returns("192.168.0.1");

            Assert.Equal("192.168.0.1", _controller.CurrentUserIPAddress);
        }

        [Fact]
        public void CreateViewData_Should_Return_ViewData_Which_Should_Contain_The_Values_Of_Settings()
        {
            var viewData = _controller.CreateViewData<SupportViewData>();

            Assert.Equal(settings.Object.SiteTitle, viewData.SiteTitle);
            Assert.Equal(settings.Object.RootUrl, viewData.RootUrl);
            Assert.Equal(settings.Object.MetaKeywords, viewData.MetaKeywords);
            Assert.Equal(settings.Object.MetaDescription, viewData.MetaDescription);
        }

        [Fact]
        public void CreateViewData_Should_Return_ViewData_Which_Should_Contain_CurrentUser_And_AutheticationStatus()
        {
            var user = new Mock<IUser>();

            _httpContext.User.Identity.SetupGet(i => i.Name).Returns(UserName);
            _httpContext.User.Identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            var viewData = _controller.CreateViewData<SupportViewData>();

            Assert.Same(user.Object, viewData.CurrentUser);
            Assert.True(viewData.IsCurrentUserAuthenticated);
        }

        [Fact]
        public void Validate_Should_Return_Null_When_Provided_Expressions_Are_False()
        {
            var result = BaseController.Validate<JsonViewData>(new Validation(() => "astring".Length == 0, "String should not be blank."));

            Assert.Null(result);
        }

        [Fact]
        public void Validate_Should_Return_ErrorMessage_When_Provided_Expressions_Are_True()
        {
            var result = BaseController.Validate<JsonViewData>(new Validation(() => string.Empty.Length == 0, "String should be blank."));

            Assert.NotNull(result);
        }

        [Fact]
        public void ThrowNotFound_Should_Throw_Page_Not_Found_Exception()
        {
            Assert.Throws<HttpException>(() => BaseController.ThrowNotFound("Page not found"));
        }
    }

    public class BaseControllerTestDouble : BaseController
    {
    }
}