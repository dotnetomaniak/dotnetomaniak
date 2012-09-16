using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Service;

    using Kigg.Test.Infrastructure;

    public class PingServerFixture : BaseFixture
    {
        private readonly Mock<IHttpForm> _httpForm;
        private readonly Mock<IEventAggregator> _eventAggregator;

        private readonly PingServer _pingServer;

        public PingServerFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _eventAggregator = new Mock<IEventAggregator>();

            _pingServer = new PingServer(settings.Object, _eventAggregator.Object, _httpForm.Object, "http://rpc.testserver.com", 30);
        }

        [Fact]
        public void StorySubmitted_Should_Http_Post()
        {
            SetupHttpForm();

            _pingServer.StorySubmitted(new StorySubmitEventArgs(new Mock<IStory>().Object, string.Empty));

            _httpForm.Verify();
        }

        [Fact]
        public void StoryApproved_Should_Http_Post()
        {
            SetupHttpForm();

            _pingServer.StoryApproved(new StoryApproveEventArgs(new Mock<IStory>().Object, new Mock<IUser>().Object,string.Empty));

            _httpForm.Verify();
        }

        [Fact]
        public void Start_Should_Subscribe_Event()
        {
            var storySubmitEvent = new Mock<StorySubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySubmitEvent>()).Returns(storySubmitEvent.Object).Verifiable();
            storySubmitEvent.Setup(e => e.Subscribe(It.IsAny<Action<StorySubmitEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyApproveEvent = new Mock<StoryApproveEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryApproveEvent>()).Returns(storyApproveEvent.Object).Verifiable();
            storyApproveEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryApproveEventArgs>>(), true)).Returns(new SubscriptionToken());

            _pingServer.Start();

            _eventAggregator.Verify();
            storySubmitEvent.Verify();
            storyApproveEvent.Verify();
        }

        [Fact]
        public void Stop_Should_Unsubscribe_Event()
        {
            var storySubmitEvent = new Mock<StorySubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySubmitEvent>()).Returns(storySubmitEvent.Object).Verifiable();
            storySubmitEvent.Setup(e => e.Subscribe(It.IsAny<Action<StorySubmitEventArgs>>(), true)).Returns(new SubscriptionToken());
            storySubmitEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyApproveEvent = new Mock<StoryApproveEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryApproveEvent>()).Returns(storyApproveEvent.Object).Verifiable();
            storyApproveEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryApproveEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyApproveEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            _pingServer.Start();
            _pingServer.Stop();

            _eventAggregator.Verify();
            storySubmitEvent.Verify();
            storyApproveEvent.Verify();
        }

        private void SetupHttpForm()
        {
            _httpForm.Setup(h => h.PostAsync(It.IsAny<HttpFormPostRawRequest>(), It.IsAny<Action<HttpFormResponse>>(), It.IsAny<Action<Exception>>())).Callback((HttpFormPostRawRequest request, Action<HttpFormResponse> onComplete, Action<Exception> onError) => onComplete(new HttpFormResponse { Response = "success" })).Verifiable();
        }
    }
}