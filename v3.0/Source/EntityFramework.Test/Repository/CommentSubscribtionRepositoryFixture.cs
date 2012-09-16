using System;

using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class CommentSubscribtionRepositoryFixture : EfBaseFixture
    {
        private readonly CommentSubscribtionRepository _commentSubscribtionRepository;

        public CommentSubscribtionRepositoryFixture()
        {
            _commentSubscribtionRepository = new CommentSubscribtionRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new CommentSubscribtionRepository(databaseFactory.Object));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var storyId = Guid.NewGuid();
            var story = new Story {Id = storyId};
            Users.Add(new User { UserName = "user1" });
            Users.Add(new User { UserName = "user2" });
            Users.Add(new User { UserName = "user3" });

            foreach(var user in Users)
            {
                user.CommentSubscriptionsInternal.Add(story);
            }

            Assert.Equal(3, _commentSubscribtionRepository.CountByStory(storyId));
        }

        [Fact]
        public void FindById_Should_Return_Correct_Subscribtion()
        {
            Stories.Add(new Story { Id = Guid.NewGuid() });
            Users.Add(new User { Id = Guid.NewGuid() });
            
            Stories[0].SubscribeComment(Users[0]);

            var storyId = Stories[0].Id;
            var userId = Users[0].Id;
            
            Assert.NotNull(_commentSubscribtionRepository.FindById(storyId, userId));
        }
    }
}