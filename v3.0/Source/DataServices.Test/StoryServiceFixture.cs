using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.DataServices.Test
{
    using Kigg.Test.Infrastructure;
    
    using Repository;
    using DomainObjects;
    using ServiceImpl;
    //using DataContracts;

    public class StoryServiceFixture : BaseFixture
    {
        private readonly StoryDataService _storySvc;
        private readonly Mock<IStoryRepository> _storyRepository;
        
        public StoryServiceFixture()
        {
            _storyRepository = new Mock<IStoryRepository>();
            _storySvc = new StoryDataService(_storyRepository.Object);
        }
        
        [Fact]
        public void GetPublishedStories_Should_Call_StoryRepository_FindPublished()
        {
            _storySvc.GetPublishedStories(0, 10);   
            _storyRepository.Verify(r=>r.FindPublished(0,10), Times.AtMostOnce());
        }

        [Fact]
        public void GetPublishedStories_Should_Return_Correct_Result()
        {
            IList<IStory> stories = GetStories(10);
            
            var pagedResults = new PagedResult<IStory>(stories, 25);

            _storyRepository.Setup(r => r.FindPublished(0, 10)).Returns(pagedResults);

            var publishedStories = _storySvc.GetPublishedStories(0, 10);
            Assert.NotNull(publishedStories);
            Assert.NotEmpty(publishedStories);
        }

        [Fact]
        public void GetUpcomingStories_Should_Call_StoryRepository_FindUpcoming()
        {
            _storySvc.GetUpcomingStories(0, 10);
            _storyRepository.Verify(r => r.FindUpcoming(0, 10), Times.AtMostOnce());
        }

        [Fact]
        public void GetUpcomingStories_Should_Return_Correct_Result()
        {
            IList<IStory> stories = GetStories(10);

            var pagedResults = new PagedResult<IStory>(stories, 25);

            _storyRepository.Setup(r => r.FindUpcoming(0, 10)).Returns(pagedResults);

            var upcomingStories = _storySvc.GetUpcomingStories(0, 10);
            Assert.NotNull(upcomingStories);
            Assert.NotEmpty(upcomingStories);
        }

        [Fact]
        public void GetStoriesByCategory_Should_Call_StoryRepository_FindPublishedByCategory()
        {
            _storySvc.GetStoriesByTag("General", 0, 10);
            _storyRepository.Verify(r => r.FindPublishedByCategory("General", 0, 10), Times.AtMostOnce());
        }
        
        [Fact]
        public void GetStoriesByCategory_Should_Return_Correct_Result()
        {
            IList<IStory> stories = GetStories(10);

            var pagedResults = new PagedResult<IStory>(stories, 25);

            _storyRepository.Setup(r => r.FindPublishedByCategory("General", 0, 10)).Returns(pagedResults);

            var categoryStories = _storySvc.GetStoriesByCategory("General", 0, 10);
            Assert.NotNull(categoryStories);
            Assert.NotEmpty(categoryStories);
        }

        [Fact]
        public void GetStoriesByTag_Should_Call_StoryRepository_FindByTag()
        {
            _storySvc.GetStoriesByTag("Tag", 0, 10);
            _storyRepository.Verify(r => r.FindByTag("Tag", 0, 10), Times.AtMostOnce());
        }

        [Fact]
        public void GetStoriesByTag_Should_Return_Correct_Result()
        {
            IList<IStory> stories = GetStories(10);

            var pagedResults = new PagedResult<IStory>(stories, 25);

            _storyRepository.Setup(r => r.FindByTag("Tag", 0, 10)).Returns(pagedResults);

            var taggedStories = _storySvc.GetStoriesByTag("Tag", 0, 10);
            Assert.NotNull(taggedStories);
            Assert.NotEmpty(taggedStories);
        }

        [Fact]
        public void GetStoriesPostedByUser_Should_Call_StoryRepository_FindPostedByUser()
        {
            _storySvc.GetStoriesPostedByUser("FakeUserName", 0, 10);
            _storyRepository.Verify(r => r.FindPostedByUser("FakeUserName", 0, 10), Times.AtMostOnce());
        }

        [Fact]
        public void GetStoriesPostedByUser_Should_Return_Correct_Result()
        {
            IList<IStory> stories = GetStories(10);

            var pagedResults = new PagedResult<IStory>(stories, 25);

            _storyRepository.Setup(r => r.FindPostedByUser("FakeUserName", 0, 10)).Returns(pagedResults);

            var userStories = _storySvc.GetStoriesPostedByUser("FakeUserName", 0, 10);
            Assert.NotNull(userStories);
            Assert.NotEmpty(userStories);
        }

        [Fact]
        public void GetStoriesShoutedByUser_Should_Call_StoryRepository_FindPostedByUser()
        {
            _storySvc.GetStoriesShoutedByUser("FakeUserName", 0, 10);
            _storyRepository.Verify(r => r.FindPromotedByUser("FakeUserName", 0, 10), Times.AtMostOnce());
        }

        [Fact]
        public void GetStoriesShoutedByUser_Should_Return_Correct_Result()
        {
            IList<IStory> stories = GetStories(10);

            var pagedResults = new PagedResult<IStory>(stories, 25);

            _storyRepository.Setup(r => r.FindPromotedByUser("FakeUserName", 0, 10)).Returns(pagedResults);

            var userShoutedStories = _storySvc.GetStoriesShoutedByUser("FakeUserName", 0, 10);
            Assert.NotNull(userShoutedStories);
            Assert.NotEmpty(userShoutedStories);
        }

        private static IList<IStory> GetStories(int count)
        {
            IList<IStory> stories = new List<IStory>(count);
            for (var i = 0; i < count; i++)
            {
                var url = String.Format("http://somedummydomain.net/{0}/story.aspx", i);
                var title = String.Format("Title {0}", i);
                var description = String.Format("Description {0}", i);
                var uniqueName = String.Format("uniquename{0}", i);
                IStory story = CreateStory(url, title, description, uniqueName);
                stories.Add(story);
            }
            return stories;
        }
        private static IStory CreateStory(string url, string title, string description, string uniqueName)
        {
            var rnd = new Random();
            var story = new Mock<IStory>();
            var user = new Mock<IUser>();
            var category = new Mock<ICategory>();

            user.SetupGet(u => u.UserName).Returns("FakeUserName");
            category.SetupGet(c => c.Name).Returns("General");

            story.SetupGet(s => s.PostedBy).Returns(user.Object);
            story.SetupGet(s => s.BelongsTo).Returns(category.Object);
            story.SetupGet(s => s.Title).Returns(title);
            story.SetupGet(s => s.UniqueName).Returns(uniqueName);
            story.SetupGet(s => s.CommentCount).Returns(rnd.Next(1, 10));
            story.SetupGet(s => s.VoteCount).Returns(rnd.Next(1, 20));
            story.SetupGet(s => s.TextDescription).Returns(description);
            story.SetupGet(s => s.Url).Returns(url);
            story.SetupGet(s => s.CreatedAt).Returns(SystemTime.Now().AddDays(rnd.Next(-10, -5)));
            story.SetupGet(s => s.ApprovedAt).Returns(SystemTime.Now().AddDays(rnd.Next(-5, -1)));
            
            return story.Object;
        }
    }
}
