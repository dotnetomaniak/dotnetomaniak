using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.DomainObjects;

    public class StoryCommentFixture
    {
        private readonly StoryComment _comment;

        public StoryCommentFixture()
        {
            _comment = new StoryComment();
        }

        [Fact]
        public void ByUser_Should_Return_The_User()
        {
            _comment.User = new User();

            Assert.Same(_comment.ByUser, _comment.User);
        }

        [Fact]
        public void FromIPAddress_Should_Return_The_IpAddress()
        {
            _comment.IpAddress = "192.168.0.1";

            Assert.Equal(_comment.IpAddress, _comment.FromIPAddress);
        }

        [Fact]
        public void ForStory_Should_Return_The_Story()
        {
            _comment.Story = new Story();

            Assert.Same(_comment.ForStory, _comment.Story);
        }

        [Fact]
        public void Mark_As_Offended_Should_Set_IsOffended_To_True()
        {
            _comment.MarkAsOffended();

            Assert.True(_comment.IsOffended);
        }
    }
}