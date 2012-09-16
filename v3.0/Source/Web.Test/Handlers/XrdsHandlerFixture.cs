using System;
using System.Collections.Specialized;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class XrdsHandlerFixture : BaseFixture
    {
        private readonly HttpContextMock _httpContext;

        public XrdsHandlerFixture()
        {
            RouteTable.Routes.Clear();
            new RegisterRoutes(settings.Object).Execute();

            _httpContext = MvcTestHelper.GetHttpContext("/Kigg", null, null);
        }

        [Fact]
        public void ProcessRequest_Should_Write_Xml()
        {
            string xml;

            cache.Setup(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);

            var handler = new XrdsHandler
                              {
                                  Settings = settings.Object,
                                  CacheDurationInDays = 365,
                                  Compress = true,
                                  GenerateETag = true
                              };

            _httpContext.HttpResponse.Setup(r => r.Write(It.IsAny<string>())).Verifiable();

            handler.ProcessRequest(_httpContext.Object);

            _httpContext.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Not_Write_Xml_When_Xml_Is_Not_Modified()
        {
            string xml;

            cache.Setup(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);

            var handler = new XrdsHandler
                              {
                                  Settings = settings.Object,
                                  CacheDurationInDays = 365,
                                  Compress = true,
                                  GenerateETag = true
                              };

            _httpContext.HttpRequest.SetupGet(r => r.Headers).Returns(new NameValueCollection { { "If-None-Match", "cPL22YznvwACntJKLjS+2w==" } });
            //_httpContext.HttpResponse.Setup(r => r.Write(It.IsAny<string>())).Never();
            _httpContext.HttpResponse.Verify(r => r.Write(It.IsAny<string>()),Times.Never());

            handler.ProcessRequest(_httpContext.Object);

            _httpContext.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Cache_Content()
        {
            string xml;

            cache.Setup(c => c.TryGet(It.IsAny<string>(), out xml)).Returns(false);
            cache.Setup(c => c.Contains(It.IsAny<string>())).Returns(false);
            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<HandlerCacheItem>(), It.IsAny<DateTime>())).Verifiable();

            var handler = new XrdsHandler
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