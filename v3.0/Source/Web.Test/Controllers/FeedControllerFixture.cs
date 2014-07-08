using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

using Moq;
using Xunit;
using Xunit.Extensions;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Repository;
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class FeedControllerFixture : BaseFixture
    {
        private const string AppPath = MvcTestHelper.AppPathModifier + "/Kigg";

        private readonly Mock<IConfigurationManager> _configurationManager;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<ITagRepository> _tagRepository;
        private readonly Mock<IStoryRepository> _storyRepository;

        private readonly FeedController _controller;

        public FeedControllerFixture()
        {
            _userRepository = new Mock<IUserRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _tagRepository = new Mock<ITagRepository>();
            _storyRepository = new Mock<IStoryRepository>();

            var cacheProfile = new OutputCacheProfile("FeedCache"){ Duration = 360 };

            var cacheSection = new OutputCacheSettingsSection();
            cacheSection.OutputCacheProfiles.Add(cacheProfile);

            _configurationManager = new Mock<IConfigurationManager>();
            _configurationManager.Setup(c => c.GetSection<OutputCacheSettingsSection>(It.IsAny<string>())).Returns(cacheSection);

            _controller = new FeedController(_configurationManager.Object, _categoryRepository.Object,
                                             _tagRepository.Object, _storyRepository.Object)
                              {
                                  Settings = settings.Object,
                                  UserRepository = _userRepository.Object
                              };

            _controller.MockHttpContext("/Kigg", null, null);

            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();
        }

        [Theory]
        [InlineData("atom")]
        [InlineData("rss")]
        public void Published_Should_Build_Correct_ViewData(string format)
        {
            var result = Published(format);
            var viewData = result.Data;

            Assert.Contains("Ostatnio opublikowane artykuły", viewData.Title);
            Assert.Equal(viewData.Title, viewData.Description);
            Assert.Equal("{0}/".FormatWith(AppPath), viewData.Url);
            Assert.Equal(format, result.Format);
        }

        [Fact]
        public void Published_Should_Log_Exception()
        {
            _storyRepository.Setup(r => r.FindPublished(It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Published("atom", 0, 10);

            log.Verify();
        }

        [Fact]
        public void Published_Should_Use_StoryRepository_To_Build_ViewData()
        {
            Published("atom");

            _storyRepository.Verify();
        }

        [Theory]
        [InlineData("atom")]
        [InlineData("rss")]
        public void Category_Should_Build_Correct_ViewData(string format)
        {
            const string CategoryName = "ASP.NET";

            var result = Category(format, CategoryName);
            var viewData = result.Data;

            Assert.Contains("Ostatnio opublikowane artykuły w {0}".FormatWith(CategoryName), viewData.Title);
            Assert.Equal(viewData.Title, viewData.Description);
            Assert.Equal("{0}/Category/{1}".FormatWith(AppPath, CategoryName), viewData.Url);
            Assert.Equal(format, result.Format);
        }

        [Fact]
        public void Category_Should_Log_Exception()
        {
            _categoryRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Category("atom", "ASP.NET", 0, 10);

            log.Verify();
        }

        [Fact]
        public void Category_Should_Use_CategoryRepository_To_Build_ViewData()
        {
            Category("ASP.NET", "atom");

            _categoryRepository.Verify();
        }

        [Fact]
        public void Category_Should_Use_StoryRepository_To_Build_ViewData()
        {
            Category("ASP.NET", "atom");

            _storyRepository.Verify();
        }

        [Theory]
        [InlineData("atom")]
        [InlineData("rss")]
        public void Upcoming_Should_Build_Correct_ViewData(string format)
        {
            var result = Upcoming(format);
            var viewData = result.Data;

            Assert.Contains("Nadchodzące artykuły", viewData.Title);
            Assert.Equal(viewData.Title, viewData.Description);
            Assert.Equal("{0}/Upcoming".FormatWith(AppPath), viewData.Url);
            Assert.Equal(format, result.Format);
        }

        [Fact]
        public void Upcoming_Should_Log_Exception()
        {
            _storyRepository.Setup(r => r.FindUpcoming(It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Upcoming("atom", 0, 10);

            log.Verify();
        }

        [Fact]
        public void Upcoming_Should_Use_StoryRepository_To_Build_ViewData()
        {
            Upcoming("atom");

            _storyRepository.Verify();
        }

        [Theory]
        [InlineData("atom")]
        [InlineData("rss")]
        public void Tags_Should_Build_Correct_ViewData(string format)
        {
            const string TagName = "ASPNETMVC";

            var result = Tags(format, TagName);
            var viewData = result.Data;

            Assert.Contains("Artykuły z tagiem {0}".FormatWith(TagName), viewData.Title);
            Assert.Equal(viewData.Title, viewData.Description);
            Assert.Equal("{0}/Tags/{1}".FormatWith(AppPath, TagName), viewData.Url);
            Assert.Equal(format, result.Format);
        }

        [Fact]
        public void Tags_Should_Log_Exception()
        {
            _tagRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Tags("atom", "ASPNETMVC", 0, 10);

            log.Verify();
        }

        [Fact]
        public void Tags_Should_Use_TagRepository_To_Build_ViewData()
        {
            Tags("ASPNETMVC", "atom");

            _tagRepository.Verify();
        }

        [Fact]
        public void Tags_Should_Use_StoryRepository_To_Build_ViewData()
        {
            Tags("ASPNETMVC", "atom");

            _storyRepository.Verify();
        }

        [Theory]
        [InlineData("atom")]
        [InlineData("rss")]
        public void PromotedBy_Should_Build_Correct_ViewData(string format)
        {
            string userId = Guid.NewGuid().Shrink();

            var result = PromotedBy(format, userId);
            var viewData = result.Data;

            Assert.Contains("Artykuły wypromowane przez użytkownika {0}".FormatWith(userId), viewData.Title);
            Assert.Equal(viewData.Title, viewData.Description);
            Assert.Equal("{0}/Users/{1}".FormatWith(AppPath, userId), viewData.Url);
            Assert.Equal(format, result.Format);
        }

        [Fact]
        public void PromotedBy_Should_Log_Exception()
        {
            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.PromotedBy("atom", Guid.NewGuid().Shrink(), 0, 10);

            log.Verify();
        }

        [Fact]
        public void PromotedBy_Should_Use_UserRepository_To_Build_ViewData()
        {
            PromotedBy("atom", Guid.NewGuid().Shrink());

            _userRepository.Verify();
        }

        [Fact]
        public void PromotedBy_Should_Use_StoryRepository_To_Build_ViewData()
        {
            PromotedBy("atom", Guid.NewGuid().Shrink());

            _storyRepository.Verify();
        }

        [Theory]
        [InlineData("atom")]
        [InlineData("rss")]
        public void PostedBy_Should_Build_Correct_ViewData(string format)
        {
            string userId = Guid.NewGuid().Shrink();

            var result = PostedBy(format, userId);
            var viewData = result.Data;

            Assert.Contains("Artykuły dodane przez użytkownika {0}".FormatWith(userId), viewData.Title);
            Assert.Equal(viewData.Title, viewData.Description);
            Assert.Equal("{0}/Users/{1}/Posted".FormatWith(AppPath, userId), viewData.Url);
            Assert.Equal(format, result.Format);
        }

        [Fact]
        public void PostedBy_Should_Log_Exception()
        {
            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.PostedBy("atom", Guid.NewGuid().Shrink(), 0, 10);

            log.Verify();
        }

        [Fact]
        public void PostedBy_Should_Use_UserRepository_To_Build_ViewData()
        {
            PostedBy("atom", Guid.NewGuid().Shrink());

            _userRepository.Verify();
        }

        [Fact]
        public void PostedBy_Should_Use_StoryRepository_To_Build_ViewData()
        {
            PostedBy("atom", Guid.NewGuid().Shrink());

            _storyRepository.Verify();
        }

        [Theory]
        [InlineData("atom")]
        [InlineData("rss")]
        public void CommentedBy_Should_Build_Correct_ViewData(string format)
        {
            string userId = Guid.NewGuid().Shrink();

            var result = CommentedBy(format, userId);
            var viewData = result.Data;

            Assert.Contains("Artykuły skomentowane przez użytkownika {0}".FormatWith(userId), viewData.Title);
            Assert.Equal(viewData.Title, viewData.Description);
            Assert.Equal("{0}/Users/{1}/Commented".FormatWith(AppPath, userId), viewData.Url);
            Assert.Equal(format, result.Format);
        }

        [Fact]
        public void CommentedBy_Should_Log_Exception()
        {
            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.CommentedBy("atom", Guid.NewGuid().Shrink(), 0, 10);

            log.Verify();
        }

        [Fact]
        public void CommentedBy_Should_Use_UserRepository_To_Build_ViewData()
        {
            CommentedBy("atom", Guid.NewGuid().Shrink());

            _userRepository.Verify();
        }

        [Fact]
        public void CommentedBy_Should_Use_StoryRepository_To_Build_ViewData()
        {
            CommentedBy("atom", Guid.NewGuid().Shrink());

            _storyRepository.Verify();
        }

        [Theory]
        [InlineData("atom")]
        [InlineData("rss")]
        public void Search_Should_Build_Correct_ViewData(string format)
        {
            const string Query = "foobar";

            var result = Search(format, Query);
            var viewData = result.Data;

            Assert.Contains("Wyniki wyszukiwania dla {0}".FormatWith(Query), viewData.Title);
            Assert.Equal(viewData.Title, viewData.Description);
            Assert.Equal("{0}/Search?q={1}".FormatWith(AppPath, Query), viewData.Url);
            Assert.Equal(format, result.Format);
        }

        [Fact]
        public void Search_Should_Log_Exception()
        {
            _storyRepository.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Throws<Exception>();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _controller.Search("atom", "foobar", 0, 10);

            log.Verify();
        }

        [Fact]
        public void Search_Should_Use_StoryRepository_To_Build_ViewData()
        {
            Search("atom", "foobar");

            _storyRepository.Verify();
        }

        [Theory]
        [InlineData(null, 0, null, 25)]
        [InlineData(-1, 0, null, 25)]
        [InlineData(null, 0, -1, 25)]
        [InlineData(0, 0, 101, 100)]
        public void EnsureInRange_Should_Assign_Correct_Value(int? startIn, int startOut, int? maxIn, int maxOut)
        {
            _controller.EnsureInRange(ref startIn, ref maxIn);

            Assert.Equal(startOut, startIn);
            Assert.Equal(maxOut, maxIn);
        }

        [Fact]
        public void GetCacheDuration_Should_Return_Correct_Duration_When_Spepcified_As_Attribute()
        {
            var controller = new FeedControllerTestDouble(_configurationManager.Object, _categoryRepository.Object, _tagRepository.Object, _storyRepository.Object);

            int duration = controller.GetCacheDuration();

            Assert.Equal(5, duration);
        }

        private FeedResult Published(string format)
        {
            _storyRepository.Setup(r => r.FindPublished(It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>()).Verifiable();

            return (FeedResult)_controller.Published(format, 0, 10);
        }

        private FeedResult Category(string format, string categoryName)
        {
            var category = new Mock<ICategory>();

            category.SetupGet(c => c.Id).Returns(Guid.NewGuid());
            category.SetupGet(c => c.Name).Returns(categoryName);
            category.SetupGet(c => c.UniqueName).Returns(categoryName);

            _categoryRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Returns(category.Object).Verifiable();
            _storyRepository.Setup(r => r.FindPublishedByCategory(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>()).Verifiable();

            return (FeedResult)_controller.Category(format, categoryName, 0, 10);
        }

        private FeedResult Upcoming(string format)
        {
            _storyRepository.Setup(r => r.FindUpcoming(It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>()).Verifiable();

            return (FeedResult)_controller.Upcoming(format, 0, 10);
        }

        private FeedResult Tags(string format, string tagName)
        {
            var tag = new Mock<ITag>();

            tag.SetupGet(t => t.Id).Returns(Guid.NewGuid());
            tag.SetupGet(t => t.Name).Returns(tagName);
            tag.SetupGet(t => t.UniqueName).Returns(tagName);

            _tagRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Returns((ITag) null).Verifiable();
            _tagRepository.Setup(r => r.FindByName(It.IsAny<string>())).Returns(tag.Object).Verifiable();
            _storyRepository.Setup(r => r.FindByTag(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>()).Verifiable();

            return (FeedResult) _controller.Tags(format, tagName, 0, 10);
        }

        private FeedResult PromotedBy(string format, string userId)
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            user.SetupGet(u => u.UserName).Returns(userId);

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(user.Object).Verifiable();
            _storyRepository.Setup(r => r.FindPromotedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>()).Verifiable();

            return (FeedResult)_controller.PromotedBy(format, userId, 0, 10);
        }

        private FeedResult PostedBy(string format, string userId)
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            user.SetupGet(u => u.UserName).Returns(userId);

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(user.Object).Verifiable();
            _storyRepository.Setup(r => r.FindPostedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>()).Verifiable();

            return (FeedResult) _controller.PostedBy(format, userId, 0, 10);
        }

        private FeedResult CommentedBy(string format, string userId)
        {
            var user = new Mock<IUser>();

            user.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            user.SetupGet(u => u.UserName).Returns(userId);

            _userRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(user.Object).Verifiable();
            _storyRepository.Setup(r => r.FindCommentedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>()).Verifiable();

            return (FeedResult)_controller.CommentedBy(format, userId, 0, 10);
        }

        private FeedResult Search(string format, string query)
        {
            _storyRepository.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>()).Verifiable();

            return (FeedResult) _controller.Search(format, query, 0, 10);
        }
    }

    [OutputCache(Duration = 300)]
    public class FeedControllerTestDouble : FeedController
    {
        public FeedControllerTestDouble(IConfigurationManager configurationManager, ICategoryRepository categoryRepository, ITagRepository tagRepository, IStoryRepository storyRepository) : base(configurationManager, categoryRepository, tagRepository, storyRepository)
        {
        }
    }
}