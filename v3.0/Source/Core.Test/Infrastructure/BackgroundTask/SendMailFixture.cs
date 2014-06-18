using System;
using System.Collections.Generic;
using Kigg.DomainObjects;
using Kigg.Infrastructure;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Service;

    public class SendMailFixture
    {
        private readonly Mock<IEventAggregator> _eventAggregator;
        private readonly Mock<IEmailSender> _emailSender;

        private readonly SendMail _sendMail;

        public SendMailFixture()
        {
            _eventAggregator = new Mock<IEventAggregator>();
            _emailSender = new Mock<IEmailSender>();

            _sendMail = new SendMail(_eventAggregator.Object, _emailSender.Object);
        }

        [Fact]
        public void CommentSubmitted_Should_SendMail()
        {
            var story = new Mock<IStory>();
            story.SetupGet(s => s.Subscribers).Returns(new List<IUser>());

            var comment = new Mock<IComment>();
            comment.SetupGet(c => c.ForStory).Returns(story.Object);

            _emailSender.Setup(es => es.SendComment(It.IsAny<string>(), It.IsAny<IComment>(), It.IsAny<IEnumerable<IUser>>())).Verifiable();

            _sendMail.CommentSubmitted(new CommentSubmitEventArgs(comment.Object, "http://dotnetshoutout.com"));

            _emailSender.Verify();
        }

        [Fact]
        public void CommentMarkedAsOffended_Should_SendMail()
        {
            var comment = new Mock<IComment>();

            _emailSender.Setup(es => es.NotifyCommentAsOffended(It.IsAny<string>(), It.IsAny<IComment>(), It.IsAny<IUser>())).Verifiable();

            _sendMail.CommentMarkedAsOffended(new CommentMarkAsOffendedEventArgs(comment.Object, new Mock<IUser>().Object, "http://dotnetshoutout.com"));

            _emailSender.Verify();
        }

        [Fact]
        public void CommentSpammed_Should_SendMail()
        {
            _emailSender.Setup(es => es.NotifyConfirmSpamComment(It.IsAny<string>(), It.IsAny<IComment>(), It.IsAny<IUser>())).Verifiable();

            _sendMail.CommentSpammed(new CommentSpamEventArgs(new Mock<IComment>().Object, new Mock<IUser>().Object,"http://dotnetshoutout.com"));

            _emailSender.Verify();
        }

        [Fact]
        public void StoryApproved_Should_SendMail()
        {
            _emailSender.Setup(es => es.NotifyStoryApprove(It.IsAny<string>(), It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _sendMail.StoryApproved(new StoryApproveEventArgs(new Mock<IStory>().Object, new Mock<IUser>().Object, "http://dotnetshoutout.com"));

            _emailSender.Verify();
        }

        [Fact]
        public void StoryDeleted_Should_SendMail()
        {
            _emailSender.Setup(es => es.NotifyStoryDelete(It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _sendMail.StoryDeleted(new StoryDeleteEventArgs(new Mock<IStory>().Object, new Mock<IUser>().Object));

            _emailSender.Verify();
        }

        [Fact]
        public void StoryMarkedAsSpam_Should_SendMail()
        {
            _emailSender.Setup(es => es.NotifyStoryMarkedAsSpam(It.IsAny<string>(), It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _sendMail.StoryMarkedAsSpam(new StoryMarkAsSpamEventArgs(new Mock<IStory>().Object, new Mock<IUser>().Object, It.IsAny<string>()));

            _emailSender.Verify();
        }

        [Fact]
        public void StorySpammed_Should_SendMail()
        {
            _emailSender.Setup(es => es.NotifyConfirmSpamStory(It.IsAny<string>(), It.IsAny<IStory>(), It.IsAny<IUser>())).Verifiable();

            _sendMail.StorySpammed(new StorySpamEventArgs(new Mock<IStory>().Object, new Mock<IUser>().Object, "http://dotnetshoutout.com"));

            _emailSender.Verify();
        }

        [Fact]
        public void StoryPublished_Should_SendMail()
        {
            _emailSender.Setup(es => es.NotifyPublishedStories(It.IsAny<DateTime>(), It.IsAny<IEnumerable<PublishedStory>>())).Verifiable();

            _sendMail.StoryPublished(new StoryPublishEventArgs(new List<PublishedStory>(), SystemTime.Now()));

            _emailSender.Verify();
        }

        [Fact]
        public void PossibleSpamStoryDetected_Should_SendMail()
        {
            _emailSender.Setup(es => es.NotifySpamStory(It.IsAny<string>(), It.IsAny<IStory>(), It.IsAny<string>())).Verifiable();

            _sendMail.PossibleSpamStoryDetected(new PossibleSpamStoryEventArgs(new Mock<IStory>().Object, "default","http://dotnetshoutout.com/Dummy-Story"));

            _emailSender.Verify();
        }

        [Fact]
        public void PossibleSpamCommentDetected_Should_SendMail()
        {
            _emailSender.Setup(es => es.NotifySpamComment(It.IsAny<string>(), It.IsAny<IComment>(), It.IsAny<string>())).Verifiable();

            _sendMail.PossibleSpamCommentDetected(new PossibleSpamCommentEventArgs(new Mock<IComment>().Object, "default", "http://dotnetshoutout.com/Dummy-Story"));

            _emailSender.Verify();
        }

        [Fact]
        public void Start_Should_Subscribe_Events()
        {
            var commentSubmitEvent = new Mock<CommentSubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentSubmitEvent>()).Returns(commentSubmitEvent.Object).Verifiable();
            commentSubmitEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentSubmitEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var commentMarkedAsOffended = new Mock<CommentMarkAsOffendedEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentMarkAsOffendedEvent>()).Returns(commentMarkedAsOffended.Object).Verifiable();
            commentMarkedAsOffended.Setup(e => e.Subscribe(It.IsAny<Action<CommentMarkAsOffendedEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var commentSpamEvent = new Mock<CommentSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentSpamEvent>()).Returns(commentSpamEvent.Object).Verifiable();
            commentSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentSpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyApproveEvent = new Mock<StoryApproveEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryApproveEvent>()).Returns(storyApproveEvent.Object).Verifiable();
            storyApproveEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryApproveEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyDeleteEvent = new Mock<StoryDeleteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryDeleteEvent>()).Returns(storyDeleteEvent.Object).Verifiable();
            storyDeleteEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryDeleteEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyMarkAsSpamEvent = new Mock<StoryMarkAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryMarkAsSpamEvent>()).Returns(storyMarkAsSpamEvent.Object).Verifiable();
            storyMarkAsSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryMarkAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storySpamEvent = new Mock<StorySpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySpamEvent>()).Returns(storySpamEvent.Object).Verifiable();
            storySpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StorySpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyPublishEvent = new Mock<StoryPublishEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryPublishEvent>()).Returns(storyPublishEvent.Object).Verifiable();
            storyPublishEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryPublishEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var possibleSpamStoryEvent = new Mock<PossibleSpamStoryEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<PossibleSpamStoryEvent>()).Returns(possibleSpamStoryEvent.Object).Verifiable();
            possibleSpamStoryEvent.Setup(e => e.Subscribe(It.IsAny<Action<PossibleSpamStoryEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var possibleSpamCommentEvent = new Mock<PossibleSpamCommentEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<PossibleSpamCommentEvent>()).Returns(possibleSpamCommentEvent.Object).Verifiable();
            possibleSpamCommentEvent.Setup(e => e.Subscribe(It.IsAny<Action<PossibleSpamCommentEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            _sendMail.Start();

            _eventAggregator.Verify();

            commentSubmitEvent.Verify();
            commentMarkedAsOffended.Verify();
            commentSpamEvent.Verify();
            storyApproveEvent.Verify();
            storyDeleteEvent.Verify();
            storySpamEvent.Verify();
            possibleSpamStoryEvent.Verify();
            possibleSpamCommentEvent.Verify();
        }

        [Fact]
        public void Stop_Should_Unsubscribe_Events()
        {
            var commentSubmitEvent = new Mock<CommentSubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentSubmitEvent>()).Returns(commentSubmitEvent.Object).Verifiable();
            commentSubmitEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentSubmitEventArgs>>(), true)).Returns(new SubscriptionToken());
            commentSubmitEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var commentMarkedAsOffended = new Mock<CommentMarkAsOffendedEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentMarkAsOffendedEvent>()).Returns(commentMarkedAsOffended.Object).Verifiable();
            commentMarkedAsOffended.Setup(e => e.Subscribe(It.IsAny<Action<CommentMarkAsOffendedEventArgs>>(), true)).Returns(new SubscriptionToken());
            commentMarkedAsOffended.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var commentSpamEvent = new Mock<CommentSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentSpamEvent>()).Returns(commentSpamEvent.Object).Verifiable();
            commentSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentSpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            commentSpamEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyApproveEvent = new Mock<StoryApproveEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryApproveEvent>()).Returns(storyApproveEvent.Object).Verifiable();
            storyApproveEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryApproveEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyApproveEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyDeleteEvent = new Mock<StoryDeleteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryDeleteEvent>()).Returns(storyDeleteEvent.Object).Verifiable();
            storyDeleteEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryDeleteEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyDeleteEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyMarkAsSpamEvent = new Mock<StoryMarkAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryMarkAsSpamEvent>()).Returns(storyMarkAsSpamEvent.Object).Verifiable();
            storyMarkAsSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryMarkAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyMarkAsSpamEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storySpamEvent = new Mock<StorySpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySpamEvent>()).Returns(storySpamEvent.Object).Verifiable();
            storySpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StorySpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            storySpamEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyPublishEvent = new Mock<StoryPublishEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryPublishEvent>()).Returns(storyPublishEvent.Object).Verifiable();
            storyPublishEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryPublishEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyPublishEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var possibleSpamStoryEvent = new Mock<PossibleSpamStoryEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<PossibleSpamStoryEvent>()).Returns(possibleSpamStoryEvent.Object).Verifiable();
            possibleSpamStoryEvent.Setup(e => e.Subscribe(It.IsAny<Action<PossibleSpamStoryEventArgs>>(), true)).Returns(new SubscriptionToken());
            possibleSpamStoryEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var possibleSpamCommentEvent = new Mock<PossibleSpamCommentEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<PossibleSpamCommentEvent>()).Returns(possibleSpamCommentEvent.Object).Verifiable();
            possibleSpamCommentEvent.Setup(e => e.Subscribe(It.IsAny<Action<PossibleSpamCommentEventArgs>>(), true)).Returns(new SubscriptionToken());
            possibleSpamCommentEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            _sendMail.Start();
            _sendMail.Stop();

            _eventAggregator.Verify();

            commentSubmitEvent.Verify();
            commentMarkedAsOffended.Verify();
            commentSpamEvent.Verify();
            storyApproveEvent.Verify();
            storyDeleteEvent.Verify();
            storySpamEvent.Verify();
            possibleSpamStoryEvent.Verify();
            possibleSpamCommentEvent.Verify();
        }
    }
}