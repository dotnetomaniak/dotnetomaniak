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
        public void Get_Should_Log_Info_When_Content_Is_Retrieved()
        {
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();
            _service.Expect(s => s.Get(It.IsAny<string>())).Returns(new StoryContent("A Story Title", "A Story Description", "http://www.test.com/trackback"));

            _loggingService.Get("http://www.test.com");
        }

        [Fact]
        public void Get_Should_Log_Warn_When_Content_Is_Not_Retrieved()
        {
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            _service.Expect(s => s.Get(It.IsAny<string>())).Returns(StoryContent.Empty);

            _loggingService.Get("http://www.test.com");
        }

        [Fact]
        public void Get_Should_Use_InnerService()
        {
            _service.Expect(s => s.Get(It.IsAny<string>())).Returns(new StoryContent("A Story Title", "A Story Description", "http://www.test.com/trackback")).Verifiable();

            _loggingService.Get("http://www.test.com");
        }

        [Fact]
        public void Ping_Should_Log_Info()
        {
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();
            _service.Expect(s => s.Ping(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            _loggingService.Ping("http://www.test.com/trackback", "A Story", "http://www.story.com", "Story Excerpt", "Kigg.com");
        }

        [Fact]
        public void Ping_Should_Use_InnerService()
        {
            _service.Expect(s => s.Ping(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _loggingService.Ping("http://www.test.com/trackback", "A Story", "http://www.story.com", "Story Excerpt", "Kigg.com");
        }

        [Fact]
        public void ShortUrl_Should_Log_Info_When_Url_Is_Shortened()
        {
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();
            _service.Expect(s => s.ShortUrl(It.IsAny<string>())).Returns("http://tinyurl.com/xyz");

            _loggingService.ShortUrl("http://www.test.com");
        }

        [Fact]
        public void ShortUrl_Should_Log_Warn_When_Url_Is_Not_Shortened()
        {
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            _service.Expect(s => s.ShortUrl(It.IsAny<string>())).Returns((string)null);

            _loggingService.ShortUrl("http://www.test.com");
        }

        [Fact]
        public void ShortUrl_Should_Use_InnerService()
        {
            _service.Expect(s => s.ShortUrl(It.IsAny<string>())).Returns("http://tinyurl.com/xyz").Verifiable();

            _loggingService.ShortUrl("http://test.com/foobar");
        }
    }
}