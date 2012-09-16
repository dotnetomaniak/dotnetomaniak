using System.Linq;
using System.Data;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    
    public class CommentRepositoryFixture : BaseIntegrationFixture
    {
        private readonly CommentRepository _commentRepository;

        public CommentRepositoryFixture()
        {
            _commentRepository = new CommentRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new CommentRepository(_dbFactory));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            using (BeginTransaction())
            {
                var story = GenerateStoryGraph();
                var comment = CreateNewComment(story, story.User, "some content");
                _database.InsertOnSubmit(story);
                _database.InsertOnSubmit(comment);
                _database.SubmitChanges();

                var count = story.StoryCommentsInternal.CreateSourceQuery().Count();
                
                Assert.Equal(count, _commentRepository.CountByStory(story.Id));
            }
        }

        [Fact]
        public void FindById_Should_Return_Correct_Comment()
        {
            using (BeginTransaction())
            {
                var story = GenerateStoryGraph();
                var comment = CreateNewComment(story, story.User, "some content");
                _database.InsertOnSubmit(story);
                _database.InsertOnSubmit(comment);
                _database.SubmitChanges();

                var foundComment = _commentRepository.FindById(story.Id, comment.Id);
                Assert.NotNull(foundComment);
                Assert.Equal(comment.Id, foundComment.Id);
            }
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_Comments()
        {
            using (BeginTransaction())
            {
                var story = GenerateStoryGraph();
                var comment = CreateNewComment(story, story.User, "some content");
                _database.InsertOnSubmit(story);
                _database.InsertOnSubmit(comment);
                _database.SubmitChanges();

                var date = comment.CreatedAt.AddSeconds(-10);

                var count = _database.CommentDataSource
                                 .Where(c => c.Story.Id == story.Id && c.CreatedAt >= date)
                                 .Count();

                var result = _commentRepository.FindAfter(story.Id, date);
                Assert.Equal(count, result.Count);
            }
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var story = GenerateStoryGraph();
                _database.InsertOnSubmit(story);
                _database.SubmitChanges();

                var comment = CreateNewComment(story, story.User,"some content");
                _commentRepository.Add(comment);
                Assert.Equal(EntityState.Added, comment.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, comment.EntityState);
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            using (BeginTransaction())
            {
                var story = GenerateStoryGraph();
                var comment = CreateNewComment(story, story.User, "some content");
                _database.InsertOnSubmit(story);
                _database.InsertOnSubmit(comment);
                _database.SubmitChanges();

                _commentRepository.Remove(comment);
                Assert.Equal(EntityState.Deleted, comment.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, comment.EntityState);
            }
        }

    }
}
