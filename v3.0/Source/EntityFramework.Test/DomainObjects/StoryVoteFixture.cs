using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.DomainObjects;

    public class StoryVoteFixture
    {
        private readonly StoryVote _vote;

        public StoryVoteFixture()
        {
            _vote = new StoryVote();
        }

        [Fact]
        public void ForStory_Should_Return_The_Story()
        {
            _vote.Story = new Story();

            Assert.Same(_vote.ForStory, _vote.Story);
        }

        [Fact]
        public void ByUser_Should_Return_The_User()
        {
            _vote.User = new User();

            Assert.Same(_vote.ByUser, _vote.User);
        }

        [Fact]
        public void FromIpAddress_Should_Return_The_IpAddress()
        {
            _vote.IpAddress = "192.168.0.1";

            Assert.Equal(_vote.IpAddress, _vote.FromIPAddress);
        }

        [Fact]
        public void PromotedAt_Should_Return_The_Timestamp()
        {
            _vote.Timestamp = SystemTime.Now();

            Assert.Equal(_vote.Timestamp, _vote.PromotedAt);
        }
    }
}