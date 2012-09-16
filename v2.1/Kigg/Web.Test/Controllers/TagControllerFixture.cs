using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class TagControllerFixture : BaseFixture
    {
        private readonly HttpContextMock _httpContext;

        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IUser> _user;
        private readonly Mock<ITagRepository> _tagRepository;

        private readonly TagController _controller;

        public TagControllerFixture()
        {
            _user = new Mock<IUser>();

            _userRepository = new Mock<IUserRepository>();
            _userRepository.Expect(r => r.FindByUserName(It.IsAny<string>())).Returns(_user.Object);

            _tagRepository = new Mock<ITagRepository>();
            _controller = new TagController(_tagRepository.Object)
                              {
                                  Settings = settings.Object,
                                  UserRepository = _userRepository.Object
                              };

            _httpContext = _controller.MockHttpContext();
        }

        [Fact]
        public void Tabs_Should_Render_Default_View()
        {
            var result = Tabs();

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public void Tabs_Should_Use_TagRepository_To_Populate_Popular_Tags()
        {
            Tabs();

            _tagRepository.Verify();
        }

        [Fact]
        public void Tabs_Should_Use_User_To_Populate_User_Tags()
        {
            Tabs();

            _user.Verify();
        }

        [Fact]
        public void SuggestTags_Should_Return_Json_Result()
        {
            var result = (JsonResult) SuggestTags(null);

            Assert.NotNull(result.Data);
            Assert.IsType<string[]>(result.Data);
        }

        [Fact]
        public void SuggestTags_Should_Return_Content_Result_For_Browser()
        {
            var result = (ContentResult) SuggestTags("browser");

            Assert.NotNull(result.Content);
            Assert.Equal("application/json", result.ContentType);
        }

        [Fact]
        public void SuggestTags_Should_Log_Exception()
        {
            log.Expect(l => l.Exception(It.IsAny<Exception>())).Verifiable();
            _tagRepository.Expect(r => r.FindMatching(It.IsAny<string>(), It.IsAny<int>())).Throws<Exception>();

            _controller.SuggestTags("xxx", null, null);

            log.Verify();
        }

        private ViewResult Tabs()
        {
            _user.ExpectGet(u => u.Tags).Returns(new List<ITag>()).Verifiable();
            _tagRepository.Expect(r => r.FindByUsage(It.IsAny<int>())).Returns((ICollection<ITag>)null).Verifiable();

            _httpContext.User.Identity.ExpectGet(i => i.Name).Returns("DummyUser");
            _httpContext.User.Identity.ExpectGet(i => i.IsAuthenticated).Returns(true);

            return (ViewResult)_controller.Tabs();
        }

        private ActionResult SuggestTags(string browser)
        {
            const string TagName = "ASPNETMVC";

            var tag = new Mock<ITag>();

            tag.ExpectGet(t => t.Id).Returns(Guid.NewGuid());
            tag.ExpectGet(t => t.Name).Returns(TagName);

            _tagRepository.Expect(r => r.FindMatching(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<ITag> { tag.Object }).Verifiable();

            return _controller.SuggestTags("ASPNET", null, browser);
        }
    }
}