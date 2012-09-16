namespace Kigg.Service
{
    using System;

    using DomainObjects;

    public class UserScoreService : IUserScoreService
    {
        private readonly IConfigurationSettings _settings;
        private readonly IUserScoreTable _userScoreTable;

        public UserScoreService(IConfigurationSettings settings, IUserScoreTable userScoreTable)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(userScoreTable, "userScoreTable");

            _settings = settings;
            _userScoreTable = userScoreTable;
        }

        public virtual void AccountActivated(IUser ofUser)
        {
            Check.Argument.IsNotNull(ofUser, "ofUser");

            if (ofUser.IsPublicUser())
            {
                ofUser.IncreaseScoreBy(_userScoreTable.AccountActivated, UserAction.AccountActivated);
            }
        }

        public virtual void StorySubmitted(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            if (byUser.IsPublicUser())
            {
                byUser.IncreaseScoreBy(_userScoreTable.StorySubmitted, UserAction.StorySubmitted);
            }
        }

        public virtual void StoryViewed(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                byUser.IncreaseScoreBy(_userScoreTable.StoryViewed, UserAction.StoryViewed);
            }
        }

        public virtual void StoryPromoted(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                UserAction reason;
                decimal score;

                if (theStory.IsPublished())
                {
                    score = _userScoreTable.PublishedStoryPromoted;
                    reason = UserAction.PublishedStoryPromoted;
                }
                else
                {
                    score = _userScoreTable.UpcomingStoryPromoted;
                    reason = UserAction.UpcomingStoryPromoted;
                }

                byUser.IncreaseScoreBy(score, reason);
            }
        }

        public virtual void StoryDemoted(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                // It might not decrease the same value which was increased when promoting the story
                // depending upon the story status(e.g. published/upcoming), but who cares!!!
                UserAction reason;
                decimal score;

                if (theStory.IsPublished())
                {
                    score = _userScoreTable.PublishedStoryPromoted;
                    reason = UserAction.PublishedStoryDemoted;
                }
                else
                {
                    score = _userScoreTable.UpcomingStoryPromoted;
                    reason = UserAction.UpcomingStoryDemoted;
                }

                byUser.DecreaseScoreBy(score, reason);
            }
        }

        public virtual void StoryMarkedAsSpam(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                byUser.IncreaseScoreBy(_userScoreTable.StoryMarkedAsSpam, UserAction.StoryMarkedAsSpam);
            }
        }

        public virtual void StoryUnmarkedAsSpam(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                byUser.DecreaseScoreBy(_userScoreTable.StoryMarkedAsSpam, UserAction.StoryUnmarkedAsSpam);
            }
        }

        public virtual void StoryCommented(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                byUser.IncreaseScoreBy(_userScoreTable.StoryCommented, UserAction.StoryCommented);
            }
        }

        public virtual void StoryDeleted(IStory theStory)
        {
            Check.Argument.IsNotNull(theStory, "theStory");

            StoryRemoved(theStory, UserAction.StoryDeleted);
        }

        public virtual void StoryPublished(IStory theStory)
        {
            Check.Argument.IsNotNull(theStory, "theStory");

            if (theStory.PostedBy.IsPublicUser())
            {
                theStory.PostedBy.IncreaseScoreBy(_userScoreTable.StoryPublished, UserAction.StoryPublished);
            }
        }

        public virtual void StorySpammed(IStory theStory)
        {
            Check.Argument.IsNotNull(theStory, "theStory");

            StoryRemoved(theStory, UserAction.SpamStorySubmitted);

            if (theStory.PostedBy.IsPublicUser())
            {
                theStory.PostedBy.DecreaseScoreBy(_userScoreTable.SpamStorySubmitted, UserAction.SpamStorySubmitted);
            }
        }

        public virtual void StoryIncorrectlyMarkedAsSpam(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "ofUser");

            if (byUser.IsPublicUser())
            {
                byUser.DecreaseScoreBy((_userScoreTable.StoryIncorrectlyMarkedAsSpam + _userScoreTable.StoryMarkedAsSpam), UserAction.StoryIncorrectlyMarkedAsSpam);
            }
        }

        public virtual void CommentSpammed(IUser ofUser)
        {
            Check.Argument.IsNotNull(ofUser, "ofUser");

            if (ofUser.IsPublicUser())
            {
                ofUser.DecreaseScoreBy(_userScoreTable.StoryCommented + _userScoreTable.SpamCommentSubmitted, UserAction.SpamCommentSubmitted);
            }
        }

        public virtual void CommentMarkedAsOffended(IUser ofUser)
        {
            Check.Argument.IsNotNull(ofUser, "ofUser");

            if (ofUser.IsPublicUser())
            {
                ofUser.DecreaseScoreBy(_userScoreTable.StoryCommented, UserAction.CommentMarkedAsOffended);
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

            if (theStory.PostedBy.IsPublicUser())
            {
                theStory.PostedBy.DecreaseScoreBy(_userScoreTable.StorySubmitted, action);
            }
        }
    }
}