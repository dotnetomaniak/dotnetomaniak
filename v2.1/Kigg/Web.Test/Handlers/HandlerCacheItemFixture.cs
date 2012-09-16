using Xunit;

namespace Kigg.Web.Test
{
    public class HandlerCacheItemFixture
    {
        private const string Content = "This is a dummy content";

        private readonly HandlerCacheItem _cacheItem;

        public HandlerCacheItemFixture()
        {
            _cacheItem = new HandlerCacheItem
                            {
                                Content = Content
                            };
        }

        [Fact]
        public void ETag_Should_Return_The_Hash_Of_Content()
        {
            Assert.Equal(Content.Hash(), _cacheItem.ETag);
        }
    }
}