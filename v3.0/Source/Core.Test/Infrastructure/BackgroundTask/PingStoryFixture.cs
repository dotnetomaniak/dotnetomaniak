using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Service;

    using Kigg.Test.Infrastructure;

    public class PingStoryFixture : BaseFixture
    {
        private readonly Mock<IContentService> _contentService;
        private readonly Mock<IHttpForm> _httpForm;
        private readonly Mock<IEventAggregator> _eventAggregator;

        private readonly PingStory _pingStory;

        public PingStoryFixture()
        {
            _httpForm = new Mock<IHttpForm>();
            _contentService = new Mock<IContentService>();
            _eventAggregator = new Mock<IEventAggregator>();

            _pingStory = new PingStory(settings.Object, _eventAggregator.Object, _httpForm.Object, _contentService.Object, "Thank you for submitting this cool story -{0}");
        }

        [Fact]
        public void StorySubmitted_Should_Use_ContentService()
        {
            StorySubmitted();

            _contentService.Verify();
        }

        [Fact]
        public void StorySubmitted_Should_Use_HttpForm()
        {
            StorySubmitted();

            _httpForm.Verify();
        }

        [Fact]
        public void StoryApproved_Should_Use_ContentService()
        {
            StoryApproved();

            _contentService.Verify();
        }

        [Fact]
        public void StoryApproved_Should_Use_HttpForm()
        {
            StoryApproved();

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

            _pingStory.Start();

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

            _pingStory.Start();
            _pingStory.Stop();

            _eventAggregator.Verify();
            storySubmitEvent.Verify();
            storyApproveEvent.Verify();
        }

        private void StorySubmitted()
        {
            var story = Setup();

            _pingStory.StorySubmitted(new StorySubmitEventArgs(story.Object, "http://dotnetshoutout.com/Dummy-Title"));
        }

        private void StoryApproved()
        {
            var story = Setup();

            _pingStory.StoryApproved(new StoryApproveEventArgs(story.Object, new Mock<IUser>().Object, "http://dotnetshoutout.com/Dummy-Title"));
        }

        private Mock<IStory> Setup()
        {
            var story = new Mock<IStory>();

            story.SetupGet(s => s.Title).Returns("Dummy Title");

            var content = new StoryContent("Dummy story", "Dummy content", "http://dummyurl.com");

            _contentService.Setup(cs => cs.Get(It.IsAny<string>())).Returns(content).Verifiable();
            _httpForm.Setup(h => h.PostAsync(It.IsAny<HttpFormPostRequest>())).Verifiable();

            return story;
        }
    }
}