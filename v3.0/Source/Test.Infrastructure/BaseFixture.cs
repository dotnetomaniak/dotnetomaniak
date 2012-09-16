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

            resolverFactory.Setup(f => f.CreateInstance()).Returns(resolver.Object);

            resolver.Setup(r => r.Resolve<IConfigurationSettings>()).Returns(settings.Object);
            resolver.Setup(r => r.Resolve<ILog>()).Returns(log.Object);
            resolver.Setup(r => r.Resolve<ICache>()).Returns(cache.Object);
            resolver.Setup(r => r.Resolve<IFile>()).Returns(file.Object);
            resolver.Setup(r => r.Resolve<IConfigurationManager>()).Returns(configurationManager.Object);
            resolver.Setup(r => r.Resolve<IUnitOfWork>()).Returns(unitOfWork.Object);
            resolver.Setup(r => r.Resolve<IThumbnail>()).Returns(thumbnail.Object);

            IoC.InitializeWith(resolverFactory.Object);
        }

        private void SetupSettingProperties()
        {
            settings.SetupGet(s => s.RootUrl).Returns("http://dotnetshoutout.com");
            settings.SetupGet(s => s.WebmasterEmail).Returns("admin@dotnetshoutout.com");
            settings.SetupGet(s => s.SupportEmail).Returns("support@dotnetshoutout.com");
            settings.SetupGet(s => s.DefaultEmailOfOpenIdUser).Returns("openiduser@dotnetshoutout.com");
            settings.SetupGet(s => s.SiteTitle).Returns("DotNetShoutout");
            settings.SetupGet(s => s.MetaKeywords).Returns("Latest News, .NET, ASP.NET, C#, VB.NET, SQL, AJAX, Silverlight, WPF, WCF, WF, Linq, Windows Forms, Web Service, Visual Studio, TDD, Microsoft Platform");
            settings.SetupGet(s => s.MetaDescription).Returns("DotNetShoutout is a place where you can find latest Microsoft .NET stories to increase your skills and share your opinions.");
            settings.SetupGet(s => s.TopTags).Returns(50);
            settings.SetupGet(s => s.AutoDiscoverContent).Returns(true);
            settings.SetupGet(s => s.HtmlStoryPerPage).Returns(10);
            settings.SetupGet(s => s.FeedStoryPerPage).Returns(25);
            settings.SetupGet(s => s.CarouselStoryCount).Returns(5);
            settings.SetupGet(s => s.TopUsers).Returns(20);
            settings.SetupGet(s => s.AutoDiscoverContent).Returns(true);
            settings.SetupGet(s => s.SendPing).Returns(true);
            settings.SetupGet(s => s.PromoteText).Returns("Shout it");
            settings.SetupGet(s => s.DemoteText).Returns("Mute it");
            settings.SetupGet(s => s.CountText).Returns("shouts");
            settings.SetupGet(s => s.MinimumAgeOfStoryInHoursToPublish).Returns(4);
            settings.SetupGet(s => s.MaximumAgeOfStoryInHoursToPublish).Returns(168);
            settings.SetupGet(s => s.AllowPossibleSpamStorySubmit).Returns(true);
            settings.SetupGet(s => s.SendMailWhenPossibleSpamStorySubmitted).Returns(true);
            settings.SetupGet(s => s.AllowPossibleSpamCommentSubmit).Returns(true);
            settings.SetupGet(s => s.SendMailWhenPossibleSpamCommentSubmitted).Returns(true);
            settings.SetupGet(s => s.PublishedStoriesFeedBurnerUrl).Returns("http://feeds.feedburner.com/Dotnetshoutout-Published");
            settings.SetupGet(s => s.UpcomingStoriesFeedBurnerUrl).Returns("http://feeds.feedburner.com/Dotnetshoutout-Upcoming");
            settings.SetupGet(s => s.MaximumUserScoreToShowCaptcha).Returns(25);
        }
        
        protected Mock<T> SetupResolve<T>() where T : class
        {
            var repository = new Mock<T>();
            resolver.Setup(r => r.Resolve<T>()).Returns(repository.Object);
            return repository;
        }
        public virtual void Dispose()
        {
        }
    }
}