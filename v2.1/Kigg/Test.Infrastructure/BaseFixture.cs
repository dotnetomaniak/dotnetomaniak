using System;
using Moq;

namespace Kigg.Test.Infrastructure
{
    using Kigg.Infrastructure;

    public abstract class BaseFixture : IDisposable
    {
        protected readonly Mock<IDependencyResolverFactory> resolverFactory;
        protected readonly Mock<ILog> log;
        protected readonly Mock<ICache> cache;
        protected readonly Mock<IUnitOfWork> unitOfWork;
        protected readonly Mock<IFile> file;
        protected readonly Mock<IConfigurationManager> configurationManager;
        protected readonly Mock<IThumbnail> thumbnail;
        protected readonly Mock<IConfigurationSettings> settings;
        protected readonly Mock<IDependencyResolver> resolver;

        protected BaseFixture()
        {
            settings = new Mock<IConfigurationSettings>();

            SetupSettingProperties();

            log = new Mock<ILog>();
            cache = new Mock<ICache>();
            unitOfWork = new Mock<IUnitOfWork>();
            file = new Mock<IFile>();
            configurationManager = new Mock<IConfigurationManager>();
            thumbnail = new Mock<IThumbnail>();
            resolver = new Mock<IDependencyResolver>();

            resolverFactory = new Mock<IDependencyResolverFactory>();

            resolverFactory.Expect(f => f.CreateInstance()).Returns(resolver.Object);

            resolver.Expect(r => r.Resolve<IConfigurationSettings>()).Returns(settings.Object);
            resolver.Expect(r => r.Resolve<ILog>()).Returns(log.Object);
            resolver.Expect(r => r.Resolve<ICache>()).Returns(cache.Object);
            resolver.Expect(r => r.Resolve<IFile>()).Returns(file.Object);
            resolver.Expect(r => r.Resolve<IConfigurationManager>()).Returns(configurationManager.Object);
            resolver.Expect(r => r.Resolve<IUnitOfWork>()).Returns(unitOfWork.Object);
            resolver.Expect(r => r.Resolve<IThumbnail>()).Returns(thumbnail.Object);

            IoC.InitializeWith(resolverFactory.Object);
        }

        private void SetupSettingProperties()
        {
            settings.ExpectGet(s => s.RootUrl).Returns("http://dotnetshoutout.com");
            settings.ExpectGet(s => s.WebmasterEmail).Returns("admin@dotnetshoutout.com");
            settings.ExpectGet(s => s.SupportEmail).Returns("support@dotnetshoutout.com");
            settings.ExpectGet(s => s.DefaultEmailOfOpenIdUser).Returns("openiduser@dotnetshoutout.com");
            settings.ExpectGet(s => s.SiteTitle).Returns("DotNetShoutout");
            settings.ExpectGet(s => s.MetaKeywords).Returns("Latest News, .NET, ASP.NET, C#, VB.NET, SQL, AJAX, Silverlight, WPF, WCF, WF, Linq, Windows Forms, Web Service, Visual Studio, TDD, Microsoft Platform");
            settings.ExpectGet(s => s.MetaDescription).Returns("DotNetShoutout is a place where you can find latest Microsoft .NET stories to increase your skills and share your opinions.");
            settings.ExpectGet(s => s.TopTags).Returns(50);
            settings.ExpectGet(s => s.AutoDiscoverContent).Returns(true);
            settings.ExpectGet(s => s.HtmlStoryPerPage).Returns(10);
            settings.ExpectGet(s => s.FeedStoryPerPage).Returns(25);
            settings.ExpectGet(s => s.CarouselStoryCount).Returns(5);
            settings.ExpectGet(s => s.TopUsers).Returns(20);
            settings.ExpectGet(s => s.AutoDiscoverContent).Returns(true);
            settings.ExpectGet(s => s.SendPing).Returns(true);
            settings.ExpectGet(s => s.PromoteText).Returns("Shout it");
            settings.ExpectGet(s => s.DemoteText).Returns("Mute it");
            settings.ExpectGet(s => s.CountText).Returns("shouts");
            settings.ExpectGet(s => s.MinimumAgeOfStoryInHoursToPublish).Returns(4);
            settings.ExpectGet(s => s.MaximumAgeOfStoryInHoursToPublish).Returns(168);
            settings.ExpectGet(s => s.AllowPossibleSpamStorySubmit).Returns(true);
            settings.ExpectGet(s => s.SendMailWhenPossibleSpamStorySubmitted).Returns(true);
            settings.ExpectGet(s => s.AllowPossibleSpamCommentSubmit).Returns(true);
            settings.ExpectGet(s => s.SendMailWhenPossibleSpamCommentSubmitted).Returns(true);
            settings.ExpectGet(s => s.PublishedStoriesFeedBurnerUrl).Returns("http://feeds.feedburner.com/Dotnetshoutout-Published");
            settings.ExpectGet(s => s.UpcomingStoriesFeedBurnerUrl).Returns("http://feeds.feedburner.com/Dotnetshoutout-Upcoming");
            settings.ExpectGet(s => s.MaximumUserScoreToShowCaptcha).Returns(25);
        }

        public virtual void Dispose()
        {
        }
    }
}