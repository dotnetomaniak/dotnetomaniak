using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogFixture : IDisposable
    {
        private const string Message = "A dummy message";

        private readonly Mock<ILogger> _logger;
        private readonly Mock<IExceptionPolicy> _exceptionPolicy;
        private readonly Weblog _Log;

        public WeblogFixture()
        {
            _logger = new Mock<ILogger>();
            _exceptionPolicy = new Mock<IExceptionPolicy>();
            _Log = new Weblog("All", _logger.Object, "Unhandled Exceptions", _exceptionPolicy.Object, 4);
        }

        public void Dispose()
        {
            _logger.Verify();
        }

        [Fact]
        public void Info_Should_Use_Logger()
        {
            _logger.Expect(l => l.Write(It.IsAny<LogEntry>())).Verifiable();

            _Log.Info(Message);
        }

        [Fact]
        public void Warning_Should_Use_Logger()
        {
            _logger.Expect(l => l.Write(It.IsAny<LogEntry>())).Verifiable();

            _Log.Warning(Message);
        }

        [Fact]
        public void Error_Should_Use_Logger()
        {
            _logger.Expect(l => l.Write(It.IsAny<LogEntry>())).Verifiable();

            _Log.Error(Message);
        }

        [Fact]
        public void Exception_Should_Throw_Exception_When_Exception_Throwing_Is_Turrned_On()
        {
            Exception exceptionToHandle = new Exception();
            Exception exceptionToThrow;

            _exceptionPolicy.Expect(ep => ep.HandleException(exceptionToHandle, It.IsAny<string>(), out exceptionToThrow)).Returns(true);

            Assert.Throws<Exception>(() => _Log.Exception(exceptionToHandle));
        }

        [Fact]
        public void Exception_Should_Not_Throw_Exception_When_Exception_Throwing_Is_Turrned_Off()
        {
            Exception exceptionToHandle = new Exception();
            Exception exceptionToThrow;

            _exceptionPolicy.Expect(ep => ep.HandleException(exceptionToHandle, It.IsAny<string>(), out exceptionToThrow)).Returns(false);

            Assert.DoesNotThrow(() => _Log.Exception(exceptionToHandle));
        }
    }
}