using Kigg.Repository;

namespace Kigg.Service
{
    using System;

    using DomainObjects;
    using Infrastructure;

    public class UserScoreService : BaseBackgroundTask
    {
        private readonly IConfigurationSettings _settings;
        private readonly IUserScoreTable _userScoreTable;
        private readonly IVoteRepository _voteRepository;

        private SubscriptionToken _userActivatedToken;
        private SubscriptionToken _storySubmitToken;
        private SubscriptionToken _storyViewToken;
        private SubscriptionToken _storyPromoteToken;
        private SubscriptionToken _storyDemoteToken;
        private SubscriptionToken _storyMarkAsSpamToken;
        private SubscriptionToken _storyUnmarkAsSpamToken;
        private SubscriptionToken _commentSubmitToken;
        private SubscriptionToken _storySpamToken;
        private SubscriptionToken _commentSpamToken;
        private SubscriptionToken _storyDeleteToken;
        private SubscriptionToken _commentMarkedAsOffendedToken;
        private SubscriptionToken _storyApproveToken;
        private SubscriptionToken _storyPublishToken;
        private SubscriptionToken _storyIncorrectlyMarkedAsSpamToken;

        public UserScoreService(IConfigurationSettings settings, IUserScoreTable userScoreTable, IEventAggregator eventAggregator, IVoteRepository voteRepository) : base(eventAggregator)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(userScoreTable, "userScoreTable");

            _settings = settings;
            _userScoreTable = userScoreTable;
            _voteRepository = voteRepository;
        }

        protected override void OnStart()
        {
            if (!IsRunning)
            {
                _userActivatedToken = Subscribe<UserActivateEvent, UserActivateEventArgs>(UserActivated);
                _storySubmitToken = Subscribe<StorySubmitEvent, StorySubmitEventArgs>(StorySubmitted);
                _storyViewToken = Subscribe<StoryViewEvent, StoryViewEventArgs>(StoryViewed);
                _storyPromoteToken = Subscribe<StoryPromoteEvent, StoryPromoteEventArgs>(StoryPromoted);
                _storyDemoteToken = Subscribe<StoryDemoteEvent, StoryDemoteEventArgs>(StoryDemoted);
                _storyMarkAsSpamToken = Subscribe<StoryMarkAsSpamEvent, StoryMarkAsSpamEventArgs>(StoryMarkedAsSpam);
                _storyUnmarkAsSpamToken = Subscribe<StoryUnmarkAsSpamEvent, StoryUnmarkAsSpamEventArgs>(StoryUnmarkedAsSpam);
                _commentSubmitToken = Subscribe<CommentSubmitEvent, CommentSubmitEventArgs>(CommentSubmitted);
                _storySpamToken = Subscribe<StorySpamEvent, StorySpamEventArgs>(StorySpammed);
                _commentSpamToken = Subscribe<CommentSpamEvent, CommentSpamEventArgs>(CommentSpammed);
                _storyDeleteToken = Subscribe<StoryDeleteEvent, StoryDeleteEventArgs>(StoryDeleted);
                _commentMarkedAsOffendedToken = Subscribe<CommentMarkAsOffendedEvent, CommentMarkAsOffendedEventArgs>(CommentMarkedAsOffended);
                _storyApproveToken = Subscribe<StoryApproveEvent, StoryApproveEventArgs>(StoryApproved);
                _storyPublishToken = Subscribe<StoryPublishEvent, StoryPublishEventArgs>(StoryPublished);
                _storyIncorrectlyMarkedAsSpamToken = Subscribe<StoryIncorrectlyMarkedAsSpamEvent, StoryIncorrectlyMarkedAsSpamEventArgs>(StoryIncorrectlyMarkedAsSpam);
            }
        }

        protected override void OnStop()
        {
            if (IsRunning)
            {
                Unsubscribe<UserActivateEvent>(_userActivatedToken);
                Unsubscribe<StorySubmitEvent>(_storySubmitToken);
                Unsubscribe<StoryViewEvent>(_storyViewToken);
                Unsubscribe<StoryPromoteEvent>(_storyPromoteToken);
                Unsubscribe<StoryDemoteEvent>(_storyDemoteToken);
                Unsubscribe<StoryMarkAsSpamEvent>(_storyMarkAsSpamToken);
                Unsubscribe<StoryUnmarkAsSpamEvent>(_storyUnmarkAsSpamToken);
                Unsubscribe<CommentSubmitEvent>(_commentSubmitToken);
                Unsubscribe<StorySpamEvent>(_storySpamToken);
                Unsubscribe<CommentSpamEvent>(_commentSpamToken);
                Unsubscribe<StoryDeleteEvent>(_storyDeleteToken);
                Unsubscribe<CommentMarkAsOffendedEvent>(_commentMarkedAsOffendedToken);
                Unsubscribe<StoryApproveEvent>(_storyApproveToken);
                Unsubscribe<StoryPublishEvent>(_storyPublishToken);
                Unsubscribe<StoryIncorrectlyMarkedAsSpamEvent>(_storyIncorrectlyMarkedAsSpamToken);
            }
        }

        internal void UserActivated(UserActivateEventArgs eventArgs)
        {
            if (eventArgs.User.IsPublicUser())
            {
                eventArgs.User.IncreaseScoreBy(_userScoreTable.AccountActivated, UserAction.AccountActivated);
            }
        }

        internal void StorySubmitted(StorySubmitEventArgs eventArgs)
        {
            StorySubmitted(eventArgs.Story.PostedBy);
        }

        internal void StoryViewed(StoryViewEventArgs eventArgs)
        {
            if (eventArgs.User != null)
            {
                if (CanChangeScoreForStory(eventArgs.Story, eventArgs.User))
                {
                    eventArgs.User.IncreaseScoreBy(_userScoreTable.StoryViewed, UserAction.StoryViewed);
                }
            }
        }

        internal void StoryPromoted(StoryPromoteEventArgs eventArgs)
        {
            if (CanChangeScoreForStory(eventArgs.Story, eventArgs.User))
            {
                UserAction reason;
                decimal score;

                if (eventArgs.Story.IsPublished())
                {
                    score = _userScoreTable.PublishedStoryPromoted;
                    reason = UserAction.PublishedStoryPromoted;
                }
                else
                {
                    score = _userScoreTable.UpcomingStoryPromoted;
                    reason = UserAction.UpcomingStoryPromoted;
                }

                eventArgs.User.IncreaseScoreBy(score, reason);
                _voteRepository.InvalidateCacheForStory(eventArgs.Story.Id);
            }
        }

        internal void StoryDemoted(StoryDemoteEventArgs eventArgs)
        {
            if (CanChangeScoreForStory(eventArgs.Story, eventArgs.User))
            {
                // It might not decrease the same value which was increased when promoting the story
                // depending upon the story status(e.g. published/upcoming), but who cares!!!
                UserAction reason;
                decimal score;

                if (eventArgs.Story.IsPublished())
                {
                    score = _userScoreTable.PublishedStoryPromoted;
                    reason = UserAction.PublishedStoryDemoted;
                }
                else
                {
                    score = _userScoreTable.UpcomingStoryPromoted;
                    reason = UserAction.UpcomingStoryDemoted;
                }

                eventArgs.User.DecreaseScoreBy(score, reason);
                _voteRepository.InvalidateCacheForStory(eventArgs.Story.Id);
            }
        }

        internal void StoryMarkedAsSpam(StoryMarkAsSpamEventArgs eventArgs)
        {
            if (CanChangeScoreForStory(eventArgs.Story, eventArgs.User))
            {
                eventArgs.User.IncreaseScoreBy(_userScoreTable.StoryMarkedAsSpam, UserAction.StoryMarkedAsSpam);
            }
        }

        internal void StoryUnmarkedAsSpam(StoryUnmarkAsSpamEventArgs eventArgs)
        {
            if (CanChangeScoreForStory(eventArgs.Story, eventArgs.User))
            {
                eventArgs.User.DecreaseScoreBy(_userScoreTable.StoryMarkedAsSpam, UserAction.StoryUnmarkedAsSpam);
            }
        }

        internal void CommentSubmitted(CommentSubmitEventArgs eventArgs)
        {
            if (CanChangeScoreForStory(eventArgs.Comment.ForStory, eventArgs.Comment.ByUser))
            {
                eventArgs.Comment.ByUser.IncreaseScoreBy(_userScoreTable.StoryCommented, UserAction.StoryCommented);
            }
        }

        internal void StorySpammed(StorySpamEventArgs eventArgs)
        {
            StoryRemoved(eventArgs.Story, UserAction.SpamStorySubmitted);

            if (eventArgs.Story.PostedBy.IsPublicUser())
            {
                eventArgs.Story.PostedBy.DecreaseScoreBy(_userScoreTable.SpamStorySubmitted, UserAction.SpamStorySubmitted);
            }
        }

        internal void CommentSpammed(CommentSpamEventArgs eventArgs)
        {
            if (eventArgs.User.IsPublicUser())
            {
                eventArgs.User.DecreaseScoreBy(_userScoreTable.StoryCommented + _userScoreTable.SpamCommentSubmitted, UserAction.SpamCommentSubmitted);
            }
        }

        internal void StoryDeleted(StoryDeleteEventArgs eventArgs)
        {
            StoryRemoved(eventArgs.Story, UserAction.StoryDeleted);

            if (eventArgs.Story.PostedBy.IsPublicUser())
            {
                eventArgs.Story.PostedBy.DecreaseScoreBy(_userScoreTable.StorySubmitted, UserAction.StoryDeleted);
            }
        }

        internal void CommentMarkedAsOffended(CommentMarkAsOffendedEventArgs eventArgs)
        {
            if (eventArgs.User.IsPublicUser())
            {
                eventArgs.User.DecreaseScoreBy(_userScoreTable.StoryCommented, UserAction.CommentMarkedAsOffended);
            }
        }

        internal void StoryApproved(StoryApproveEventArgs eventArgs)
        {
            StorySubmitted(eventArgs.Story.PostedBy);
        }

        internal void StoryPublished(StoryPublishEventArgs eventArgs)
        {
            foreach(PublishedStory publishedStory in eventArgs.PublishedStories)
            {
                if (publishedStory.Story.PostedBy.IsPublicUser())
                {
                    publishedStory.Story.PostedBy.IncreaseScoreBy(_userScoreTable.StoryPublished, UserAction.StoryPublished);
                }
            }
        }

        internal void StoryIncorrectlyMarkedAsSpam(StoryIncorrectlyMarkedAsSpamEventArgs eventArgs)
        {
            if (eventArgs.User.IsPublicUser())
            {
                eventArgs.User.DecreaseScoreBy((_userScoreTable.StoryIncorrectlyMarkedAsSpam + _userScoreTable.StoryMarkedAsSpam), UserAction.StoryIncorrectlyMarkedAsSpam);
            }
        }

        private static bool CanChangeScoreForStory(IStory theStory, IUser theUser)
        {
            return !theStory.HasExpired() && theUser.IsPublicUser();
        }

        private void StoryRemoved(IStory theStory, UserAction action)
        {
            Check.Argument.IsNotNull(theStory, "theStory");

            DateTime expireDate = theStory.CreatedAt.AddHours(_settings.MaximumAgeOfStoryInHoursToPublish);

            foreach (IMarkAsSpam markAsSpam in theStory.MarkAsSpams)
            {
                if (markAsSpam.ByUser.IsPublicUser() && (markAsSpam.MarkedAt <= expireDate))
                {
                    markAsSpam.ByUser.DecreaseScoreBy(_userScoreTable.StoryMarkedAsSpam, action);
                }
            }

            foreach (IComment comment in theStory.Comments)
            {
                if (comment.ByUser.IsPublicUser() && (comment.CreatedAt <= expireDate))
                {
                    comment.ByUser.DecreaseScoreBy(_userScoreTable.StoryCommented, action);
                }
            }

            foreach (IVote vote in theStory.Votes)
            {
                if (!theStory.IsPostedBy(vote.ByUser))
                {
                    if (vote.ByUser.IsPublicUser() && (vote.PromotedAt <= expireDate))
                    {
                        decimal score = theStory.IsPublished() ?
                                        _userScoreTable.PublishedStoryPromoted :
                                        _userScoreTable.UpcomingStoryPromoted;

                        vote.ByUser.DecreaseScoreBy(score, action);
                    }
                }
            }
        }

        private void StorySubmitted(IUser ofUser)
        {
            if (ofUser.IsPublicUser())
            {
                ofUser.IncreaseScoreBy(_userScoreTable.StorySubmitted, UserAction.StorySubmitted);
            }
        }
    }
}