using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.DomainObjects;

    public class CommentSubscribtionFixture
    {
        private readonly CommentSubscribtion _subscribtion;
        private readonly User _user;
        private readonly Story _story;
        public CommentSubscribtionFixture()
        {
            _user = new User();
            _story = new Story();
            _subscribtion = new CommentSubscribtion(_story, _user);
        }

        [Fact]
        public void ForStory_Should_Return_The_Story()
        {
            Assert.Same(_subscribtion.ForStory, _story);
        }

        [Fact]
        public void ByUser_Should_Return_The_Story()
        {
            Assert.Same(_subscribtion.ByUser, _user);
        }
    }
}