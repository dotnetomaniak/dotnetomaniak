using System;

using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;
    using Repository.LinqToSql;

    public class CommentSubscribtionRepositoryFixture : LinqToSqlBaseFixture
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

            CommentSubscribtions.Add(new CommentSubscribtion { StoryId = storyId });
            CommentSubscribtions.Add(new CommentSubscribtion { StoryId = storyId });
            CommentSubscribtions.Add(new CommentSubscribtion { StoryId = storyId });

            Assert.Equal(3, _commentSubscribtionRepository.CountByStory(storyId));
        }

        [Fact]
        public void FindById_Should_Return_Correct_Subscribtion()
        {
            CommentSubscribtions.Add(new CommentSubscribtion{UserId = Guid.NewGuid(), StoryId = Guid.NewGuid()});

            var userId = CommentSubscribtions[0].UserId;
            var storyId = CommentSubscribtions[0].StoryId;

            Assert.NotNull(_commentSubscribtionRepository.FindById(storyId, userId));
        }
    }
}