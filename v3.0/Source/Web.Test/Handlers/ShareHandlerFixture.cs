using System;
using System.Collections.Specialized;
using System.Web;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using DomainObjects;
    using Repository;
    using Kigg.Test.Infrastructure;

    public class ShareHandlerFixture : BaseFixture
    {
        private readonly HttpContextMock _httpContext;

        private readonly Mock<IStoryRepository> _storyRepository;
        private readonly Mock<ISocialServiceRedirector> _redirector;

        private readonly ShareHandler _handler;

        public ShareHandlerFixture()
        {
            _httpContext = MvcTestHelper.GetHttpContext();

            _redirector = new Mock<ISocialServiceRedirector>();
            _storyRepository = new Mock<IStoryRepository>();

            resolver.Setup(r => r.Resolve<ISocialServiceRedirector>(It.IsAny<string>())).Returns(_redirector.Object);

            _handler = new ShareHandler { StoryRepository = _storyRepository.Object };
        }

        public override void Dispose()
        {
            _httpContext.Verify();
        }

        [Fact]
        public void RedirectToPrevious_Should_Redirect_To_Referrer()
        {
            _httpContext.HttpRequest.SetupGet(r => r.UrlReferrer).Returns(new Uri("http://dotnetshoutout.com/Upcoming"));
            _httpContext.HttpResponse.Setup(r => r.Redirect(It.IsAny<string>())).Verifiable();

            ShareHandler.RedirectToPrevious(_httpContext.Object);
        }

        [Fact]
        public void ProcessRequest_Should_Redirect_Successfully()
        {
            _httpContext.HttpRequest.SetupGet(r => r.UrlReferrer).Returns(new Uri("http://dotnetshoutout.com/Upcoming"));
            _httpContext.HttpRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection { { "id", Guid.NewGuid().Shrink() }, { "srv", "twitter" } });

            _httpContext.HttpResponse.Setup(r => r.Redirect(It.IsAny<string>()));

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(new Mock<IStory>().Object);

            _redirector.Setup(r => r.Redirect(It.IsAny<HttpContextBase>(), It.IsAny<IStory>()));

            _handler.ProcessRequest(_httpContext.Object);

            _redirector.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Redirect_To_Referrer_When_Id_Is_Not_Valid()
        {
            _httpContext.HttpRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection { { "id", "abc" }, { "srv", "twitter" } });
            _httpContext.HttpResponse.Setup(r => r.Redirect(It.IsAny<string>())).Verifiable();

            _handler.ProcessRequest(_httpContext.Object);
        }

        [Fact]
        public void ProcessRequest_Should_Redirect_To_Referrer_When_Story_Does_Not_Exist()
        {
            _httpContext.HttpRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection { { "id", Guid.NewGuid().Shrink() }, { "srv", "twitter" } });
            _httpContext.HttpResponse.Setup(r => r.Redirect(It.IsAny<string>())).Verifiable();

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns((IStory) null).Verifiable();

            _handler.ProcessRequest(_httpContext.Object);
        }

        [Fact]
        public void ProcessRequest_Should_Redirect_To_Default_Service_When_Invalid_Service_Is_Specified()
        {
            _httpContext.HttpRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection { { "id", Guid.NewGuid().Shrink() }, { "srv", "google" } });
            _httpContext.HttpResponse.Setup(r => r.Redirect(It.IsAny<string>()));

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(new Mock<IStory>().Object);

            resolver.Setup(r => r.Resolve<ISocialServiceRedirector>("google")).Throws<Exception>();
            resolver.Setup(r => r.Resolve<ISocialServiceRedirector>("msdn")).Returns(_redirector.Object);

            _redirector.Setup(r => r.Redirect(It.IsAny<HttpContextBase>(), It.IsAny<IStory>()));

            _handler.ProcessRequest(_httpContext.Object);

            _redirector.Verify();
        }

        [Fact]
        public void ProcessRequest_Should_Log_When_Invalid_Service_Is_Specified()
        {
            _httpContext.HttpRequest.SetupGet(r => r.QueryString).Returns(new NameValueCollection { { "id", Guid.NewGuid().Shrink() }, { "srv", "google" } });
            _httpContext.HttpResponse.Setup(r => r.Redirect(It.IsAny<string>()));

            _storyRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(new Mock<IStory>().Object);

            resolver.Setup(r => r.Resolve<ISocialServiceRedirector>("google")).Throws<Exception>();
            resolver.Setup(r => r.Resolve<ISocialServiceRedirector>("msdn")).Returns(_redirector.Object);

            log.Setup(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            _handler.ProcessRequest(_httpContext.Object);

            log.Verify();
        }
    }
}