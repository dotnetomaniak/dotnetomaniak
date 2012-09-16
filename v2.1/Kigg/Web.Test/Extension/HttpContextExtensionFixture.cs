using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

using Moq;

using Xunit;
using Xunit.Extensions;

namespace Kigg.Web.Test
{
    public class HttpContextExtensionFixture : IDisposable
    {
        private readonly Mock<HttpContextBase> _httpContext;

        public HttpContextExtensionFixture()
        {
            _httpContext = new Mock<HttpContextBase>();
        }

        public void Dispose()
        {
            _httpContext.Verify();
        }

        [Fact]
        public void CacheFor_Should_Set_up_Cache_Correctly()
        {
            var httpResponse = new Mock<HttpResponseBase>();
            var httpCachePolicy = new Mock<HttpCachePolicyBase>();

            _httpContext.ExpectGet(c => c.Timestamp).Returns(SystemTime.Now());
            _httpContext.ExpectGet(c => c.Response).Returns(httpResponse.Object);
            httpResponse.ExpectGet(r => r.Cache).Returns(httpCachePolicy.Object);

            httpCachePolicy.Expect(c => c.SetCacheability(It.IsAny<HttpCacheability>())).Verifiable();
            httpCachePolicy.Expect(c => c.SetExpires(It.IsAny<DateTime>())).Verifiable();
            httpCachePolicy.Expect(c => c.SetMaxAge(It.IsAny<TimeSpan>())).Verifiable();
            httpCachePolicy.Expect(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches)).Verifiable();

            _httpContext.Object.CacheResponseFor(TimeSpan.FromDays(1));
        }

        [Theory]
        [InlineData("GZip")]
        [InlineData("Deflate")]
        public void Compress_Should_Set_Correct_Encoding(string encoding)
        {
            var httpResponse = new Mock<HttpResponseBase>();
            var httpRequest = new Mock<HttpRequestBase>();

            _httpContext.ExpectGet(c => c.Request).Returns(httpRequest.Object);
            _httpContext.ExpectGet(c => c.Response).Returns(httpResponse.Object);

            httpRequest.ExpectGet(r => r.Headers).Returns(new NameValueCollection { { "Accept-Encoding", encoding } });

            Stream filter = new MemoryStream();

            httpResponse.ExpectGet(r => r.Filter).Returns(filter).Verifiable();
            httpResponse.Expect(r => r.AddHeader("Content-encoding", It.IsAny<string>()));

            _httpContext.Object.CompressResponse();
        }
    }
}