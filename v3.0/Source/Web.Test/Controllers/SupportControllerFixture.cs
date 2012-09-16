using System;
using System.Web.Mvc;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class SupportControllerFixture : BaseFixture
    {
        private const string UserName = "dummyUser";

        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IStoryRepository> _storyRepository;

        private readonly HttpContextMock _httpContext;
        private readonly SupportController _controller;

        public SupportControllerFixture()
        {
            _emailSender = new Mock<IEmailSender>();
            _storyRepository = new Mock<IStoryRepository>();
            _userRepository = new Mock<IUserRepository>();

            _controller = new SupportController(_storyRepository.Object, _emailSender.Object)
                              {
                                  Settings = settings.Object,
                                  UserRepository = _userRepository.Object
                              };

            _httpContext = _controller.MockHttpContext("/Kigg", null, null);
            _httpContext.HttpRequest.SetupGet(r => r.UserHostAddress).Returns("192.168.0.1");
        }

        [Fact]
        public void Faq_Should_Render_Default_View()
        {
            var result = (ViewResult) _controller.Faq();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void Contact_Should_Render_Default_View()
        {
            var result = (ViewResult) _controller.Contact();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void Contact_Should_Send_Email()
        {
            JsonViewData viewData = (JsonViewData) ((JsonResult)_controller.Contact("xxx@xxx.com", new string('x', 4), new string('x', 16))).Data;

            Assert.True(viewData.isSuccessful);
        }

        [Fact]
        public void Contact_Should_Use_EmailSender_To_Send_Email()
        {
            _emailSender.Setup(s => s.NotifyFeedback(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _controller.Contact("xxx@xxx.com", new string('x', 4), new string('x', 16));

            _emailSender.Verify();
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Blank_Email_Is_Passed()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact(string.Empty, new string('x', 4), new string('x', 16))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Email cannot be blank.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Invalid_Email_Is_Passed()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx", new string('x', 4), new string('x', 16))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Invalid email format.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Blank_Name_Is_Passed()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx@xxx.com", string.Empty, new string('x', 16))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Name cannot be blank.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Name_Is_Less_Than_Four_Character()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx@xxx.com", "xxx", new string('x', 16))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Name cannot be less than 4 character.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Blank_Message_Is_Passed()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx@xxx.com", new string('x', 4), string.Empty)).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Message cannot be blank.", viewData.errorMessage);
        }

        [Fact]
        public void Contact_Should_Return_Error_When_Message_Is_Less_Than_Sixteen_Character()
        {
            JsonViewData viewData = (JsonViewData)((JsonResult)_controller.Contact("xxx@xxx.com", "xxxx", new string('x', 15))).Data;

            Assert.False(viewData.isSuccessful);
            Assert.Equal("Message cannot be less than 16 character.", viewData.errorMessage);
        }

        [Fact]
        public void About_Should_Render_Default_View()
        {
            var result = (ViewResult) _controller.About();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void ControlPanel_Should_Render_Default_View()
        {
            var result = ControlPanel(false);

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void ControlPanel_Should_Use_StoryRepository_When_User_Has_Permission()
        {
            ControlPanel(true);

            _storyRepository.Verify();
        }

        [Fact]
        public void ControlPanel_Should_Return_Error_Messsage_When_User_Is_Not_Permitted()
        {
            var viewData = (ControlPanelViewData) ControlPanel(false).ViewData.Model;

            Assert.Equal("You do not have the privilege to view it.", viewData.ErrorMessage);
        }

        private ViewResult ControlPanel(bool permitted)
        {
            SetCurrentUser(new Mock<IUser>(), permitted ? Roles.Moderator : Roles.User);

            _storyRepository.Setup(r => r.CountByUnapproved()).Returns(10).Verifiable();
            _storyRepository.Setup(r => r.CountByNew()).Returns(10).Verifiable();
            _storyRepository.Setup(r => r.CountByPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(10).Verifiable();

            return (ViewResult) _controller.ControlPanel();
        }

        private void SetCurrentUser(Mock<IUser> user, Roles role)
        {
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