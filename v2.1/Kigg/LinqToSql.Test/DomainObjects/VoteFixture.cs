using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;

    public class VoteFixture
    {
        private readonly StoryVote _vote;

        public VoteFixture()
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
            _vote.IPAddress = "192.168.0.1";

            Assert.Equal(_vote.IPAddress, _vote.FromIPAddress);
        }

        [Fact]
        public void PromotedAt_Should_Return_The_Timestamp()
        {
            _vote.Timestamp = SystemTime.Now();

            Assert.Equal(_vote.Timestamp, _vote.PromotedAt);
        }
    }
}