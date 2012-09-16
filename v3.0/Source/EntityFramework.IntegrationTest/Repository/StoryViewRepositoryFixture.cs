using System.Linq;
using System.Data;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;

    public class StoryViewRepositoryFixture : BaseIntegrationFixture
    {
        private readonly StoryViewRepository _storyViewRepository;

        public StoryViewRepositoryFixture()
        {
            _storyViewRepository = new StoryViewRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new StoryViewRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var view = CreateNewStoryView();
                _storyViewRepository.Add(view);
                Assert.Equal(EntityState.Added, view.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, view.EntityState);
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var view = CreateNewStoryView();
                _database.InsertOnSubmit(view);
                _database.SubmitChanges();

                _storyViewRepository.Remove(view);
                Assert.Equal(EntityState.Deleted, view.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, view.EntityState);
            }
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            using(BeginTransaction())
            {
                var view = CreateNewStoryView();
                _database.InsertOnSubmit(view);
                _database.SubmitChanges();

                var storyId = view.Story.Id;
                var count = _database.StoryViewDataSource.Count(v => v.Story.Id == storyId);

                Assert.Equal(count, _storyViewRepository.CountByStory(storyId));    
            }
            
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_Views()
        {
            using (BeginTransaction())
            {
                var view = CreateNewStoryView();
                var storyId = view.Story.Id;
                _database.InsertOnSubmit(view);
                _database.SubmitChanges();

                var date = view.ViewedAt.AddSeconds(-10);
                var count = _database.StoryViewDataSource
                                     .Count(v => v.Story.Id == storyId && v.Timestamp >= date);

                var result = _storyViewRepository.FindAfter(storyId, date);

                Assert.Equal(count, result.Count);
            }
        }
    }
}
