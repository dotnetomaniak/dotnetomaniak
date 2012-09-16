using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using Kigg.LinqToSql.DomainObjects;

    public class MarkAsSpamFixture
    {
        private readonly StoryMarkAsSpam _markAsSpam;

        public MarkAsSpamFixture()
        {
            _markAsSpam = new StoryMarkAsSpam();
        }

        [Fact]
        public void ForStory_Should_Return_The_Story()
        {
            _markAsSpam.Story = new Story();

            Assert.Same(_markAsSpam.ForStory, _markAsSpam.Story);
        }

        [Fact]
        public void ByUser_Should_Return_The_User()
        {
            _markAsSpam.User = new User();

            Assert.Same(_markAsSpam.ByUser, _markAsSpam.User);
        }

        [Fact]
        public void FromIPAddress_Should_Return_The_IPAddress()
        {
            _markAsSpam.IPAddress = "192.168.0.1";

            Assert.Equal(_markAsSpam.IPAddress, _markAsSpam.FromIPAddress);
        }

        [Fact]
        public void MarkedAt_Should_Return_The_Timestamp()
        {
            _markAsSpam.Timestamp = SystemTime.Now();

            Assert.Equal(_markAsSpam.Timestamp, _markAsSpam.MarkedAt);
        }
    }
}