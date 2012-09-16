using System;

using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;

    public class VoteRepositoryFixture : EfBaseFixture
    {
        private readonly VoteRepository _voteRepository;

        public VoteRepositoryFixture()
        {
            _voteRepository = new VoteRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new VoteRepository(databaseFactory.Object));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var storyId = Guid.NewGuid();

            Votes.Add(new StoryVote { StoryId = storyId });
            Votes.Add(new StoryVote { StoryId = storyId });
            Votes.Add(new StoryVote { StoryId = storyId });

            Assert.Equal(3, _voteRepository.CountByStory(storyId));
        }

        [Fact]
        public void FindById_Should_Return_Correct_Vote()
        {
            Votes.Add(new StoryVote { StoryId = Guid.NewGuid(), UserId = Guid.NewGuid() });

            var storyId = Votes[0].StoryId;
            var userId = Votes[0].UserId;

            Assert.NotNull(_voteRepository.FindById(storyId, userId));
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_Votes()
        {
            var now = SystemTime.Now().AddDays(-1);
            var storyId = Guid.NewGuid();

            Votes.Add(new StoryVote { StoryId = storyId, Timestamp = now.AddHours(1) });
            Votes.Add(new StoryVote { StoryId = storyId, Timestamp = now.AddHours(2) });
            Votes.Add(new StoryVote { StoryId = storyId, Timestamp = now.AddHours(3) });

            var result = _voteRepository.FindAfter(storyId, now);

            Assert.Equal(3, result.Count);
        }
    }
}