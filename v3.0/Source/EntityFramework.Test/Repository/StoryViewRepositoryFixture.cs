using System;

using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;

    public class StoryViewRepositoryFixture : EfBaseFixture
    {
        private readonly StoryViewRepository _storyViewRepository;

        public StoryViewRepositoryFixture()
        {
            _storyViewRepository = new StoryViewRepository(database.Object);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new StoryViewRepository(databaseFactory.Object));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var storyId = Guid.NewGuid();
            var story = new Story {Id = storyId};

            Views.Add(new StoryView { Story = story });
            Views.Add(new StoryView { Story = story });
            Views.Add(new StoryView { Story = story });

            Assert.Equal(3, _storyViewRepository.CountByStory(storyId));
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_Views()
        {
            var now = SystemTime.Now().AddDays(-1);
            var storyId = Guid.NewGuid();

            var story = new Story { Id = storyId };

            Views.Add(new StoryView { Story = story, Timestamp = now.AddHours(1) });
            Views.Add(new StoryView { Story = story, Timestamp = now.AddHours(2) });
            Views.Add(new StoryView { Story = story, Timestamp = now.AddHours(3) });

            var result = _storyViewRepository.FindAfter(storyId, now);

            Assert.Equal(3, result.Count);
        }
    }
}