using System.Web;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;

    public class MsdnRedirectorFixture
    {
        private readonly ISocialServiceRedirector _redirector;

        public MsdnRedirectorFixture()
        {
            _redirector = new MsdnRedirector();
        }

        [Fact]
        public void Redirect_Should_Take_User_To_MSDN_Bookmarking_Page()
        {
            var httpContext = new Mock<HttpContextBase>();
            var httpResponse = new Mock<HttpResponseBase>();

            httpContext.ExpectGet(c => c.Response).Returns(httpResponse.Object);
            httpResponse.Expect(r => r.Redirect(It.IsAny<string>())).Verifiable();

            var story = new Mock<IStory>();
            story.ExpectGet(s => s.Url).Returns("http://asp.net");
            story.ExpectGet(s => s.Title).Returns("Official ASP.net site");
            story.ExpectGet(s => s.TextDescription).Returns("A Dummy description");

            _redirector.Redirect(httpContext.Object, story.Object);

            httpResponse.Verify();
        }
    }
}