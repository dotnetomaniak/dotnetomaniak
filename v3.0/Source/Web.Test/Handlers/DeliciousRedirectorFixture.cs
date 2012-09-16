using System.Web;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;

    public class DeliciousRedirectorFixture
    {
        private readonly ISocialServiceRedirector _redirector;

        public DeliciousRedirectorFixture()
        {
            _redirector = new DeliciousRedirector();
        }

        [Fact]
        public void Redirect_Should_Take_User_To_Delicious_Page()
        {
            var httpContext = new Mock<HttpContextBase>();
            var httpResponse = new Mock<HttpResponseBase>();

            httpContext.SetupGet(c => c.Response).Returns(httpResponse.Object);
            httpResponse.Setup(r => r.Redirect(It.IsAny<string>())).Verifiable();

            var story = new Mock<IStory>();
            story.SetupGet(s => s.Url).Returns("http://asp.net");
            story.SetupGet(s => s.Title).Returns("Official ASP>net site");

            _redirector.Redirect(httpContext.Object, story.Object);

            httpContext.Verify();
        }
    }
}