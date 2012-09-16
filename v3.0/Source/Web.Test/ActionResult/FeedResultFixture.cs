using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Kigg.Test.Infrastructure;

    public class FeedResultFixture : BaseFixture
    {
        private readonly FeedViewData feedViewData;
        private readonly HttpContextMock _httpContext;

        public FeedResultFixture()
        {
            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();

            feedViewData = CreateViewData();
            _httpContext = MvcTestHelper.GetHttpContext("/Kigg", null, null);

            _httpContext.HttpRequest.SetupGet(r => r.Url).Returns(new Uri("http://kigg.com"));
        }

        [Fact]
        public void Data_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            FeedResult result = new FeedResult(feedViewData, "rss");

            Assert.Same(feedViewData, result.Data);
        }

        [Fact]
        public void Format_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            FeedResult result = new FeedResult(feedViewData, "atom");

            Assert.Equal("atom", result.Format);
        }

        [Fact]
        public void ExecuteResult_Should_Write_Rss()
        {
            var controllerContext = new ControllerContext(_httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            _httpContext.HttpResponse.SetupSet(r => r.ContentType = "application/rss+xml").Verifiable();
            _httpContext.HttpResponse.Setup(r => r.Write(It.IsAny<string>())).Verifiable();

            FeedResult result = new FeedResult(feedViewData, "rss");
            result.ExecuteResult(controllerContext);

            _httpContext.Verify();
        }

        [Fact]
        public void ExecuteResult_Should_Write_Atom()
        {
            var controllerContext = new ControllerContext(_httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            _httpContext.HttpResponse.SetupSet(r => r.ContentType = "application/atom+xml").Verifiable();
            _httpContext.HttpResponse.Setup(r => r.Write(It.IsAny<string>())).Verifiable();

            FeedResult result = new FeedResult(feedViewData, "atom");
            result.ExecuteResult(controllerContext);

            _httpContext.Verify();
        }

        private static FeedViewData CreateViewData()
        {
            const string SiteTitle = "Kigg.com";
            const string Title = "RSS Title";
            const string Description = "RSS Description";
            const string Url = "/";
            const string Email = "admin@kigg.com";
            const string RootUrl = "http://kigg.com";
            const int CacheDuration = 5;

            var tag1Id = Guid.NewGuid();
            var tag2Id = Guid.NewGuid();

            var story1 = CreateStory("Category1", "User1", "Story1", "Story1Description", "http://story1.com", tag1Id, "Tag1", 2);
            var story2 = CreateStory("Category2", "User2", "Story2", "Story2Description", "http://story2.com", tag2Id, "Tag2", 3);
            var story3 = CreateStory("Category2", "User2", "Story3", "Story3Description", "http://story3.com", tag2Id, "Tag2", 3);
            var story4 = CreateStory("Category1", "User1", "Story4", "Story4Description", "http://story4.com", tag2Id, "Tag1", 2);
            var story5 = CreateStory("Category2", "User2", "Story5", "Story5Description", "http://story5.com", tag2Id, "Tag2", 4);

            var stories = new List<IStory> { story1, story2, story3, story4, story5 };

            return new FeedViewData
                       {
                           SiteTitle = SiteTitle,
                           RootUrl = RootUrl,
                           CacheDurationInMinutes = CacheDuration,
                           Title = Title,
                           Description = Description,
                           Email = Email,
                           Stories = stories,
                           TotalStoryCount = stories.Count,
                           Url = Url
                       };
        }

        private static IStory CreateStory(string categoryName, string postedBy, string storyTitle, string storyDescription, string storyUrl, Guid tagId, string tagName, int commentCount)
        {
            var category = new Mock<ICategory>();

            category.SetupGet(c => c.Id).Returns(Guid.NewGuid());
            category.SetupGet(c => c.Name).Returns(categoryName);
            category.SetupGet(c => c.UniqueName).Returns(categoryName);

            var user = new Mock<IUser>();

            user.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            user.SetupGet(u => u.UserName).Returns(postedBy);

            var tag = new Mock<ITag>();

            tag.SetupGet(t => t.Id).Returns(tagId);
            tag.SetupGet(t => t.Name).Returns(tagName);
            tag.SetupGet(t => t.UniqueName).Returns(tagName);

            var story = new Mock<IStory>();

            story.SetupGet(s => s.Id).Returns(Guid.NewGuid());
            story.SetupGet(s => s.Title).Returns(storyTitle);
            story.SetupGet(s => s.UniqueName).Returns(storyTitle);
            story.SetupGet(s => s.HtmlDescription).Returns(storyDescription);
            story.SetupGet(s => s.Url).Returns(storyUrl);
            story.SetupGet(s => s.CommentCount).Returns(commentCount);
            story.SetupGet(s => s.BelongsTo).Returns(category.Object);
            story.SetupGet(s => s.PostedBy).Returns(user.Object);
            story.SetupGet(s => s.Tags).Returns(new List<ITag> { tag.Object });
            story.SetupGet(s => s.TagCount).Returns(story.Object.Tags.Count);
            story.SetupGet(s => s.PublishedAt).Returns(SystemTime.Now());

            return story.Object;
        }
    }
}