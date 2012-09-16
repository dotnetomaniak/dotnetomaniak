using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class DecoratedContentServiceFixture : IDisposable
    {
        private readonly Mock<IContentService> _innerContentService;
        private readonly DecoratedContentService _contentService;

        public DecoratedContentServiceFixture()
        {
            _innerContentService = new Mock<IContentService>();
            _contentService = new DecoratedContentServiceTestDouble(_innerContentService.Object);
        }

        public void Dispose()
        {
            _innerContentService.VerifyAll();
        }

        [Fact]
        public void Get_Should_Use_InnerService()
        {
            _innerContentService.Setup(s => s.Get(It.IsAny<string>()));

            _contentService.Get("http://aurl.com");
        }

        [Fact]
        public void ShortUrl_Should_Use_InnerService()
        {
            _innerContentService.Setup(s => s.ShortUrl(It.IsAny<string>()));

            _contentService.ShortUrl("http://aurl.com");
        }
    }

    public class DecoratedContentServiceTestDouble : DecoratedContentService
    {
        public DecoratedContentServiceTestDouble(IContentService innerService) : base(innerService)
        {
        }
    }
}