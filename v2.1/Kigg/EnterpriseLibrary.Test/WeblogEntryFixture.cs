using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogEntryFixture
    {
        private readonly Mock<HttpContextBase> _context;

        public WeblogEntryFixture()
        {
            _context = new Mock<HttpContextBase>();
        }

        [Fact]
        public void Constructor_Should_Not_Throw_Exception_When_Not_Hosted_In_WebServer()
        {
            Assert.DoesNotThrow(() => new WeblogEntry("Test", "Test", TraceEventType.Critical, null, null, null));
        }

        [Fact]
        public void GetMethodDetails_Should_Return_Namespace_Class_And_MethodSignature()
        {
            string namespaceName;
            string className;
            string methodSignature;

            WeblogEntry.GetMethodDetails(1, out namespaceName, out className, out methodSignature);

            Assert.True(namespaceName.Length > 0);
            Assert.True(className.Length > 0);
            Assert.True(methodSignature.Length > 0);
        }

        [Fact]
        public void NamespaceName_Returns_The_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Null(CreateEntry(_context.Object).NamespaceName);
        }

        [Fact]
        public void Is_Request_Available_Should_Be_False_When_Application_Start()
        {
            _context.ExpectGet(c => c.Request).Throws<HttpException>();

            Assert.False(CreateEntry(_context.Object).IsRequestAvailable);
        }

        [Fact]
        public void Is_Request_Available_Should_Be_True_When_Application_Is_Running()
        {
            var request = new Mock<HttpRequestBase>();
            _context.ExpectGet(c => c.Request).Returns(request.Object);

            Assert.True(CreateEntry(_context.Object).IsRequestAvailable);
        }

        [Fact]
        public void ClassName_Returns_The_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Null(CreateEntry(_context.Object).ClassName);
        }

        [Fact]
        public void MethodSignature_Should_Return_The_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Null(CreateEntry(_context.Object).MethodSignature);
        }

        [Fact]
        public void CurentUserName_Returns_Unavailable_When_HttpContext_Is_Null()
        {
            Assert.Equal(WeblogEntry.Unavailable, CreateEntry(null).CurrentUserName);
        }

        [Fact]
        public void CurentUserName_Returns_UserName_When_HttpContext_Is_Not_Null()
        {
            const string UserName = "DummyUser";

            var identity = new Mock<IIdentity>();

            identity.ExpectGet(i => i.IsAuthenticated).Returns(true);
            identity.ExpectGet(i => i.Name).Returns(UserName);

            var user =  new Mock<IPrincipal>();

            user.ExpectGet(u => u.Identity).Returns(identity.Object);

            _context.ExpectGet(c => c.User).Returns(user.Object);

            Assert.Equal(UserName, CreateEntry(_context.Object).CurrentUserName);
        }

        [Fact]
        public void CurrentUserIPAddress_Returns_Unavailable_When_HttpContext_Is_Null()
        {
            Assert.Equal(WeblogEntry.Unavailable, CreateEntry(null).CurrentUserIPAddress);
        }

        [Fact]
        public void CurrentUserIPAddress_Returns_IPAaddress_When_HttpContext_Is_Not_Null()
        {
            var serverVariables = new NameValueCollection { { "HTTP_X_FORWARDED_FOR", "192.168.1.0" } };
            var request = new Mock<HttpRequestBase>();

            request.ExpectGet(r => r.UserHostAddress).Returns("202.192.168.1");
            request.ExpectGet(r => r.ServerVariables).Returns(serverVariables);

            _context.ExpectGet(c => c.Request).Returns(request.Object);

            Assert.Equal("202.192.168.1->192.168.1.0", CreateEntry(_context.Object).CurrentUserIPAddress);
        }

        [Fact]
        public void CurrentUserAgent_Returns_Unavailable_When_HttpContext_Is_Null()
        {
            Assert.Equal(WeblogEntry.Unavailable, CreateEntry(null).CurrentUserAgent);
        }

        [Fact]
        public void CurrentUserAgent_Returns_UserAgent_When_HttpContext_Is_Not_Null()
        {
            const string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9) Gecko/2008052906 Firefox/3.0";
            var request = new Mock<HttpRequestBase>();

            request.ExpectGet(r => r.UserAgent).Returns(UserAgent);
            _context.ExpectGet(c => c.Request).Returns(request.Object);

            Assert.Equal(UserAgent, CreateEntry(_context.Object).CurrentUserAgent);
        }

        [Fact]
        public void CurrentUrl_Returns_Unavailable_When_HttpContext_Is_Null()
        {
            Assert.Equal(WeblogEntry.Unavailable, CreateEntry(null).CurrentUrl);
        }

        [Fact]
        public void CurrentUrl_Returns_Url_When_HttpContext_Is_Not_Null()
        {
            const string Url = "http://dotnetshoutout.com/Category/Asp.net";

            var request = new Mock<HttpRequestBase>();

            request.ExpectGet(r => r.RawUrl).Returns(Url);
            _context.ExpectGet(c => c.Request).Returns(request.Object);

            Assert.Equal(Url, CreateEntry(_context.Object).CurrentUrl);
        }

        [Fact]
        public void CurrentUrlReferrer_Should_Return_Unavailable_When_HttpContext_Is_Null()
        {
            Assert.Equal(WeblogEntry.Unavailable, CreateEntry(null).CurrentUrlReferrer);
        }

        [Fact]
        public void CurrentUrlReferrer_Should_Return_Url_When_HttpContext_Is_Not_Null()
        {
            const string Url = "http://dotnetshoutout.com/";

            var url = new Uri(Url);

            var request = new Mock<HttpRequestBase>();

            request.ExpectGet(r => r.UrlReferrer).Returns(url);
            _context.ExpectGet(c => c.Request).Returns(request.Object);

            Assert.Equal(Url, CreateEntry(_context.Object).CurrentUrlReferrer);
        }

        private static WeblogEntry CreateEntry(HttpContextBase context)
        {
            return new WeblogEntry(context, "Test", "Test", TraceEventType.Critical, null, null, null);
        }
    }
}