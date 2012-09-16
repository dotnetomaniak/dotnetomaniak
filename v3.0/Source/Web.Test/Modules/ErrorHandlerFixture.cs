using System;
using System.Web;
using System.Web.Configuration;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class ErrorHandlerFixture : BaseFixture
    {
        private readonly HttpContextMock _httpContext;
        private readonly ErrorHandler _handler;

        public ErrorHandlerFixture()
        {
            _httpContext = MvcTestHelper.GetHttpContext();
            _httpContext.SetupGet(h => h.IsCustomErrorEnabled).Returns(true);

            _handler = new ErrorHandler();
        }

        [Fact]
        public void OnError_Should_Transfer_To_Error_Page()
        {
            _httpContext.HttpServerUtility.Setup(s => s.GetLastError()).Returns(new Exception());
            _httpContext.HttpServerUtility.Setup(s => s.Transfer(It.IsAny<string>())).Verifiable();

            OnError();

            _httpContext.HttpServerUtility.Verify();
        }

        [Fact]
        public void OnError_Should_Log_Exception()
        {
            _httpContext.HttpServerUtility.Setup(s => s.GetLastError()).Returns(new HttpException());

            OnError();

            log.Verify();
        }

        [Fact]
        public void OnError_Should_Not_Log_For_404()
        {
            _httpContext.HttpServerUtility.Setup(s => s.GetLastError()).Returns(new HttpException(404, "Page not found"));

            Configure();

            //log.Setup(l => l.Exception(It.IsAny<Exception>())).Never();
            _handler.OnError(_httpContext.Object);

            log.Verify(l => l.Exception(It.IsAny<Exception>()), Times.Never());
        }

        private void OnError()
        {
            Configure();

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _handler.OnError(_httpContext.Object);
        }

        private void Configure()
        {
            CustomErrorsSection section = new CustomErrorsSection { DefaultRedirect = "~/ErrorPages/Generic.aspx", Mode = CustomErrorsMode.On };

            section.Errors.Add(new CustomError(403, "~/ErrorPages/AccessDenied.aspx"));
            section.Errors.Add(new CustomError(404, "~/ErrorPages/NotFound.aspx"));
            configurationManager.Setup(m => m.GetSection<CustomErrorsSection>(It.IsAny<string>())).Returns(section);
        }
    }
}