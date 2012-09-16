using System.Web;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Infrastructure;

    public class TwitterRedirectorFixture
    {
        private readonly ISocialServiceRedirector _redirector;
        private readonly Mock<IContentService> _contentService;

        public TwitterRedirectorFixture()
        {
            _contentService = new Mock<IContentService>();
            _redirector = new TwitterRedirector(_contentService.Object);
        }

        [Fact]
        public void Redirect_Should_Take_User_To_Twitter_Page()
        {
            var httpContext = new Mock<HttpContextBase>();
            var httpResponse = new Mock<HttpResponseBase>();

            httpContext.SetupGet(c => c.Response).Returns(httpResponse.Object);
            httpResponse.Setup(r => r.Redirect(It.IsAny<string>())).Verifiable();

            var story = new Mock<IStory>();
            story.SetupGet(s => s.Url).Returns("http://asp.net");

            _contentService.Setup(c => c.ShortUrl(It.IsAny<string>())).Returns("http://tinyurl.com/a1");

            _redirector.Redirect(httpContext.Object, story.Object);

            _contentService.Verify();
            httpContext.Verify();
        }
    }
}