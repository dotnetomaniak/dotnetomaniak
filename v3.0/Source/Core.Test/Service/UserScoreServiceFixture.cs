using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Service;

    using Kigg.Test.Infrastructure;

    public class UserScoreServiceFixture : BaseFixture
    {
        private readonly Mock<IEventAggregator> _eventAggregator;
        private readonly Mock<IUser> _user;
        private readonly UserScoreService _userScoreService;

        public UserScoreServiceFixture()
        {
            Mock<IUserScoreTable> userScoreTable = new Mock<IUserScoreTable>();

            userScoreTable.SetupGet(us => us.AccountActivated).Returns(5);
            userScoreTable.SetupGet(us => us.StorySubmitted).Returns(20);
            userScoreTable.SetupGet(us => us.StoryViewed).Returns(1);
            userScoreTable.SetupGet(us => us.UpcomingStoryPromoted).Returns(3);
            userScoreTable.SetupGet(us => us.PublishedStoryPromoted).Returns(2);
            userScoreTable.SetupGet(us => us.StoryPublished).Returns(10);
            userScoreTable.SetupGet(us => us.StoryCommented).Returns(2);
            userScoreTable.SetupGet(us => us.StoryMarkedAsSpam).Returns(10);
            userScoreTable.SetupGet(us => us.SpamStorySubmitted).Returns(50);
            userScoreTable.SetupGet(us => us.StoryIncorrectlyMarkedAsSpam).Returns(1);
            userScoreTable.SetupGet(us => us.SpamCommentSubmitted).Returns(5);

            _user = new Mock<IUser>();

            _user.SetupGet(u => u.Id).Returns(Guid.NewGuid());
            _user.SetupGet(u => u.Role).Returns(Roles.User);

            _eventAggregator = new Mock<IEventAggregator>();

            _userScoreService = new UserScoreService(settings.Object, userScoreTable.Object, _eventAggregator.Object);
        }

        public override void Dispose()
        {
            _user.Verify();
        }

        [Fact]
        public void UserActivated_Should_Increase_Score()
        {
            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.AccountActivated)).Verifiable();
            _userScoreService.UserActivated(new UserActivateEventArgs(_user.Object));
        }

        [Fact]
        public void StorySubmitted_Should_Increase_Score()
        {
            var story = new Mock<IStory>();

            story.SetupGet(s => s.PostedBy).Returns(_user.Object);

            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StorySubmitted)).Verifiable();
            _userScoreService.StorySubmitted(new StorySubmitEventArgs(story.Object, string.Empty));
        }

        [Fact]
        public void StoryViewed_Should_Increase_Score_When_Story_Has_Not_Expired()
        {
            var story = MockStory();

            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryViewed)).Verifiable();
            _userScoreService.StoryViewed(new StoryViewEventArgs(story.Object, _user.Object));
        }

        [Fact]
        public void StoryPromoted_Should_Increase_Score_When_Story_Is_Published_And_Story_Has_Not_Expired()
        {
            var story = MockStory();

            story.SetupGet(s => s.PublishedAt).Returns(SystemTime.Now().AddHours(-4));
            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.PublishedStoryPromoted)).Verifiable();

            _userScoreService.StoryPromoted(new StoryPromoteEventArgs(story.Object, _user.Object));
        }

        [Fact]
        public void StoryPromoted_Should_Increase_Score_When_Story_Is_Not_Published_And_Story_Has_Not_Expired()
        {
            var story = MockStory();

            story.SetupGet(s => s.PublishedAt).Returns((DateTime?) null);
            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.UpcomingStoryPromoted)).Verifiable();

            _userScoreService.StoryPromoted(new StoryPromoteEventArgs(story.Object, _user.Object));
        }

        [Fact]
        public void StoryDemoted_Should_Decreases_Score_When_Story_Is_Published_And_Story_Has_Not_Expired()
        {
            var story = MockStory();

            story.SetupGet(s => s.PublishedAt).Returns(SystemTime.Now().AddHours(-4));
            _user.Setup(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.PublishedStoryDemoted)).Verifiable();

            _userScoreService.StoryDemoted(new StoryDemoteEventArgs(story.Object, _user.Object));
        }

        [Fact]
        public void StoryDemoted_Should_Decreases_Score_When_Story_Is_Not_Published_And_Story_Has_Not_Expired()
        {
            var story = MockStory();

            story.SetupGet(s => s.PublishedAt).Returns((DateTime?) null);
            _user.Setup(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.UpcomingStoryDemoted)).Verifiable();

            _userScoreService.StoryDemoted(new StoryDemoteEventArgs(story.Object, _user.Object));
        }

        [Fact]
        public void StoryMarkedAsSpam_Should_Increase_Score_When_Story_Has_Not_Expired()
        {
            var story = MockStory();

            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryMarkedAsSpam)).Verifiable();
            _userScoreService.StoryMarkedAsSpam(new StoryMarkAsSpamEventArgs(story.Object, _user.Object, string.Empty));
        }

        [Fact]
        public void StoryUnmarkedAsSpam_Should_Decrease_Score_When_Story_Has_Not_Expired()
        {
            var story = MockStory();

            _user.Setup(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryUnmarkedAsSpam)).Verifiable();
            _userScoreService.StoryUnmarkedAsSpam(new StoryUnmarkAsSpamEventArgs(story.Object, _user.Object));
        }

        [Fact]
        public void CommentSubmitted_Should_Increase_Score_When_Story_Has_Not_Expired()
        {
            var comment = new Mock<IComment>();
            var story = MockStory();

            comment.SetupGet(c => c.ForStory).Returns(story.Object);
            comment.SetupGet(c => c.ByUser).Returns(_user.Object);

            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryCommented)).Verifiable();
            _userScoreService.CommentSubmitted(new CommentSubmitEventArgs(comment.Object, string.Empty));
        }

        [Fact]
        public void StoryDeleted_Should_Decreasase_Score()
        {
            var story = new Mock<IStory>();

            PrepareStoryToRemove(story);
            story.SetupGet(s => s.PostedBy).Returns(_user.Object);

            _user.Setup(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryDeleted)).Verifiable();

            _userScoreService.StoryDeleted(new StoryDeleteEventArgs(story.Object, _user.Object));
        }

        [Fact]
        public void StoryPublished_Should_Increase_Score()
        {
            List<PublishedStory> stories = new List<PublishedStory>();

            for(int i = 0; i < 5; i++)
            {
                var story = new Mock<IStory>();
                story.SetupGet(s => s.PostedBy).Returns(_user.Object);
                stories.Add(new PublishedStory(story.Object));
            }

            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryPublished)).Verifiable();
            _userScoreService.StoryPublished(new StoryPublishEventArgs(stories, SystemTime.Now()));
        }

        [Fact]
        public void StorySpammed_Should_Decreasase_Score()
        {
            var story = new Mock<IStory>();

            PrepareStoryToRemove(story);
            story.SetupGet(s => s.PostedBy).Returns(_user.Object);

            _user.Setup(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.SpamStorySubmitted)).Verifiable();

            _userScoreService.StorySpammed(new StorySpamEventArgs(story.Object, _user.Object, string.Empty));
        }

        [Fact]
        public void StoryIncorrectlyMarkedAsSpam_Should_Decrease_Score()
        {
            _user.Setup(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.StoryIncorrectlyMarkedAsSpam)).Verifiable();
            _userScoreService.StoryIncorrectlyMarkedAsSpam(new StoryIncorrectlyMarkedAsSpamEventArgs(new Mock<IStory>().Object, _user.Object));
        }

        [Fact]
        public void CommentSpammed_Should_Decrease_Score()
        {
            var comment = new Mock<IComment>();
            comment.SetupGet(c => c.ByUser).Returns(new Mock<IUser>().Object);

            _user.Setup(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.SpamCommentSubmitted)).Verifiable();
            _userScoreService.CommentSpammed(new CommentSpamEventArgs(comment.Object, _user.Object, string.Empty));
        }

        [Fact]
        public void CommentMarkedAsOffended_Should_Decrease_Score()
        {
            var comment = new Mock<IComment>();
            comment.SetupGet(c => c.ByUser).Returns(new Mock<IUser>().Object);

            _user.Setup(u => u.DecreaseScoreBy(It.IsAny<decimal>(), UserAction.CommentMarkedAsOffended)).Verifiable();
            _userScoreService.CommentMarkedAsOffended(new CommentMarkAsOffendedEventArgs(comment.Object, _user.Object, string.Empty));
        }

        [Fact]
        public void StoryApproved_Should_Increase_Score()
        {
            var story = MockStory();
            story.SetupGet(s => s.PostedBy).Returns(_user.Object);

            _user.Setup(u => u.IncreaseScoreBy(It.IsAny<decimal>(), UserAction.StorySubmitted)).Verifiable();
            _userScoreService.StoryApproved(new StoryApproveEventArgs(story.Object, new Mock<IUser>().Object, string.Empty));
        }

        [Fact]
        public void Start_Should_Subscribe_Events()
        {
            var userActivateEvent = new Mock<UserActivateEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<UserActivateEvent>()).Returns(userActivateEvent.Object).Verifiable();
            userActivateEvent.Setup(e => e.Subscribe(It.IsAny<Action<UserActivateEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storySubmitEvent = new Mock<StorySubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySubmitEvent>()).Returns(storySubmitEvent.Object).Verifiable();
            storySubmitEvent.Setup(e => e.Subscribe(It.IsAny<Action<StorySubmitEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyViewEvent = new Mock<StoryViewEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryViewEvent>()).Returns(storyViewEvent.Object).Verifiable();
            storyViewEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryViewEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyPromoteEvent = new Mock<StoryPromoteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryPromoteEvent>()).Returns(storyPromoteEvent.Object).Verifiable();
            storyPromoteEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryPromoteEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyDemoteEvent = new Mock<StoryDemoteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryDemoteEvent>()).Returns(storyDemoteEvent.Object).Verifiable();
            storyDemoteEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryDemoteEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyMarkAsSpamEvent = new Mock<StoryMarkAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryMarkAsSpamEvent>()).Returns(storyMarkAsSpamEvent.Object).Verifiable();
            storyMarkAsSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryMarkAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyUnMarkAsSpamEvent = new Mock<StoryUnmarkAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryUnmarkAsSpamEvent>()).Returns(storyUnMarkAsSpamEvent.Object).Verifiable();
            storyUnMarkAsSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryUnmarkAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var commentSubmitEvent = new Mock<CommentSubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentSubmitEvent>()).Returns(commentSubmitEvent.Object).Verifiable();
            commentSubmitEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentSubmitEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storySpamEvent = new Mock<StorySpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySpamEvent>()).Returns(storySpamEvent.Object).Verifiable();
            storySpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StorySpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var commentSpamEvent = new Mock<CommentSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentSpamEvent>()).Returns(commentSpamEvent.Object).Verifiable();
            commentSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentSpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyDeleteEvent = new Mock<StoryDeleteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryDeleteEvent>()).Returns(storyDeleteEvent.Object).Verifiable();
            storyDeleteEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryDeleteEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var commentMarkAsOffendedEvent = new Mock<CommentMarkAsOffendedEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentMarkAsOffendedEvent>()).Returns(commentMarkAsOffendedEvent.Object).Verifiable();
            commentMarkAsOffendedEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentMarkAsOffendedEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyApproveEvent = new Mock<StoryApproveEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryApproveEvent>()).Returns(storyApproveEvent.Object).Verifiable();
            storyApproveEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryApproveEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyPublishEvent = new Mock<StoryPublishEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryPublishEvent>()).Returns(storyPublishEvent.Object).Verifiable();
            storyPublishEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryPublishEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyIncorrectlyMarkedAsSpamEvent = new Mock<StoryIncorrectlyMarkedAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryIncorrectlyMarkedAsSpamEvent>()).Returns(storyIncorrectlyMarkedAsSpamEvent.Object).Verifiable();
            storyIncorrectlyMarkedAsSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryIncorrectlyMarkedAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            _userScoreService.Start();

            _eventAggregator.Verify();
            userActivateEvent.Verify();
            storySubmitEvent.Verify();
            storyViewEvent.Verify();
            storyPromoteEvent.Verify();
            storyDemoteEvent.Verify();
            storyMarkAsSpamEvent.Verify();
            storyUnMarkAsSpamEvent.Verify();
            commentSubmitEvent.Verify();
            storySpamEvent.Verify();
            commentSpamEvent.Verify();
            storyDeleteEvent.Verify();
            commentMarkAsOffendedEvent.Verify();
            storyApproveEvent.Verify();
            storyPublishEvent.Verify();
            storyIncorrectlyMarkedAsSpamEvent.Verify();
        }

        [Fact]
        public void Stop_Should_Unsubscribe_Events()
        {
            var userActivateEvent = new Mock<UserActivateEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<UserActivateEvent>()).Returns(userActivateEvent.Object).Verifiable();
            userActivateEvent.Setup(e => e.Subscribe(It.IsAny<Action<UserActivateEventArgs>>(), true)).Returns(new SubscriptionToken());
            userActivateEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>()));

            var storySubmitEvent = new Mock<StorySubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySubmitEvent>()).Returns(storySubmitEvent.Object).Verifiable();
            storySubmitEvent.Setup(e => e.Subscribe(It.IsAny<Action<StorySubmitEventArgs>>(), true)).Returns(new SubscriptionToken());
            storySubmitEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>()));

            var storyViewEvent = new Mock<StoryViewEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryViewEvent>()).Returns(storyViewEvent.Object).Verifiable();
            storyViewEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryViewEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyViewEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>()));

            var storyPromoteEvent = new Mock<StoryPromoteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryPromoteEvent>()).Returns(storyPromoteEvent.Object).Verifiable();
            storyPromoteEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryPromoteEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();
            storyPromoteEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>()));

            var storyDemoteEvent = new Mock<StoryDemoteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryDemoteEvent>()).Returns(storyDemoteEvent.Object).Verifiable();
            storyDemoteEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryDemoteEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyDemoteEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyMarkAsSpamEvent = new Mock<StoryMarkAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryMarkAsSpamEvent>()).Returns(storyMarkAsSpamEvent.Object).Verifiable();
            storyMarkAsSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryMarkAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyMarkAsSpamEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyUnMarkAsSpamEvent = new Mock<StoryUnmarkAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryUnmarkAsSpamEvent>()).Returns(storyUnMarkAsSpamEvent.Object).Verifiable();
            storyUnMarkAsSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryUnmarkAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyUnMarkAsSpamEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var commentSubmitEvent = new Mock<CommentSubmitEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentSubmitEvent>()).Returns(commentSubmitEvent.Object).Verifiable();
            commentSubmitEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentSubmitEventArgs>>(), true)).Returns(new SubscriptionToken());
            commentSubmitEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storySpamEvent = new Mock<StorySpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StorySpamEvent>()).Returns(storySpamEvent.Object).Verifiable();
            storySpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StorySpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            storySpamEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var commentSpamEvent = new Mock<CommentSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentSpamEvent>()).Returns(commentSpamEvent.Object).Verifiable();
            commentSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentSpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            commentSpamEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyDeleteEvent = new Mock<StoryDeleteEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryDeleteEvent>()).Returns(storyDeleteEvent.Object).Verifiable();
            storyDeleteEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryDeleteEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyDeleteEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var commentMarkAsOffendedEvent = new Mock<CommentMarkAsOffendedEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<CommentMarkAsOffendedEvent>()).Returns(commentMarkAsOffendedEvent.Object).Verifiable();
            commentMarkAsOffendedEvent.Setup(e => e.Subscribe(It.IsAny<Action<CommentMarkAsOffendedEventArgs>>(), true)).Returns(new SubscriptionToken());
            commentMarkAsOffendedEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyApproveEvent = new Mock<StoryApproveEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryApproveEvent>()).Returns(storyApproveEvent.Object).Verifiable();
            storyApproveEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryApproveEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyApproveEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyPublishEvent = new Mock<StoryPublishEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryPublishEvent>()).Returns(storyPublishEvent.Object).Verifiable();
            storyPublishEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryPublishEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyPublishEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyIncorrectlyMarkedAsSpamEvent = new Mock<StoryIncorrectlyMarkedAsSpamEvent>();

            _eventAggregator.Setup(ea => ea.GetEvent<StoryIncorrectlyMarkedAsSpamEvent>()).Returns(storyIncorrectlyMarkedAsSpamEvent.Object).Verifiable();
            storyIncorrectlyMarkedAsSpamEvent.Setup(e => e.Subscribe(It.IsAny<Action<StoryIncorrectlyMarkedAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyIncorrectlyMarkedAsSpamEvent.Setup(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            _userScoreService.Start();
            _userScoreService.Stop();

            _eventAggregator.Verify();
            userActivateEvent.Verify();
            storySubmitEvent.Verify();
            storyViewEvent.Verify();
            storyPromoteEvent.Verify();
            storyDemoteEvent.Verify();
            storyMarkAsSpamEvent.Verify();
            storyUnMarkAsSpamEvent.Verify();
            commentSubmitEvent.Verify();
            storySpamEvent.Verify();
            commentSpamEvent.Verify();
            storyDeleteEvent.Verify();
            commentMarkAsOffendedEvent.Verify();
            storyApproveEvent.Verify();
            storyPublishEvent.Verify();
            storyIncorrectlyMarkedAsSpamEvent.Verify();
        }

        private static Mock<IStory> MockStory()
        {
            var story = new Mock<IStory>();
            story.SetupGet(s => s.CreatedAt).Returns(SystemTime.Now().AddHours(-1));

            return story;
        }

        private static void PrepareStoryToRemove(Mock<IStory> story)
        {
            const int counter = 5;

            DateTime fakeDate = SystemTime.Now().AddDays(-1);

            List<IMarkAsSpam> markAsSpams = new List<IMarkAsSpam>();

            for(var i = 1; i <= counter; i++)
            {
                var markAsSpam = new Mock<IMarkAsSpam>();

                markAsSpam.SetupGet(m => m.ByUser).Returns(new Mock<IUser>().Object);
                markAsSpam.SetupGet(m => m.MarkedAt).Returns(fakeDate.AddHours(1));

                markAsSpams.Add(markAsSpam.Object);
            }

            story.SetupGet(s => s.MarkAsSpams).Returns(markAsSpams);

            List<IComment> comments = new List<IComment>();

            for (var i = 1; i <= counter; i++)
            {
                var comment = new Mock<IComment>();

                comment.SetupGet(c => c.ByUser).Returns(new Mock<IUser>().Object);
                comment.SetupGet(c => c.CreatedAt).Returns(fakeDate.AddHours(1));

                comments.Add(comment.Object);
            }

            story.SetupGet(s => s.Comments).Returns(comments);

            List<IVote> votes = new List<IVote>();

            for (var i = 1; i <= counter; i++)
            {
                var vote = new Mock<IVote>();

                vote.SetupGet(v => v.ByUser).Returns(new Mock<IUser>().Object);
                vote.SetupGet(v => v.PromotedAt).Returns(fakeDate.AddHours(1));

                votes.Add(vote.Object);
            }

            story.SetupGet(s => s.Votes).Returns(votes);

            story.SetupGet(s => s.CreatedAt).Returns(fakeDate);
        }
    }
}