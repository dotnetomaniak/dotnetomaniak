using System;

using Xunit;

namespace Kigg.Web.Test
{
    public class RemoveWwwRedirectorFixture
    {
        [Fact]
        public void OnBeginRequest_Should_Redirect_WWW_Less_Url_When_Requesting_WWW_Url()
        {
            var httpContext = MvcTestHelper.GetHttpContext();

            httpContext.HttpRequest.ExpectGet(r => r.Url).Returns(new Uri("http://www.dotnetshoutout.com/Upcoming"));

            httpContext.HttpResponse.ExpectSet(r => r.StatusCode).Verifiable();
            httpContext.HttpResponse.ExpectSet(r => r.Status).Verifiable();
            httpContext.HttpResponse.ExpectSet(r => r.RedirectLocation).Verifiable();
            httpContext.HttpResponse.Expect(r => r.End()).Verifiable();

            var module = new RemoveWwwRedirector();

            module.OnBeginRequest(httpContext.Object);

            httpContext.Verify();
        }
    }
}