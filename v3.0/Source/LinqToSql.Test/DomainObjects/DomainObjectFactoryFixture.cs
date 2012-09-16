using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using Kigg.LinqToSql.DomainObjects;
    using DomainObjects;

    public class DomainObjectFactoryFixture
    {
        private readonly DomainObjectFactory _factory;

        public DomainObjectFactoryFixture()
        {
            _factory = new DomainObjectFactory();
        }

        [Fact]
        public void CreateUser_Should_Return_New_User()
        {
            var user = CreateUser();

            Assert.NotNull(user);
        }

        [Fact]
        public void CreateKnownSource_Should_Return_New_KnownSource()
        {
            var knownSource = _factory.CreateKnownSource("http://asp.net");

            Assert.NotNull(knownSource);
        }

        [Fact]
        public void CreateCategory_Should_Return_New_Category()
        {
            var category = CreateCategory();

            Assert.NotNull(category);
        }

        [Fact]
        public void CreateTag_Should_Return_New_Tag()
        {
            var tag = _factory.CreateTag("Dummy Tag");

            Assert.NotNull(tag);
        }

        [Fact]
        public void CreateStory_Should_Return_New_Story()
        {
            var story = CreateStory();

            Assert.NotNull(story);
        }

        [Fact]
        public void CreateStoryView_Should_Return_New_StoryView()
        {
            var view = _factory.CreateStoryView(CreateStory(), SystemTime.Now(), "192.168.0.1");

            Assert.NotNull(view);
        }

        [Fact]
        public void CreateStoryVote_Should_Return_New_StoryVote()
        {
            var vote = _factory.CreateStoryVote(CreateStory(), SystemTime.Now(), CreateUser(), "192.168.0.1");

            Assert.NotNull(vote);
        }

        [Fact]
        public void CreateMarkAsSpam_Should_Return_New_StoryMarkAsSpam()
        {
            var spamStory = _factory.CreateMarkAsSpam(CreateStory(), SystemTime.Now(), CreateUser(), "192.168.0.1");

            Assert.NotNull(spamStory);
        }

        [Fact]
        public void CreateComment_Should_Return_New_StoryComment()
        {
            var comment = _factory.CreateComment(CreateStory(), "dummy content", SystemTime.Now(), CreateUser(), "192.168.0.1");

            Assert.NotNull(comment);
        }

        [Fact]
        public void CreateCommentSubscribtion_Should_Return_New_CommentSubscribtion()
        {
            var subscribtion = _factory.CreateCommentSubscribtion(CreateStory(), CreateUser());

            Assert.NotNull(subscribtion);
        }

        private ICategory CreateCategory()
        {
            return _factory.CreateCategory("Dummy Category");
        }

        private IUser CreateUser()
        {
            return _factory.CreateUser("dummyUser", "dummy@users.com", "dummyPassword");
        }

        private IStory CreateStory()
        {
            return _factory.CreateStory(CreateCategory(), CreateUser(), "192.168.0.1", "Dummy Story", "Dummy Description", "http://astory.com");
        }
    }
}