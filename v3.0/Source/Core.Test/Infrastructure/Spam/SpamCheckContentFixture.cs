using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class SpamCheckContentFixture
    {
        private readonly SpamCheckContent _spamCheckContent;

        public SpamCheckContentFixture()
        {
            _spamCheckContent = new SpamCheckContent();
        }

        [Fact]
        public void Extra_Should_Never_Be_Null()
        {
            Assert.NotNull(_spamCheckContent.Extra);
        }
    }
}