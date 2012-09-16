using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;
    using Repository.LinqToSql;

    public class StoryRepositoryFixture : LinqToSqlBaseFixture
    {
        private readonly StoryRepository _storyRepository;
        private readonly IDomainObjectFactory _factory;

        public StoryRepositoryFixture()
        {
            _factory = new DomainObjectFactory();
            _storyRepository = new StoryRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new StoryRepository(databaseFactory.Object));
        }

        [Fact]
        public void Add_Should_Use_Database()
        {
            database.Expect(d => d.Insert(It.IsAny<Story>())).Verifiable();

            _storyRepository.Add(CreateStory());
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            Stories.Add(CreateStory() as Story);

            Assert.Throws<ArgumentException>(() => _storyRepository.Add(CreateStory()));
        }

        [Fact]
        public void Remove_Should_Use_Database()
        {
            Stories.Add(CreateStory() as Story);

            database.Expect(d => d.Delete(It.IsAny<Story>())).Verifiable();

            _storyRepository.Remove(Stories[0]);
        }

        [Fact]
        public void FindById_Should_Return_Correct_Story()
        {
            Stories.Add(CreateStory() as Story);

            var id = Stories[0].Id;
            var story = _storyRepository.FindById(id);

            Assert.Equal(id, story.Id);
        }

        [Fact]
        public void FindByUniqueName_Should_Return_Correct_Story()
        {
            Stories.Add(CreateStory() as Story);

            var uniqueName = Stories[0].UniqueName;
            var story = _storyRepository.FindByUniqueName(uniqueName);

            Assert.Equal(uniqueName, story.UniqueName);
        }

        [Fact]
        public void FindByUrl_Should_Return_Correct_Story()
        {
            Stories.Add(CreateStory() as Story);

            var url = Stories[0].Url;
            var story = _storyRepository.FindByUrl(url);

            Assert.Equal(url, story.Url);
        }

        [Fact]
        public void FindByPublished_Should_Return_Paged_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            var rnd = new Random();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            Stories.ForEach(s => s.Publish(SystemTime.Now().AddDays(-rnd.Next(1, 5)), rnd.Next(1, 10)));

            var pagedResult = _storyRepository.FindPublished(0, 10);

            Assert.Equal(3, pagedResult.Result.Count);
            Assert.Equal(3, pagedResult.Total);
        }

        [Fact]
        public void FindPublishedByCategory_Should_Return_Paged_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            var rnd = new Random();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            Stories.ForEach(s => s.Publish(SystemTime.Now().AddDays(-rnd.Next(1, 5)), rnd.Next(1, 10)));

            var pagedResult = _storyRepository.FindPublishedByCategory(Stories[0].CategoryId, 0, 10);

            Assert.Equal(1, pagedResult.Result.Count);
            Assert.Equal(1, pagedResult.Total);
        }

        [Fact]
        public void FindUpcoming_Should_Return_Paged_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            var pagedResult = _storyRepository.FindUpcoming(0, 10);

            Assert.Equal(3, pagedResult.Result.Count);
            Assert.Equal(3, pagedResult.Total);
        }

        [Fact]
        public void FindNew_Should_Return_Paged_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            var pagedResult = _storyRepository.FindNew(0, 10);

            Assert.Equal(3, pagedResult.Result.Count);
            Assert.Equal(3, pagedResult.Total);
        }

        [Fact]
        public void FindUnapproved_Should_Return_Paged_Result()
        {
            var story1 = CreateStory(false);
            var story2 = CreateStory(false);
            var story3 = CreateStory(false);

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            var pagedResult = _storyRepository.FindUnapproved(0, 10);

            Assert.Equal(3, pagedResult.Result.Count);
            Assert.Equal(3, pagedResult.Total);
        }

        [Fact]
        public void FindPublishable_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            Stories.ForEach(s => s.CreatedAt = SystemTime.Now().AddDays(-3));

            var result = _storyRepository.FindPublishable(SystemTime.Now().AddDays(-7), SystemTime.Now().AddHours(-4), 0, 10);

            Assert.Equal(3, result.Total);
        }

        [Fact]
        public void FindByTag_Should_Return_Paged_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            var tag = _factory.CreateTag("dummy");

            Stories.ForEach(s => s.AddTag(tag));

            var pagedResult = _storyRepository.FindByTag(tag.Id, 0, 10);

            Assert.Equal(3, pagedResult.Result.Count);
            Assert.Equal(3, pagedResult.Total);
        }

        [Fact]
        public void Search_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            var pagedResult = _storyRepository.Search("Test", 0, 10);

            Assert.Equal(3, pagedResult.Result.Count);
            Assert.Equal(3, pagedResult.Total);
        }

        [Fact]
        public void FindPostedByUser_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            var pagedResult = _storyRepository.FindPostedByUser(Stories[0].UserId, 0, 10);

            Assert.Equal(1, pagedResult.Result.Count);
            Assert.Equal(1, pagedResult.Total);
        }

        [Fact]
        public void FindPromotedByUser_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            var user = _factory.CreateUser("promoter", "promoter@users.com", "xxxxxx");

            Stories.ForEach(s => Votes.Add(new StoryVote{ User = (User) user, Story = s, IPAddress = "192.168.0.1", Timestamp = SystemTime.Now().AddDays(-3) } ));

            var pagedResult = _storyRepository.FindPromotedByUser(user.Id, 0, 10);

            Assert.Equal(3, pagedResult.Result.Count);
            Assert.Equal(3, pagedResult.Total);
        }

        [Fact]
        public void FindCommentedByUser_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            var user = _factory.CreateUser("commenter", "commenter@users.com", "xxxxxx");

            Stories.ForEach(s => Comments.Add(new StoryComment { User = (User) user, Story = s, IPAddress = "192.168.0.1", CreatedAt = SystemTime.Now().AddDays(-3), HtmlBody = "<p>This is a comment</p>", TextBody = "This is a comment." }));

            var pagedResult = _storyRepository.FindCommentedByUser(user.Id, 0, 10);

            Assert.Equal(3, pagedResult.Result.Count);
            Assert.Equal(3, pagedResult.Total);
        }

        [Fact]
        public void CountByPublished_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            var rnd = new Random();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            Stories.ForEach(s => s.Publish(SystemTime.Now().AddDays(-rnd.Next(1, 5)), rnd.Next(1, 10)));

            var result = _storyRepository.CountByPublished();

            Assert.Equal(3, result);
        }

        [Fact]
        public void CountByUpcoming_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            var result = _storyRepository.CountByUpcoming();

            Assert.Equal(3, result);
        }

        [Fact]
        public void CountByCategory_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            var rnd = new Random();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            Stories.ForEach(s => s.Publish(SystemTime.Now().AddDays(-rnd.Next(1, 5)), rnd.Next(1, 10)));

            var result = _storyRepository.CountByCategory(Stories[0].CategoryId);

            Assert.Equal(1, result);
        }

        [Fact]
        public void CountByTag_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            var tag = _factory.CreateTag("dummy");

            Stories.ForEach(s => s.AddTag(tag));

            var result = _storyRepository.CountByTag(tag.Id);

            Assert.Equal(3, result);
        }

        [Fact]
        public void CountByNew_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });

            var result = _storyRepository.CountByNew();

            Assert.Equal(3, result);
        }

        [Fact]
        public void CountByUnapproved_Should_Return_Correct_Result()
        {
            var story1 = CreateStory(false);
            var story2 = CreateStory(false);
            var story3 = CreateStory(false);

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            Stories.ForEach(s => s.CreatedAt = SystemTime.Now().AddDays(-3));

            var result = _storyRepository.CountByUnapproved();

            Assert.Equal(3, result);
        }

        [Fact]
        public void CountByPublishable_Should_Return_Correct_Result()
        {
            var story1 = CreateStory();
            var story2 = CreateStory();
            var story3 = CreateStory();

            Stories.AddRange(new[] { story1 as Story, story2 as Story, story3 as Story });
            Stories.ForEach(s => s.CreatedAt = SystemTime.Now().AddDays(-3));

            var result = _storyRepository.CountByPublishable(SystemTime.Now().AddDays(-7), SystemTime.Now().AddHours(-4));

            Assert.Equal(3, result);
        }

        private IStory CreateStory()
        {
            return CreateStory(true);
        }

        private IStory CreateStory(bool approved)
        {
            var category = _factory.CreateCategory("test");
            var user = _factory.CreateUser("test", "test@users.com", "xxxxxxxx");

            var story = _factory.CreateStory(category, user, "192.168.0.1", "Test Story", "Test Story Description", "http://astory.com");

            if (approved)
            {
                story.Approve(SystemTime.Now());
            }

            return story;
        }
    }
}