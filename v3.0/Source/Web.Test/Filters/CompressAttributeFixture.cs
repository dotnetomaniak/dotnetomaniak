using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

using Moq;
using Xunit;
using Xunit.Extensions;

namespace Kigg.Web.Test
{
    public class CompressAttributeFixture
    {
        private readonly CompressAttribute _attribute;

        public CompressAttributeFixture()
        {
            _attribute = new CompressAttribute();
        }

        [Theory]
        [InlineData("gzip")]
        [InlineData("deflate")]
        [InlineData("")]
        public void OnResultExecuted_Should_Compress_Response(string encoding)
        {
            HttpContextMock httpContext = MvcTestHelper.GetHttpContext();
            ControllerContext controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            ResultExecutedContext executedContext = new ResultExecutedContext(controllerContext, new EmptyResult(), false, null);

            httpContext.HttpRequest.Object.Headers.Add("Accept-Encoding", encoding);

            if (!string.IsNullOrEmpty(encoding))
            {
                httpContext.HttpResponse.SetupGet(r => r.Filter).Returns(new MemoryStream());
                httpContext.HttpResponse.Setup(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
                httpContext.HttpResponse.SetupSet(r => r.Filter = It.IsAny<Stream>());
            }

            _attribute.OnResultExecuted(executedContext);

            if (!string.IsNullOrEmpty(encoding))
            {
                httpContext.Verify();
            }
        }

        [Fact]
        public void OnResultExecuted_Should_Not_Compress_Response_When_Exception_Occurred_And_Exception_Is_Not_Handled()
        {
            HttpContextMock httpContext = MvcTestHelper.GetHttpContext();
            ControllerContext controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            ResultExecutedContext executedContext = new ResultExecutedContext(controllerContext, new EmptyResult(), false, new Exception());

            //httpContext.HttpResponse.Setup(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>())).Never();
            httpContext.HttpResponse.Verify(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());//.Never();
            _attribute.OnResultExecuted(executedContext);

            httpContext.Verify();
        }

        [Fact]
        public void OnResultExecuted_Should_Compress_Response_When_Exception_Occurred_But_Exception_Is_Handled()
        {
            HttpContextMock httpContext = MvcTestHelper.GetHttpContext();
            ControllerContext controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            ResultExecutedContext executedContext = new ResultExecutedContext(controllerContext, new EmptyResult(), false, new Exception())
                                                        {
                                                            ExceptionHandled = true
                                                        };

            httpContext.HttpRequest.Object.Headers.Add("Accept-Encoding", "gzip");
            httpContext.HttpResponse.Setup(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            httpContext.HttpResponse.SetupGet(r => r.Filter).Returns(new MemoryStream());
            httpContext.HttpResponse.Setup(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            httpContext.HttpResponse.SetupSet(r => r.Filter = It.IsAny<Stream>());

            _attribute.OnResultExecuted(executedContext);

            httpContext.Verify();
        }
    }
}