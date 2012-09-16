using System;
using System.Collections.Specialized;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class OpenSearchHandlerFixture : BaseFixture
    {
        private readonly HttpContextMock _httpContext;

        public OpenSearchHandlerFixture()
        {
            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();

            _httpContext = MvcTestHelper.GetHttpContext("/Kigg", null, null);
        }

        [Fact]
        public void ProcessRequest_Should_Write_Xml()
        {
            string xml;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);

            var handler = new OpenSearchHandler
                              {
                                  Settings = settings.Object,
                                  CacheDurationInDays = 365,
                                  Compress = true,
                                  GenerateETag = true
                              };

            _httpContext.HttpResponse.Expect(r => r.Write(It.IsAny<string>())).Verifiable();

            handler.ProcessRequest(_httpContext.Object);

            _httpContext.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Not_Write_Xml_When_Xml_Is_Not_Modified()
        {
            string xml;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);

            var handler = new OpenSearchHandler
                              {
                                  Settings = settings.Object,
                                  CacheDurationInDays = 365,
                                  Compress = true,
                                  GenerateETag = true
                              };

            _httpContext.HttpRequest.ExpectGet(r => r.Headers).Returns(new NameValueCollection { { "If-None-Match", "5uSN9UoD5sU4x6sV+nD0ww==" } });
            _httpContext.HttpResponse.Expect(r => r.Write(It.IsAny<string>())).Never();

            handler.ProcessRequest(_httpContext.Object);

            _httpContext.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Cache_Content()
        {
            string xml;

            cache.Expect(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);
            cache.Expect(c => c.Contains(It.IsAny<string>())).Returns(false);
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<HandlerCacheItem>(), It.IsAny<DateTime>())).Verifiable();

            var handler = new OpenSearchHandler
                              {
                                  Settings = settings.Object,
                                  CacheDurationInDays = 365,
                                  Compress = true,
                                  GenerateETag = true
                              };

            handler.ProcessRequest(_httpContext.Object);

            cache.Verify();
        }
    }
}