using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

using Xunit;

namespace Kigg.Web.Test
{
    public class BaseHandlerFixture
    {
        private readonly BaseHandler _handler;

        public BaseHandlerFixture()
        {
            _handler = new BaseHandlerTestDouble();
        }

        [Fact]
        public void IsReusable_Should_Be_False()
        {
            Assert.False(_handler.IsReusable);
        }

        [Fact]
        public void HandleIfNotModified_Should_Return_True_If_Content_Is_Not_Modified()
        {
            const string ETag = "abcdef";

            var httpContext = MvcTestHelper.GetHttpContext();

            httpContext.HttpRequest.SetupGet(r => r.Headers).Returns(new NameValueCollection { { "If-None-Match", ETag } });

            Assert.True(BaseHandler.HandleIfNotModified(httpContext.Object, ETag));
        }

        [Fact]
        public void HandleIfNotModified_Should_Return_False_If_Content_Is_Modified()
        {
            const string ETag = "abcdef";

            var httpContext = MvcTestHelper.GetHttpContext();

            httpContext.HttpRequest.SetupGet(r => r.Headers).Returns(new NameValueCollection { { "If-None-Match", "foobar" } });

            Assert.False(BaseHandler.HandleIfNotModified(httpContext.Object, ETag));
        }

        [Fact]
        public void HandleIfNotModified_Should_Return_False_When_ETag_Is_Blank()
        {
            var httpContext = MvcTestHelper.GetHttpContext();

            Assert.False(BaseHandler.HandleIfNotModified(httpContext.Object, string.Empty));
        }

        [Fact]
        public void HandleIfNotModified_Should_Set_NotModified_Status_Code_In_Response()
        {
            const string ETag ="abcdef";

            var httpContext = MvcTestHelper.GetHttpContext();

            httpContext.HttpRequest.SetupGet(r => r.Headers).Returns(new NameValueCollection { { "If-None-Match", ETag } });
            httpContext.HttpResponse.SetupSet(r => r.StatusCode = Moq.It.IsAny<int>());

            httpContext.Verify();
        }

        [Fact]
        public void CreateUrlHelper_Should_Return_New_UrlHelper()
        {
            var httpContext = MvcTestHelper.GetHttpContext();

            Assert.NotNull(new BaseHandlerTestDouble().CreateUrlHelperForTest(httpContext.Object));
        }
    }

    public class BaseHandlerTestDouble : BaseHandler
    {
        public override void ProcessRequest(HttpContextBase context)
        {
        }

        public UrlHelper CreateUrlHelperForTest(HttpContextBase context)
        {
            return CreateUrlHelper(context);
        }
    }
}