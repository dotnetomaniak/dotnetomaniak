using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.DomainObjects;

    public class StoryMarkAsSpamFixture
    {
        private readonly StoryMarkAsSpam _markAsSpam;

        public StoryMarkAsSpamFixture()
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
            _markAsSpam.IpAddress = "192.168.0.1";

            Assert.Equal(_markAsSpam.IpAddress, _markAsSpam.FromIPAddress);
        }

        [Fact]
        public void MarkedAt_Should_Return_The_Timestamp()
        {
            _markAsSpam.Timestamp = SystemTime.Now();

            Assert.Equal(_markAsSpam.Timestamp, _markAsSpam.MarkedAt);
        }
    }
}