using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class LoggingContentServiceFixture : BaseFixture
    {
        private readonly Mock<IContentService> _service;
        private readonly LoggingContentService _loggingService;

        public LoggingContentServiceFixture()
        {
            _service = new Mock<IContentService>();

            _loggingService = new LoggingContentService(_service.Object);
        }

        public override void Dispose()
        {
            log.Verify();
            _service.Verify();
        }

        [Fact]
        public void IsRestricted_Should_Log_Info_When_Url_Is_Not_Restricted()
        {
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();
            _service.Setup(s => s.IsRestricted(It.IsAny<string>())).Returns(false);

            _loggingService.IsRestricted("http://www.test.com");
        }

        [Fact]
        public void IsRestricted_Should_Log_Warn_When_Url_Is_Restricted()
        {
            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            _service.Setup(s => s.IsRestricted(It.IsAny<string>())).Returns(true);

            _loggingService.IsRestricted("http://www.test.com");
        }

        [Fact]
        public void Get_Should_Log_Info_When_Content_Is_Retrieved()
        {
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();
            _service.Setup(s => s.Get(It.IsAny<string>())).Returns(new StoryContent("A Story Title", "A Story Description", "http://www.test.com/trackback"));

            _loggingService.Get("http://www.test.com");
        }

        [Fact]
        public void Get_Should_Log_Warn_When_Content_Is_Not_Retrieved()
        {
            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            _service.Setup(s => s.Get(It.IsAny<string>())).Returns(StoryContent.Empty);

            _loggingService.Get("http://www.test.com");
        }

        [Fact]
        public void Get_Should_Use_InnerService()
        {
            _service.Setup(s => s.Get(It.IsAny<string>())).Returns(new StoryContent("A Story Title", "A Story Description", "http://www.test.com/trackback")).Verifiable();

            _loggingService.Get("http://www.test.com");
        }

        [Fact]
        public void ShortUrl_Should_Log_Info_When_Url_Is_Shortened()
        {
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();
            _service.Setup(s => s.ShortUrl(It.IsAny<string>())).Returns("http://tinyurl.com/xyz");

            _loggingService.ShortUrl("http://www.test.com");
        }

        [Fact]
        public void ShortUrl_Should_Log_Warn_When_Url_Is_Not_Shortened()
        {
            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            _service.Setup(s => s.ShortUrl(It.IsAny<string>())).Returns((string)null);

            _loggingService.ShortUrl("http://www.test.com");
        }

        [Fact]
        public void ShortUrl_Should_Use_InnerService()
        {
            _service.Setup(s => s.ShortUrl(It.IsAny<string>())).Returns("http://tinyurl.com/xyz").Verifiable();

            _loggingService.ShortUrl("http://test.com/foobar");
        }
    }
}