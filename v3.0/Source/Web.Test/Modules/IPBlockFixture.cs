using System;
using System.Web;
using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class IPBlockFixture : BaseFixture
    {
        private readonly HttpContextMock _httpContext;
        private readonly BaseHttpModule _module;

        public IPBlockFixture()
        {
            const string BlockedIP = "192.168.0.1";

            file.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(string.Empty);

            var collection = new Mock<IBlockedIPCollection>();

            collection.Setup(c => c.Contains(BlockedIP)).Returns(true);
            resolver.Setup(r => r.Resolve<IBlockedIPCollection>()).Returns(collection.Object);

            _httpContext = MvcTestHelper.GetHttpContext();

            _httpContext.HttpRequest.SetupGet(r => r.UserHostAddress).Returns(BlockedIP);
            _httpContext.HttpRequest.SetupGet(r => r.Url).Returns(new Uri("http://dotnetshoutout.com/Upcoming"));

            _module = new IPBlock();
        }

        [Fact]
        public void OnBeginRequest_Should_Block_Ip_If_Ip_Exists_In_BlockedIpCollection()
        {
            Assert.Throws<HttpException>(() => _module.OnBeginRequest(_httpContext.Object));
        }

        [Fact]
        public void OnBeginRequest_Should_Warn_In_Log_When_Ip_Address_Is_Blocked()
        {
            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();

            Assert.Throws<HttpException>(() => _module.OnBeginRequest(_httpContext.Object));

            log.Verify();
        }

        [Fact]
        public void OnBeginRequest_Should_Not_Block_Assets_Directory()
        {
            _httpContext.HttpRequest.SetupGet(r => r.Url).Returns(new Uri("http://dotnetshoutout.com/Assets/a.jpg"));

            Assert.DoesNotThrow(() => _module.OnBeginRequest(_httpContext.Object));
        }

        [Fact]
        public void OnBeginRequest_Should_Not_Block_Access_Denied_Page()
        {
            _httpContext.HttpRequest.SetupGet(r => r.Url).Returns(new Uri("http://dotnetshoutout.com/ErrorPages/AccessDenied.aspx"));

            Assert.DoesNotThrow(() => _module.OnBeginRequest(_httpContext.Object));
        }
    }
}