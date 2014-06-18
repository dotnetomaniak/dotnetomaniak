using System;
using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Kigg.Test.Infrastructure;

    public class CachingContentServiceFixture : BaseFixture
    {
        private readonly Mock<IContentService> _service;
        private readonly CachingContentService _cachingService;

        public CachingContentServiceFixture()
        {
            _service = new Mock<IContentService>();
            _cachingService = new CachingContentService(_service.Object, 10, 5);
        }

        [Fact]
        public void Get_Should_Use_InnerService()
        {
            Get();

            _service.Verify();
        }

        [Fact]
        public void Get_Should_Cache_Content_When_Url_Does_Not_Exist_In_Cache()
        {
            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<StoryContent>(), It.IsAny<DateTime>())).Verifiable();

            Get();

            cache.Verify();
        }

        [Fact]
        public void ShortUrl_Should_Use_InnerService()
        {
            ShortUrl();

            _service.Verify();
        }

        [Fact]
        public void ShortUrl_Should_Cache_When_Url_Does_Not_Exist_In_Cache()
        {
            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Verifiable();

            ShortUrl();

            cache.Verify();
        }

        private void Get()
        {
            _service.Setup(s => s.Get(It.IsAny<string>())).Returns(new StoryContent("A Story", "A Description", null)).Verifiable();
            _cachingService.Get("http://www.test.com");
        }

        private void ShortUrl()
        {
            _service.Setup(s => s.ShortUrl(It.IsAny<string>())).Returns("http://tinyurl.com/abc").Verifiable();

            _cachingService.ShortUrl("http://www.test.com/foobar");
        }
    }
}