using System;

using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using Kigg.LinqToSql.DomainObjects;
    using Kigg.LinqToSql.Repository;

    public class MarkAsSpamRepositoryFixture : LinqToSqlBaseFixture
    {
        private readonly MarkAsSpamRepository _markAsSpamRepository;

        public MarkAsSpamRepositoryFixture()
        {
            _markAsSpamRepository = new MarkAsSpamRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new MarkAsSpamRepository(databaseFactory.Object));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var storyId = Guid.NewGuid();

            MarkAsSpams.Add(new StoryMarkAsSpam { StoryId = storyId });
            MarkAsSpams.Add(new StoryMarkAsSpam { StoryId = storyId });
            MarkAsSpams.Add(new StoryMarkAsSpam { StoryId = storyId });

            Assert.Equal(3, _markAsSpamRepository.CountByStory(storyId));
        }

        [Fact]
        public void FindById_Should_Return_Correct_MarkAsSpam()
        {
            MarkAsSpams.Add(new StoryMarkAsSpam { StoryId = Guid.NewGuid(), UserId = Guid.NewGuid() });

            var storyId = MarkAsSpams[0].StoryId;
            var userId = MarkAsSpams[0].UserId;

            Assert.NotNull(_markAsSpamRepository.FindById(storyId, userId));
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_MarkAsSpams()
        {
            var now = SystemTime.Now().AddDays(-1);
            var storyId = Guid.NewGuid();

            MarkAsSpams.Add(new StoryMarkAsSpam { StoryId = storyId, Timestamp = now.AddHours(1) });
            MarkAsSpams.Add(new StoryMarkAsSpam { StoryId = storyId, Timestamp = now.AddHours(2) });
            MarkAsSpams.Add(new StoryMarkAsSpam { StoryId = storyId, Timestamp = now.AddHours(3) });

            var result = _markAsSpamRepository.FindAfter(storyId, now);

            Assert.Equal(3, result.Count);
        }
    }
}