using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;

    public class CommentSubscribtionFixture
    {
        private readonly CommentSubscribtion _subscribtion;

        public CommentSubscribtionFixture()
        {
            _subscribtion = new CommentSubscribtion();
        }

        [Fact]
        public void ForStory_Should_Return_The_Story()
        {
            _subscribtion.Story = new Story();

            Assert.Same(_subscribtion.ForStory, _subscribtion.Story);
        }

        [Fact]
        public void ByUser_Should_Return_The_Story()
        {
            _subscribtion.User = new User();

            Assert.Same(_subscribtion.ByUser, _subscribtion.User);
        }
    }
}