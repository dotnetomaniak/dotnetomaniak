using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogLoggingExceptionHandlerFixture : IDisposable
    {
        private readonly Mock<ILogger> _logger;
        private readonly WeblogLoggingExceptionHandler _handler;

        public WeblogLoggingExceptionHandlerFixture()
        {
            _logger = new Mock<ILogger>();
            _handler = new WeblogLoggingExceptionHandler("foo", _logger.Object);
        }

        public void Dispose()
        {
            _logger.Verify();
        }

        [Fact]
        public void HandleException_Should_Use_Logger()
        {
            _logger.Expect(l => l.Write(It.IsAny<WeblogEntry>())).Verifiable();

            _handler.HandleException(new Exception("A dummy exception"), Guid.NewGuid());
        }
    }
}