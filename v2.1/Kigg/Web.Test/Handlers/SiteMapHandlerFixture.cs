using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class SiteMapHandlerFixture : BaseFixture
    {
        private readonly HttpContextMock _httpContext;
        private readonly SiteMapHandler _handler;

        public SiteMapHandlerFixture()
        {
            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();

            var userRepository = new Mock<IUserRepository>();
            var storyRepository = new Mock<IStoryRepository>();
            var categoryRepository = new Mock<ICategoryRepository>();
            var tagRepository = new Mock<ITagRepository>();

            List<IUser> topMovers = new List<IUser>();

            for (int i = 1; i <= settings.Object.TopUsers; i++)
            {
                var user = new Mock<IUser>();

                user.ExpectGet(u => u.Id).Returns(Guid.NewGuid());
                user.ExpectGet(u => u.UserName).Returns("Top Mover {0}".FormatWith(i));

                topMovers.Add(user.Object);
            }

            userRepository.Expect(r => r.FindTop(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IUser>(topMovers, settings.Object.TopUsers));

            Func<string, Mock<IStory>> createStory = name =>
                                                     {
                                                         var category = new Mock<ICategory>();
                                                         category.ExpectGet(c => c.Name).Returns("Dummy");

                                                         var tag = new Mock<ITag>();
                                                         tag.ExpectGet(t => t.Name).Returns("Dummy");

                                                         var story = new Mock<IStory>();
                                                         story.ExpectGet(s => s.BelongsTo).Returns(category.Object);
                                                         story.ExpectGet(s => s.UniqueName).Returns(name);
                                                         story.ExpectGet(s => s.Tags).Returns(new List<ITag>{ tag.Object });
                                                         story.ExpectGet(s => s.TagCount).Returns(1);

                                                         return story;
                                                     };

            List<IStory> publishedStories = new List<IStory>();

            for (int i = 1; i <= 50; i++)
            {
                var story = createStory("Published Story {0}".FormatWith(i));

                story.ExpectGet(s => s.PublishedAt).Returns(Constants.ProductionDate);

                publishedStories.Add(story.Object);
            }

            List<IStory> upcomingStories = new List<IStory>();

            for (int i = 1; i <= 50; i++)
            {
                var story = createStory("Upcoming Story {0}".FormatWith(i));

                upcomingStories.Add(story.Object);
            }

            storyRepository.Expect(r => r.FindPublished(It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>(publishedStories, 1000));
            storyRepository.Expect(r => r.FindUpcoming(It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IStory>(upcomingStories, 1000));

            storyRepository.Expect(r => r.CountByPublished()).Returns(300);
            storyRepository.Expect(r => r.CountByUpcoming()).Returns(100);

            List<ICategory> categories = new List<ICategory>();

            for(int i = 1; i <= 10; i++)
            {
                var category = new Mock<ICategory>();

                category.ExpectGet(c => c.UniqueName).Returns("Category {0}".FormatWith(i));

                categories.Add(category.Object);
            }

            categoryRepository.Expect(r => r.FindAll()).Returns(new ReadOnlyCollection<ICategory>(categories));
            storyRepository.Expect(r => r.CountByCategory(It.IsAny<Guid>())).Returns(5);

            List<ITag> tags = new List<ITag>();

            for (int i = 1; i <= settings.Object.TopTags; i++)
            {
                var tag = new Mock<ITag>();

                tag.ExpectGet(t => t.UniqueName).Returns("Tag {0}".FormatWith(i));

                tags.Add(tag.Object);
            }

            tagRepository.Expect(r => r.FindByUsage(It.IsAny<int>())).Returns(new ReadOnlyCollection<ITag>(tags));
            storyRepository.Expect(r => r.CountByTag(It.IsAny<Guid>())).Returns(5);

            _httpContext = MvcTestHelper.GetHttpContext("/Kigg", null, null);

            _handler = new SiteMapHandler
                           {
                               Settings = settings.Object,
                               UserRepository = userRepository.Object,
                               CategoryRepository = categoryRepository.Object,
                               TagRepository = tagRepository.Object,
                               StoryRepository = storyRepository.Object,
                               PublishedStoryMaxCount = 5,
                               UpcomingStoryMaxCount = 5,
                               CacheDurationInMinutes = 45,
                               Compress = true,
                               GenerateETag = true
                           };
        }

        public override void Dispose()
        {
            _httpContext.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Write_Correct_Xml_For_Regular_SiteMap()
        {
            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("sitemap.axd");
            _httpContext.HttpResponse.Expect(r => r.Write(It.IsAny<string>())).Verifiable();

            _handler.ProcessRequest(_httpContext.Object);
        }

        [Fact]
        public void ProcessRequest_Should_Cache_Xml_For_Regular_SiteMap()
        {
            string xml;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);
            cache.Expect(c => c.Contains(It.IsAny<string>())).Returns(false);
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<HandlerCacheItem>(), It.IsAny<DateTime>())).Verifiable();

            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("sitemap.axd");

            _handler.ProcessRequest(_httpContext.Object);

            cache.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Log_For_Regular_SiteMap()
        {
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("sitemap.axd");

            _handler.ProcessRequest(_httpContext.Object);

            log.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Write_Correct_Xml_For_Mobile_SiteMap()
        {
            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("mobilesitemap.axd");
            _httpContext.HttpResponse.Expect(r => r.Write(It.IsAny<string>())).Verifiable();

            _handler.ProcessRequest(_httpContext.Object);
        }

        [Fact]
        public void ProcessRequest_Should_Cache_Xml_For_Regular_Mobile_SiteMap()
        {
            string xml;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);
            cache.Expect(c => c.Contains(It.IsAny<string>())).Returns(false);
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<HandlerCacheItem>(), It.IsAny<DateTime>())).Verifiable();

            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("mobilesitemap.axd");

            _handler.ProcessRequest(_httpContext.Object);

            cache.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Log_For_Regular_Mobile_SiteMap()
        {
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("mobilesitemap.axd");

            _handler.ProcessRequest(_httpContext.Object);

            log.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Write_Correct_Xml_For_News_SiteMap()
        {
            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("newssitemap.axd");
            _httpContext.HttpResponse.Expect(r => r.Write(It.IsAny<string>())).Verifiable();

            _handler.ProcessRequest(_httpContext.Object);
        }

        [Fact]
        public void ProcessRequest_Should_Cache_Xml_For_Regular_News_SiteMap()
        {
            string xml;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);
            cache.Expect(c => c.Contains(It.IsAny<string>())).Returns(false);
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<HandlerCacheItem>(), It.IsAny<DateTime>())).Verifiable();

            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("newssitemap.axd");

            _handler.ProcessRequest(_httpContext.Object);

            cache.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Log_For_Regular_News_SiteMap()
        {
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("newssitemap.axd");

            _handler.ProcessRequest(_httpContext.Object);

            log.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Not_Write_Xml_When_Xml_Is_Not_Modified()
        {
            _httpContext.HttpRequest.ExpectGet(r => r.Path).Returns("newssitemap.axd");
            _httpContext.HttpRequest.ExpectGet(r => r.Headers).Returns(new NameValueCollection { { "If-None-Match", "U1VbkHgP/G8l8Diz8p8Mdg==" } });
            _httpContext.HttpResponse.Expect(r => r.Write(It.IsAny<string>())).Never();

            _handler.ProcessRequest(_httpContext.Object);

            _httpContext.Verify();
        }
    }
}