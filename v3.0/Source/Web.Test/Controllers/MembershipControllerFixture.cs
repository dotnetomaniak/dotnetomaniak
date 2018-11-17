using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using DotNetOpenAuth.OpenId;
//using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;

using Kigg.Service;
using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Infrastructure;
    using Repository;

    using Kigg.Test.Infrastructure;

    public class MembershipControllerFixture : BaseFixture
    {
        private readonly Mock<IDomainObjectFactory> _factory;
        private readonly Mock<IEventAggregator> _eventAggregator;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<IBlockedIPCollection> _blockedIPList;

        private readonly Mock<IFormsAuthentication> _formsAuthentication;
        private readonly Mock<IOpenIdRelyingParty> _openIdRelyingParty;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IStoryRepository> _storyRepository;

        private readonly HttpContextMock _httpContext;
        private readonly MembershipController _controller;

        public MembershipControllerFixture()
        {
            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();

            _factory = new Mock<IDomainObjectFactory>();
            _eventAggregator = new Mock<IEventAggregator>();
            _emailSender = new Mock<IEmailSender>();
            _blockedIPList = new Mock<IBlockedIPCollection>();

            _formsAuthentication = new Mock<IFormsAuthentication>();
            _openIdRelyingParty = new Mock<IOpenIdRelyingParty>();
            _userRepository = new Mock<IUserRepository>();
            _storyRepository = new Mock<IStoryRepository>();

            _storyRepository.Setup(x => x.CountByUpcoming()).Returns(0);

            resolver.Setup(r => r.Resolve<IUserRepository>()).Returns(_userRepository.Object);

            _controller = new MembershipController(_factory.Object, _eventAggregator.Object, _emailSender.Object, _blockedIPList.Object)
                              {
                                  Settings = settings.Object,
                                  UserRepository = _userRepository.Object,
                                  FormsAuthentication = _formsAuthentication.Object,
                                  OpenIdRelyingParty = _openIdRelyingParty.Object,
                                  StoryRepository = _storyRepository.Object
                              };

            _httpContext = _controller.MockHttpContext("/Kigg", null, null);
        }

        [Fact]
        public void OpenId_Should_Redirect_To_Provider()
        {
            var openIdRequest = new Mock<IAuthenticationRequest>();

            _openIdRelyingParty.Setup(o => o.CreateRequest(It.IsAny<Identifier>(), It.IsAny<Realm>())).Returns(openIdRequest.Object);
            openIdRequest.Setup(r => r.RedirectingResponse).Verifiable();

            _controller.OpenId("http://kazimanzurrashid.myopendid.com", true);

            openIdRequest.Verify();
        }

        [Fact]
        public void OpenId_Should_Login_Existing_User()
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Email).Returns("kazimanzurrashid@hotmail.com");
            OpenIdForExistingUser(user);

            _formsAuthentication.Verify();
        }

        [Fact(Skip = "Has dependency on CliamsResponse OpenID related class which has internal constructors and the class is sealed")]
        public void OpenId_Should_Update_Existing_User_Email_When_Email_Is_Different()
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Email).Returns("kazimanzurrashid@hotmail.com");
            user.Setup(u => u.ChangeEmail(It.IsAny<string>())).Verifiable();

            OpenIdForExistingUser(user);

            user.Verify();
        }

        [Fact]
        public void OpenId_Should_Signup_New_User()
        {
            OpenIdForNewUser();

            _userRepository.Verify();
        }

        [Fact]
        public void OpenId_Should_Publish_Event()
        {
            OpenIdForNewUser();

            _eventAggregator.Verify();
        }

        [Fact]
        public void OpenId_Should_Login_New_User()
        {
            OpenIdForNewUser();

            _formsAuthentication.Verify();
        }

        [Fact]
        public void OpenId_Should_Redirect_To_Home()
        {
            var result = (RedirectToRouteResult) OpenIdForNewUser();

            Assert.Equal("Published", result.RouteName);
        }

        [Fact]
        public void OpenId_Should_Generate_Error_Cookie_When_Login_Failed()
        {
            var openIdResponse = new Mock<IAuthenticationResponse>();

            openIdResponse.SetupGet(r => r.Status).Returns(AuthenticationStatus.Failed);
            _openIdRelyingParty.SetupGet(o => o.Response).Returns(openIdResponse.Object);

            _controller.OpenId("http://kazimanzurrashid.myopenid.com", true);

            var cookie = _httpContext.HttpResponse.Object.Cookies["notification"];

            Assert.NotNull(cookie);
        }

        [Fact]
        public void OpenId_Should_Generate_Error_Cookie_When_Exception_Occurrs()
        {
            _openIdRelyingParty.SetupGet(o => o.Response).Throws<Exception>();

            _controller.OpenId("http://kazimanzurrashid.myopenid.com", true);

            var cookie = _httpContext.HttpResponse.Object.Cookies["notification"];

            Assert.NotNull(cookie);
        }

        [Fact]
        public void OpenId_Should_Generate_Error_Cookie_When_User_Is_LockedOut()
        {
            var openIdResponse = new Mock<IAuthenticationResponse>();

            openIdResponse.SetupGet(r => r.Status).Returns(AuthenticationStatus.Authenticated);
            openIdResponse.SetupGet(r => r.FriendlyIdentifierForDisplay).Returns("kazimanzurrashid.myopenid.com");

            _openIdRelyingParty.SetupGet(o => o.Response).Returns(openIdResponse.Object);

            var user = new Mock<IUser>();
            user.SetupGet(u => u.IsLockedOut).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            _controller.OpenId("http://kazimanzurrashid.myopenid.com", true);

            var cookie = _httpContext.HttpResponse.Object.Cookies["notification"];

            Assert.NotNull(cookie);
        }

        [Fact]
        public void Signup_Should_Create_New_Account()
        {
            var data = Signup();

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void Signup_Should_Use_DomainObjectFactory()
        {
            Signup();

            _factory.Verify();
        }

        [Fact]
        public void Signup_Should_Use_UserRepository()
        {
            Signup();

            _userRepository.Verify();
        }

        [Fact]
        public void Signup_Should_Use_EmailSender()
        {
            Signup();

            _emailSender.Verify();
        }

        [Fact]
        public void Signup_Should_Use_Log()
        {
            Signup();

            log.Verify();
        }

        [Fact]
        public void Signup_Should_Log_Exception()
        {
            _factory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<InvalidOperationException>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Signup("dummyuser", "xxxxxx", "foo@bar.com");

            log.Verify();
        }

        [Fact]
        public void Signup_Should_Return_Error_When_Same_User_Already_Exists()
        {
            _factory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new Mock<IUser>().Object);
            _userRepository.Setup(r => r.Add(It.IsAny<IUser>())).Throws<ArgumentException>();

            var data  = (JsonViewData) ((JsonResult) _controller.Signup("dummyuser", "xxxxxx", "foo@bar.com")).Data;

            Assert.False(data.isSuccessful);
        }

        [Fact]
        public void Signup_Should_Return_Error_When_Invalid_Email_Is_Specified()
        {
            var data = (JsonViewData)((JsonResult)_controller.Signup("dummyuser", "xxxxxx", "foo")).Data;

            Assert.False(data.isSuccessful);
            Assert.Equal("Niepoprawny adres e-mail.", data.errorMessage);
        }

        [Fact]
        public void Signup_Should_Return_Error_When_Blank_Email_Is_Specified()
        {
            var data = (JsonViewData)((JsonResult)_controller.Signup("dummyuser", "xxxxxx", string.Empty)).Data;

            Assert.False(data.isSuccessful);
            Assert.Equal("Adres e-mail nie może być pusty.", data.errorMessage);
        }

        [Fact]
        public void Login_Should_Login_User()
        {
            var user = new Mock<IUser>();

            var data = Login(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void LoginWithEmail_ShouldLogin_User()
        {
            var user = new Mock<IUser>();

            var data = LoginWithEmail(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void Login_Should_Use_UserRepository()
        {
            var user = new Mock<IUser>();

            Login(user);

            _userRepository.Verify();
        }

        [Fact]
        public void Login_Should_Use_User()
        {
            var user = new Mock<IUser>();

            Login(user);

            user.Verify();
        }

        [Fact]
        public void Login_Should_Use_FormsAuthentication()
        {
            var user = new Mock<IUser>();

            Login(user);

            _userRepository.Verify();
        }

        [Fact]
        public void Login_Should_Log()
        {
            var user = new Mock<IUser>();

            Login(user);

            log.Verify();
        }

        [Fact]
        public void Login_Should_Log_Exception()
        {
            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Throws<InvalidOperationException>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Login("dummyuser", "xxxxxx", null);

            log.Verify();
        }

        [Fact]
        public void Login_Should_Return_Error_When_Credentials_Are_Invalid()
        {
            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns((IUser) null);

            var result = (JsonViewData)((JsonResult)_controller.Login("dummyuser", "xxxxxx", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Niepoprawne dane logowania.", result.errorMessage);
        }

        [Fact]
        public void Login_Should_Return_Error_When_User_Account_Type_Is_OpenID()
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Password).Returns((string) null);
            user.SetupGet(u => u.IsActive).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            var result =(JsonViewData)((JsonResult)_controller.Login("dummyuser", "xxxxxx", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Podany login jest poprawny tylko z OpenID.", result.errorMessage);
        }

        [Fact]
        public void Login_Should_Return_Error_When_User_Is_Not_Active()
        {
            var user = new Mock<IUser>();

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            var result = (JsonViewData)((JsonResult)_controller.Login("dummyuser", "xxxxxx", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Twoje konto nie zostało jeszcze aktywowane. Na Twoją skrzynkę e-mail wysłano ponownie link aktywacyjny.", result.errorMessage);
        }

        [Fact]
        public void Login_Should_Resend_Activation_Email_When_User_Is_Not_Active()
        {
            var user = new Mock<IUser>();

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            var result = (JsonViewData)((JsonResult)_controller.Login("dummyuser", "xxxxxx", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Twoje konto nie zostało jeszcze aktywowane. Na Twoją skrzynkę e-mail wysłano ponownie link aktywacyjny.", result.errorMessage);
            _emailSender.Verify();
        }

        [Fact]
        public void Login_Should_Not_Resend_Activation_Email_When_User_Is_Active()
        {
            var user = new Mock<IUser>();

            var data = Login(user);

            Assert.True(data.isSuccessful);

            _emailSender.Verify(f => f.SendRegistrationInfo("","","",""), Times.Never());
        }

        [Fact]
        public void Login_Should_Return_Error_When_User_Is_LockedOut()
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.IsLockedOut).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);

            var result = (JsonViewData)((JsonResult)_controller.Login("dummyuser", "xxxxxx", true)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Twoje konto jest aktualnie zablokowane. Skontaktuj się z pomocą aby rozwiązać ten problem.", result.errorMessage);
        }

        [Fact]
        public void Login_Should_Return_Error_When_Blank_Password_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.Login("dummyuser", string.Empty, null)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Hasło nie może być puste.", result.errorMessage);
        }

        [Fact]
        public void Login_Should_Return_Error_When_Blank_UserName_Is_Specified()
        {
            var result = (JsonViewData)((JsonResult)_controller.Login(string.Empty, "xxxxxx", null)).Data;

            Assert.False(result.isSuccessful);
            Assert.Equal("Nazwa użytkownika/e-mail nie może być pusty.", result.errorMessage);
        }

        [Fact]
        public void Logout_Should_LogOff_CurrentUser()
        {
            var user = new Mock<IUser>();

            var data = Logout(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void Logout_Should_Use_User()
        {
            var user = new Mock<IUser>();

            Logout(user);

            user.Verify();
        }

        [Fact]
        public void Logout_Should_Use_FormsAuthentication()
        {
            var user = new Mock<IUser>();

            Logout(user);

            _formsAuthentication.Verify();
        }

        [Fact]
        public void Logout_Should_Use_Log()
        {
            var user = new Mock<IUser>();

            Logout(user);

            log.Verify();
        }

        [Fact]
        public void Logout_Should_Log_Exception()
        {
            var user = new Mock<IUser>();

            SetCurrentUser(user, Roles.User);

            _formsAuthentication.Setup(fa => fa.SignOut()).Throws<InvalidOperationException>();
            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Logout();

            log.Verify();
        }

        [Fact]
        public void Logout_Should_Return_Error_When_Calling_User_Is_Not_Authenticated()
        {
            var result = (JsonResult)_controller.Logout();
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", viewData.errorMessage);
        }

        [Fact]
        public void ForgotPassword_Should_Send_New_Password()
        {
            var user = new Mock<IUser>();

            var data = ForgotPassword(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void ForgotPassword_Should_Use_UserRepository()
        {
            var user = new Mock<IUser>();

            ForgotPassword(user);

            _userRepository.Verify();
        }

        [Fact]
        public void ForgotPassword_Should_Use_User()
        {
            var user = new Mock<IUser>();

            ForgotPassword(user);

            user.Verify();
        }

        [Fact]
        public void ForgotPassword_Should_Use_EmailSender()
        {
            var user = new Mock<IUser>();

            ForgotPassword(user);

            _emailSender.Verify();
        }

        [Fact]
        public void ForgotPassword_Should_Log()
        {
            var user = new Mock<IUser>();

            ForgotPassword(user);

            log.Verify();
        }

        [Fact]
        public void ForgotPassword_Should_Log_Exception()
        {
            _userRepository.Setup(r => r.FindByEmail(It.IsAny<string>())).Throws<InvalidOperationException>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.ForgotPassword("foo@bar.com");

            log.Verify();
        }

        [Fact]
        public void ForgotPassword_Should_Return_Error_When_User_With_Specified_Email_Does_Not_Exist()
        {
            _userRepository.Setup(r => r.FindByEmail(It.IsAny<string>())).Returns((IUser) null);

            var result = (JsonResult)_controller.ForgotPassword("foo@bar.com");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie znaleziono użytkownika z podanym adresem e-mail.", viewData.errorMessage);
        }

        [Fact]
        public void ForgotPassword_Should_Return_Error_When_Reseting_OpenID_Account()
        {
            var user = new Mock<IUser>();

            user.Setup(u => u.ResetPassword()).Throws<InvalidOperationException>();

            _userRepository.Setup(r => r.FindByEmail(It.IsAny<string>())).Returns(user.Object);

            var result = (JsonResult)_controller.ForgotPassword("foo@bar.com");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
        }

        [Fact]
        public void ForgotPassword_Should_Return_Error_When_Invalid_Email_Is_Specified()
        {
            var result = (JsonResult) _controller.ForgotPassword("foobar");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Niepoprawny adres e-mail.", viewData.errorMessage);
        }

        [Fact]
        public void ForgotPassword_Should_Return_Error_When_Blank_Email_Is_Specified()
        {
            var result = (JsonResult)_controller.ForgotPassword(string.Empty);
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Pole e-mail nie może być puste.", viewData.errorMessage);
        }

        [Fact]
        public void ChangePassword_Should_Update_CurrentUser_Password()
        {
            var user = new Mock<IUser>();

            var data = ChangePassword(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void ChangePassword_Should_Use_User()
        {
            var user = new Mock<IUser>();

            ChangePassword(user);

            user.Verify();
        }

        [Fact]
        public void ChangePassword_Should_Log_Exception()
        {
            var user = new Mock<IUser>();

            SetCurrentUser(user, Roles.User);

            user.Setup(u => u.ChangePassword(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.ChangePassword("xxxxxxxx", "yyyyyyyy", "yyyyyyyy");

            log.Verify();
        }

        [Fact]
        public void ChangePassword_Should_Return_Error_When_Old_Password_Does_Not_Match_With_Current_Password()
        {
            var user = new Mock<IUser>();

            SetCurrentUser(user, Roles.User);

            user.Setup(u => u.ChangePassword(It.IsAny<string>(), It.IsAny<string>())).Throws<InvalidOperationException>();

            var viewData = (JsonViewData)((JsonResult)_controller.ChangePassword("xxxxxxxx", "yyyyyyyy", "yyyyyyyy")).Data;

            Assert.False(viewData.isSuccessful);
        }

        [Fact]
        public void ChangePassword_Should_Return_Error_When_Calling_User_Is_Not_Authenticated()
        {
            var result = (JsonResult)_controller.ChangePassword("xxxxxx", "yyyyyyyy", "yyyyyyyy");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", viewData.errorMessage);
        }

        [Fact]
        public void ChangePassword_Should_Return_Error_When_New_Password_And_Confirm_Password_Does_Not_Match()
        {
            var result = (JsonResult)_controller.ChangePassword("xxxxxx", "yyyyyyyy", "zzzzzzzz");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nowe hasło i jego potwierdzenie są różne.", viewData.errorMessage);
        }

        [Fact]
        public void ChangePassword_Should_Return_Error_When_New_Password_Length_Is_Less_Than_Minimum_Length()
        {
            var result = (JsonResult)_controller.ChangePassword("xxxxxx", "yy", "yy");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nowe hasło nie może być krótsze niż 4 znaki.", viewData.errorMessage);
        }

        [Fact]
        public void ChangePassword_Should_Return_Error_When_Blank_New_Password_Is_Specified()
        {
            var result = (JsonResult)_controller.ChangePassword("xxxxxx", string.Empty, "yyyyyy");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nowe hasło nie może być puste.", viewData.errorMessage);
        }

        [Fact]
        public void ChangePassword_Should_Return_Error_When_Blank_Old_Password_Is_Specified()
        {
            var result = (JsonResult) _controller.ChangePassword(string.Empty, "yyyyyy", "yyyyyy");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Stare hasło nie może być puste.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeEmail_Should_Update_CurrentUser_Email()
        {
            var user = new Mock<IUser>();

            var data = ChangeEmail(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void ChangeEmail_Should_Use_User()
        {
            var user = new Mock<IUser>();

            ChangeEmail(user);

            user.Verify();
        }

        [Fact]
        public void ChangeEmail_Should_Log_Exception()
        {
            var user = new Mock<IUser>();

            SetCurrentUser(user, Roles.User);

            user.Setup(u => u.ChangeEmail(It.IsAny<string>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.ChangeEmail("foo@bar.com");

            log.Verify();
        }

        [Fact]
        public void ChangeEmail_Should_Return_Error_When_Specified_Email_Already_Exist()
        {
            var user = new Mock<IUser>();

            SetCurrentUser(user, Roles.User);

            user.Setup(u => u.ChangeEmail(It.IsAny<string>())).Throws<InvalidOperationException>();

            _controller.ChangeEmail("foo@bar.com");

            var result = (JsonResult)_controller.ChangeEmail("foo@bar.com");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
        }

        [Fact]
        public void ChangeEmail_Should_Return_Error_When_Calling_User_Is_Not_Authenticated()
        {
            var result = (JsonResult) _controller.ChangeEmail("foo@bar.com");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeEmail_Should_Return_Error_When_Invalid_Email_Is_Specified()
        {
            var result = (JsonResult)_controller.ChangeEmail("foobar");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Niepoprawny adres e-mail.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeEmail_Should_Return_Error_When_Blank_Email_Is_Specified()
        {
            var result = (JsonResult)_controller.ChangeEmail(string.Empty);
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Adres e-mail nie może być pusty.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeRole_Should_Update_User_Role()
        {
            var user = new Mock<IUser>();

            var data = ChangeRole(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void ChangeRole_Should_Use_UserRepository()
        {
            var user = new Mock<IUser>();

            ChangeRole(user);

            _userRepository.Verify();
        }

        [Fact]
        public void ChangeRole_Should_Use_User()
        {
            var user = new Mock<IUser>();

            ChangeRole(user);

            user.Verify();
        }

        [Fact]
        public void ChangeRole_Should_Log_Exception()
        {
            SetAdmin();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.ChangeRole(Guid.NewGuid().Shrink(), Roles.Moderator.ToString());

            log.Verify();
        }

        [Fact]
        public void ChangeRole_Should_Return_Error_When_Specified_User_Does_Not_Exist()
        {
            SetAdmin();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns((IUser)null);

            var result = (JsonResult) _controller.ChangeRole(Guid.NewGuid().Shrink(), ((int) Roles.Moderator).ToString());
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Podany użytkownik nie istnieje.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeRole_Should_Return_Error_When_Calling_User_Is_Not_Authenticated()
        {
            var result = (JsonResult)_controller.ChangeRole(Guid.NewGuid().Shrink(), "admin");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeRole_Should_Return_Error_When_Calling_User_Is_Not_Admin()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            var result = (JsonResult)_controller.ChangeRole(Guid.NewGuid().Shrink(), "admin");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie masz uprawnień do wywoływania tej metody.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeRole_Should_Return_Error_When_Invalid_UserId_Is_Specified()
        {
            var result = (JsonResult)_controller.ChangeRole("foobar", "4");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator użytkownika.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeRole_Should_Return_Error_When_Blank_UserId_Is_Specified()
        {
            var result = (JsonResult)_controller.ChangeRole(string.Empty, string.Empty);
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Identyfikator użytkownika nie może być pusty.", viewData.errorMessage);
        }

        [Fact]
        public void ChangeRole_Should_Return_Error_When_Blank_Role_Is_Specified()
        {
            var result = (JsonResult)_controller.ChangeRole(Guid.NewGuid().Shrink(), string.Empty);
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Rola nie może być pusta.", viewData.errorMessage);
        }

        [Fact]
        public void Lock_Should_Lock_The_Specified_User()
        {
            var user = new Mock<IUser>();

            var data = Lock(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void Lock_Should_Use_UserRepository()
        {
            var user = new Mock<IUser>();

            Lock(user);

            _userRepository.Verify();
        }

        [Fact]
        public void Lock_Should_Use_User()
        {
            var user = new Mock<IUser>();

            Lock(user);

            user.Verify();
        }

        [Fact]
        public void Lock_Should_Return_Error_When_Specified_User_Does_Not_Exist()
        {
            SetAdmin();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns((IUser)null);

            var result = (JsonResult) _controller.Lock(Guid.NewGuid().Shrink());
            var viewData = (JsonViewData) result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Podany użytkownik nie istnieje.", viewData.errorMessage);
        }

        [Fact]
        public void Lock_Should_Return_Error_When_Calling_User_Is_Not_Authenticated()
        {
            var result = (JsonResult)_controller.Lock(Guid.NewGuid().Shrink());
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", viewData.errorMessage);
        }

        [Fact]
        public void Lock_Should_Return_Error_When_Calling_User_Is_Not_Admin()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            var result = (JsonResult) _controller.Lock(Guid.NewGuid().Shrink());
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie masz uprawnień do wywoływania tej metody.", viewData.errorMessage);
        }

        [Fact]
        public void Lock_Should_Return_Error_When_Invalid_UserId_Is_Specified()
        {
            var result = (JsonResult)_controller.Lock("foobar");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator użytkownika.", viewData.errorMessage);
        }

        [Fact]
        public void Lock_Should_Return_Error_When_Blank_UserId_Is_Specified()
        {
            var result = (JsonResult)_controller.Lock(string.Empty);
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Identyfikator użytkownika nie może być pusty.", viewData.errorMessage);
        }

        [Fact]
        public void Lock_Should_Log_Exception()
        {
            SetAdmin();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Lock(Guid.NewGuid().Shrink());

            log.Verify();
        }

        [Fact]
        public void Unlock_Should_Unlock_The_Specified_User()
        {
            var user = new Mock<IUser>();

            var data = Unlock(user);

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void Unlock_Should_Use_UserRepository()
        {
            var user = new Mock<IUser>();

            Unlock(user);

            _userRepository.Verify();
        }

        [Fact]
        public void Unlock_Should_Use_User()
        {
            var user = new Mock<IUser>();

            Unlock(user);

            user.Verify();
        }

        [Fact]
        public void Unlock_Should_Return_Error_When_Specified_User_Does_Not_Exist()
        {
            SetAdmin();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns((IUser)null);

            var result = (JsonResult)_controller.Unlock(Guid.NewGuid().Shrink());
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Podany użytkownik nie istnieje.", viewData.errorMessage);
        }

        [Fact]
        public void Unlock_Should_Return_Error_When_Calling_User_Is_Not_Authenticated()
        {
            var result = (JsonResult)_controller.Unlock(Guid.NewGuid().Shrink());
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", viewData.errorMessage);
        }

        [Fact]
        public void Unlock_Should_Return_Error_When_Calling_User_Is_Not_Admin()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Moderator);

            var result = (JsonResult) _controller.Unlock(Guid.NewGuid().Shrink());
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie masz uprawnień do wywoływania tej metody.", viewData.errorMessage);
        }

        [Fact]
        public void Unlock_Should_Return_Error_When_Invalid_UserId_Is_Specified()
        {
            var result = (JsonResult)_controller.Unlock("foobar");
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator użytkownika.", viewData.errorMessage);
        }

        [Fact]
        public void Unlock_Should_Return_Error_When_Blank_UserId_Is_Specified()
        {
            var result = (JsonResult)_controller.Unlock(string.Empty);
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Identyfikator użytkownika nie może być pusty.", viewData.errorMessage);
        }

        [Fact]
        public void Unlock_Should_Log_Exception()
        {
            SetAdmin();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Unlock(Guid.NewGuid().Shrink());

            log.Verify();
        }

        [Fact]
        public void AllowIps_Should_Allow_The_Specified_Ips()
        {
            var data = AllowIps();

            Assert.True(data.isSuccessful);
        }

        [Fact]
        public void AllowIps_Should_Use_UserRepository()
        {
            AllowIps();

            _userRepository.Verify();
        }

        [Fact]
        public void AllowIps_Should_Use_BlockedIPCollection()
        {
            AllowIps();

            _blockedIPList.Verify();
        }

        [Fact]
        public void AllowIps_Should_Return_Error_When_Specified_User_Does_Not_Exist()
        {
            SetAdmin();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns((IUser) null);

            var result = (JsonResult) _controller.AllowIps(Guid.NewGuid().Shrink(), new[] { "192.168.0.1" });
            var viewData = (JsonViewData) result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Podany użytkownik nie istnieje.", viewData.errorMessage);
        }

        [Fact]
        public void AllowIps_Should_Return_Error_When_Calling_User_Is_Not_Authenticated()
        {
            var result = (JsonResult)_controller.AllowIps(Guid.NewGuid().Shrink(), new[] { "192.168.0.1" });
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie jesteś zalogowany.", viewData.errorMessage);
        }

        [Fact]
        public void AllowIps_Should_Return_Error_When_Calling_User_Is_Not_Admin()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            var result = (JsonResult)_controller.AllowIps(Guid.NewGuid().Shrink(), new[] { "192.168.0.1" });
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Nie masz uprawnień do wywoływania tej metody.", viewData.errorMessage);
        }

        [Fact]
        public void AllowIps_Should_Return_Error_When_Invalid_UserId_Is_Specified()
        {
            var result = (JsonResult)  _controller.AllowIps("foobar", new[] { "192.168.0.1" });
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Niepoprawny identyfikator użytkownika.", viewData.errorMessage);
        }

        [Fact]
        public void AllowIps_Should_Return_Error_When_Blank_UserId_Is_Specified()
        {
            var result = (JsonResult)_controller.AllowIps(string.Empty, new[] { "192.168.0.1" });
            var viewData = (JsonViewData)result.Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Identyfikator użytkownika nie może być pusty.", viewData.errorMessage);
        }

        [Fact]
        public void AllowIps_Should_Log_Exception()
        {
            SetAdmin();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.AllowIps(Guid.NewGuid().Shrink(), new[] { "192.168.0.1" });

            log.Verify();
        }

        [Fact]
        public void List_Should_Render_Default_View()
        {
            var result = List();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void List_Should_Use_UserRepository()
        {
            List();

            _userRepository.Verify();
        }

        [Fact]
        public void Detail_Should_Render_Default_View()
        {
            var result = Detail();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void Detail_Should_Use_UserRepository()
        {
            Detail();

            _userRepository.Verify();
        }

        [Fact]
        public void Detail_Should_Redirect_To_Users_When_Blank_Name_Is_Specified()
        {
            var result = (RedirectToRouteResult) _controller.Detail(null, null, null);

            Assert.Equal("Users", result.RouteName);
        }

        [Fact]
        public void Detail_Should_Throw_Not_Found_When_Specified_User_Does_Not_Exist()
        {
            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns((IUser) null);

            Assert.Throws<HttpException>(() => _controller.Detail("foo", null, null));
        }

        [Fact]
        public void Activate_Should_Redirect_To_Published()
        {
            var result = (RedirectToRouteResult) _controller.Activate("foo");

            Assert.Equal("Published", result.RouteName);
        }

        [Fact]
        public void Activate_Should_Use_UserRepository()
        {
            var user = new Mock<IUser>();

            Activate(ref user);

            _userRepository.Verify();
        }

        [Fact]
        public void Activate_Should_Use_User()
        {
            var user = new Mock<IUser>();

            Activate(ref user);

            user.Verify();
        }

        [Fact]
        public void Activate_Should_Publish_Event()
        {
            var user = new Mock<IUser>();

            Activate(ref user);

            _eventAggregator.Verify();
        }

        [Fact]
        public void Activate_Should_Use_Log()
        {
            var user = new Mock<IUser>();

            Activate(ref user);

            log.Verify();
        }

        [Fact]
        public void Activate_Should_Generate_Successful_Cookie()
        {
            var user = new Mock<IUser>();

            Activate(ref user);

            Assert.True(_httpContext.HttpResponse.Object.Cookies.Count > 0);
        }

        [Fact]
        public void Active_Should_Log_Exception()
        {
            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Activate(Guid.NewGuid().Shrink());

            log.Verify();
        }

        [Fact]
        public void Activate_Should_Generate_Error_Cookie_When_Exception_Occur()
        {
            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            _controller.Activate(Guid.NewGuid().Shrink());

            Assert.True(_httpContext.HttpResponse.Object.Cookies.Count > 0);
        }

        [Fact]
        public void Activate_Should_Generate_Error_Cookie_When_User_Not_Found_Or_User_Account_Is_Already_Activated()
        {
            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns((IUser) null);

            _controller.Activate(Guid.NewGuid().Shrink());

            Assert.True(_httpContext.HttpResponse.Object.Cookies.Count > 0);
        }

        [Fact]
        public void Menu_Should_Render_Default_View()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.User);

            var result = (ViewResult)_controller.Menu();
            var viewData = result.ViewData.Model;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.IsType<UserMenuViewData>(viewData);
        }

        [Fact]
        public void TopTabs_Should_Render_Default_View()
        {
            var result = TopTabs();
            var viewData = result.ViewData.Model;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.IsType<TopUserTabsViewData>(viewData);
        }

        [Fact]
        public void TopTabs_Should_Use_UserRepository()
        {
            TopTabs();

            _userRepository.Verify();
        }

        private ActionResult OpenIdForNewUser()
        {
            var openIdResponse = new Mock<IAuthenticationResponse>();
            

            openIdResponse.SetupGet(r => r.Status).Returns(AuthenticationStatus.Authenticated);
            openIdResponse.SetupGet(r => r.ClaimedIdentifier).Returns("kazimanzurrashid.myopenid.com");
            
            _openIdRelyingParty.SetupGet(o => o.Response).Returns(openIdResponse.Object);

            var user = new Mock<IUser>();
            user.SetupGet(u => u.Email).Returns("kazimanzurrashid@hotmail.com");

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns((IUser)null);

            _factory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(user.Object);
            _userRepository.Setup(r => r.Add(It.IsAny<IUser>())).Verifiable();
            _eventAggregator.Setup(ea => ea.GetEvent<UserActivateEvent>()).Returns(new UserActivateEvent()).Verifiable();

            _formsAuthentication.Setup(fa => fa.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>())).Verifiable();

            _httpContext.HttpRequest.Object.Cookies.Add(new HttpCookie("oidr", "foo"));
            _httpContext.HttpResponse.Object.Cookies.Add(new HttpCookie("oidr", bool.TrueString));

            return _controller.OpenId("http://kazimanzurrashid.myopenid.com", true);
        }

        private void OpenIdForExistingUser(Mock<IUser> user)
        {
            var openIdResponse = new Mock<IAuthenticationResponse>();
            
            #pragma warning disable 618,612
            //var claim = new ClaimsResponse { Email = "kazimanzurrashid@gmail.com" };
            #pragma warning restore 618,612

            openIdResponse.SetupGet(r => r.Status).Returns(AuthenticationStatus.Authenticated);
            openIdResponse.SetupGet(r => r.ClaimedIdentifier).Returns("kazimanzurrashid.myopenid.com");
            //openIdResponse.Setup(r => r.GetExtension<ClaimsResponse>()).Returns(claim);

            _openIdRelyingParty.SetupGet(o => o.Response).Returns(openIdResponse.Object);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object);
            _formsAuthentication.Setup(fa => fa.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>())).Verifiable();

            _httpContext.HttpRequest.Object.Cookies.Add(new HttpCookie("oidr", "foo"));
            _httpContext.HttpResponse.Object.Cookies.Add(new HttpCookie("oidr", bool.TrueString));

            _controller.OpenId("http://kazimanzurrashid.myopenid.com", true);
        }

        private JsonViewData Signup()
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Id).Returns(Guid.NewGuid());

            _factory.Setup(f => f.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(user.Object).Verifiable();
            _userRepository.Setup(r => r.Add(It.IsAny<IUser>())).Verifiable();
            _emailSender.Setup(es => es.SendRegistrationInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();

            return (JsonViewData) ((JsonResult) _controller.Signup("dummyuser", "xxxxx", "dummy@users.com")).Data;
        }

        private JsonViewData Login(Mock<IUser> user)
        {
            user.SetupGet(u => u.Password).Returns("xxxxxx".Hash());
            user.SetupGet(u => u.IsActive).Returns(true);

            _userRepository.Setup(r => r.FindByUserName(It.IsAny<string>())).Returns(user.Object).Verifiable();
            user.SetupSet(u => u.LastActivityAt = It.IsAny<DateTime>()).Verifiable();
            _formsAuthentication.Setup(fa => fa.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            return (JsonViewData) ((JsonResult) _controller.Login("dummyuser", "xxxxxx", true)).Data;
        }

        private JsonViewData LoginWithEmail(Mock<IUser> user)
        {
            user.SetupGet(u => u.Password).Returns("xxxxxx".Hash());
            user.SetupGet(u => u.IsActive).Returns(true);

            _userRepository.Setup(r => r.FindByEmail(It.IsAny<string>())).Returns(user.Object).Verifiable();
            user.SetupSet(u => u.LastActivityAt = It.IsAny<DateTime>()).Verifiable();
            _formsAuthentication.Setup(fa => fa.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>())).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            return (JsonViewData) ((JsonResult) _controller.Login("dummyemail@email.com", "xxxxxx", true)).Data;
        }

        private JsonViewData Logout(Mock<IUser> user)
        {
            SetCurrentUser(user, Roles.User);

            user.SetupSet(u => u.LastActivityAt = It.IsAny<DateTime>()).Verifiable();
            _formsAuthentication.Setup(fa => fa.SignOut()).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            return (JsonViewData)((JsonResult)_controller.Logout()).Data;
        }

        private JsonViewData ForgotPassword(Mock<IUser> user)
        {
            _userRepository.Setup(r => r.FindByEmail(It.IsAny<string>())).Returns(user.Object).Verifiable();
            user.Setup(u => u.ResetPassword()).Returns("xxxxxx").Verifiable();
            _emailSender.Setup(es => es.SendNewPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            return (JsonViewData) ((JsonResult) _controller.ForgotPassword("foo@bar.com")).Data;
        }

        private JsonViewData ChangePassword(Mock<IUser> user)
        {
            SetCurrentUser(user, Roles.User);

            user.Setup(u => u.ChangePassword(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            return (JsonViewData)((JsonResult)_controller.ChangePassword("xxxxxxxx", "yyyyyyyy", "yyyyyyyy")).Data;
        }

        private JsonViewData ChangeEmail(Mock<IUser> user)
        {
            SetCurrentUser(user, Roles.User);

            user.Setup(u => u.ChangeEmail(It.IsAny<string>())).Verifiable();

            return (JsonViewData)((JsonResult)_controller.ChangeEmail("foo@bar.com")).Data;
        }

        private JsonViewData ChangeRole(Mock<IUser> user)
        {
            SetAdmin();

            var userId = Guid.NewGuid();

            user.SetupGet(u => u.Id).Returns(userId);
            user.SetupSet(u => u.Role = It.IsAny<Roles>()).Verifiable();

            _userRepository.Setup(r => r.FindById(userId)).Returns(user.Object).Verifiable();

            return (JsonViewData) ((JsonResult)_controller.ChangeRole(userId.Shrink(), Roles.Moderator.ToString())).Data;
        }

        private JsonViewData Unlock(Mock<IUser> user)
        {
            SetAdmin();

            var userId = Guid.NewGuid();

            user.SetupGet(u => u.Id).Returns(userId);
            user.Setup(u => u.Unlock()).Verifiable();

            _userRepository.Setup(r => r.FindById(userId)).Returns(user.Object).Verifiable();

            return (JsonViewData) ((JsonResult) _controller.Unlock(userId.Shrink())).Data;
        }

        private JsonViewData Lock(Mock<IUser> user)
        {
            SetAdmin();

            var userId = Guid.NewGuid();

            user.SetupGet(u => u.Id).Returns(userId);
            user.Setup(u => u.Lock()).Verifiable();

            _userRepository.Setup(r => r.FindById(userId)).Returns(user.Object).Verifiable();

            return (JsonViewData) ((JsonResult)_controller.Lock(userId.Shrink())).Data;
        }

        private JsonViewData AllowIps()
        {
            SetAdmin();

            var userId = Guid.NewGuid();
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Id).Returns(userId);

            _userRepository.Setup(r => r.FindById(userId)).Returns(user.Object).Verifiable();
            _userRepository.Setup(r => r.FindIPAddresses(userId)).Returns(new[] { "192.168.0.1", "192.168.0.2" }).Verifiable();

            _blockedIPList.Setup(bl => bl.AddRange(It.IsAny<ICollection<string>>())).Verifiable();
            _blockedIPList.Setup(bl => bl.RemoveRange(It.IsAny<ICollection<string>>())).Verifiable();

            return (JsonViewData) ((JsonResult)_controller.AllowIps(userId.Shrink(), new[] { "192.168.0.1", "192.168.0.3" })).Data;
        }

        private ViewResult List()
        {
            _userRepository.Setup(r => r.FindAll(It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IUser>()).Verifiable();

            return (ViewResult) _controller.List(2);
        }

        private ViewResult Detail()
        {
            string userId = Guid.NewGuid().Shrink();

            SetAdmin();

            _userRepository.Setup(r => r.FindById(userId.ToGuid())).Returns(new Mock<IUser>().Object).Verifiable();
            _userRepository.Setup(r => r.FindIPAddresses(It.IsAny<Guid>())).Returns(new[] { "192.168.0.1" }).Verifiable();

            return (ViewResult)_controller.Detail(userId, "Promoted", null);
        }

        private void Activate(ref Mock<IUser> user)
        {
            user = new Mock<IUser>();

            user.SetupSet(u => u.IsActive = true).Verifiable();
            user.SetupSet(u => u.LastActivityAt = It.IsAny<DateTime>()).Verifiable();

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(user.Object).Verifiable();

            _eventAggregator.Setup(ea => ea.GetEvent<UserActivateEvent>()).Returns(new UserActivateEvent()).Verifiable();

            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _controller.Activate(Guid.NewGuid().Shrink());
        }

        private ViewResult TopTabs()
        {
            var user1 = new Mock<IUser>();
            user1.SetupGet(u => u.CurrentScore).Returns(1000);

            var user2 = new Mock<IUser>();
            user2.SetupGet(u => u.CurrentScore).Returns(1000);

            var user3 = new Mock<IUser>();
            user2.SetupGet(u => u.CurrentScore).Returns(1000);

            _userRepository.Setup(r => r.FindTop(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IUser>(new List<IUser> { user1.Object, user2.Object, user3.Object }, 10)).Verifiable();

            return (ViewResult)_controller.TopTabs();
        }

        private void SetAdmin()
        {
            SetCurrentUser(new Mock<IUser>(), Roles.Administrator);
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