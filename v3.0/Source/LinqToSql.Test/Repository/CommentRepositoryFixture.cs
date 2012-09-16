using System;

using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using Kigg.LinqToSql.DomainObjects;
    using Kigg.LinqToSql.Repository;

    public class CommentRepositoryFixture : LinqToSqlBaseFixture
    {
        private readonly CommentRepository _commentRepository;

        public CommentRepositoryFixture()
        {
            _commentRepository = new CommentRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new CommentRepository(databaseFactory.Object));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var storyId = Guid.NewGuid();

            Comments.Add(new StoryComment { StoryId = storyId });
            Comments.Add(new StoryComment { StoryId = storyId });
            Comments.Add(new StoryComment { StoryId = storyId });

            Assert.Equal(3, _commentRepository.CountByStory(storyId));
        }

        [Fact]
        public void FindById_Should_Return_Correct_Comment()
        {
            Comments.Add(new StoryComment{Id = Guid.NewGuid(), StoryId = Guid.NewGuid()});

            var id = Comments[0].Id;
            var storyId = Comments[0].StoryId;

            Assert.NotNull(_commentRepository.FindById(storyId, id));
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_Comments()
        {
            var now = SystemTime.Now().AddDays(-1);
            var storyId = Guid.NewGuid();

            Comments.Add(new StoryComment { StoryId = storyId, CreatedAt = now.AddHours(1) });
            Comments.Add(new StoryComment { StoryId = storyId, CreatedAt = now.AddHours(2) });
            Comments.Add(new StoryComment { StoryId = storyId, CreatedAt = now.AddHours(3) });

            var result = _commentRepository.FindAfter(storyId, now);

            Assert.Equal(3, result.Count);
        }
    }
}