using System;
using System.Net;
using Xunit;

namespace Kigg.Web.Test
{
    public class RemoveWwwRedirectorFixture
    {
        [Fact]
        public void OnBeginRequest_Should_Redirect_When_Requesting_WWW_Url()
        {
            var httpContext = MvcTestHelper.GetHttpContext();

            httpContext.HttpRequest.SetupGet(r => r.Url).Returns(new Uri("http://www.dotnetshoutout.com/Upcoming"));

            httpContext.HttpResponse.SetupSet(r => r.StatusCode = (int)HttpStatusCode.MovedPermanently).Verifiable();
            httpContext.HttpResponse.SetupSet(r => r.Status = "301 Moved Permanently").Verifiable();
            httpContext.HttpResponse.SetupSet(r => r.RedirectLocation = "http://dotnetshoutout.com/Upcoming").Verifiable();
            httpContext.HttpResponse.Setup(r => r.End()).Verifiable();

            var module = new RemoveWwwRedirector();

            module.OnBeginRequest(httpContext.Object);

            httpContext.Verify();
        }
    }
}