using System;
using System.Collections.Generic;
using Kigg.Core.DomainObjects;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;

namespace Kigg.Infrastructure.EF
{
    public class DomainObjectFactory: IDomainObjectFactory
    {
        public IUser CreateUser(string userName, string email, string password)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotInvalidEmail(email, "email");

            DateTime now = SystemTime.Now();

            string hashedPassword = null;

            if (!string.IsNullOrEmpty(password))
            {
                hashedPassword = password.Trim().Hash();
            }

            return new User
            {
                Id = Guid.NewGuid(),
                UserName = userName.Trim(),
                Email = email.ToLowerInvariant(),
                Password = hashedPassword,
                Role = Roles.User,
                IsActive = string.IsNullOrEmpty(hashedPassword),
                LastActivityAt = now,
                CreatedAt = now,
                UserTags =  new List<UserTag>(),
                CommentSubscribtions = new List<CommentSubscribtion>(),
                Stories = new List<Story>(),
                StoryComments = new List<StoryComment>(),
                StoryMarkAsSpams = new List<StoryMarkAsSpam>(),
                StoryVotes = new List<StoryVote>(),
                UserScores = new List<UserScore>(),
                UserAchievements = new List<UserAchievement>()
            };
        }

        public IKnownSource CreateKnownSource(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            return new KnownSource
            {
                Url = url.ToLowerInvariant(),
                Grade = KnownSourceGrade.None
            };
        }

        public ICategory CreateCategory(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return new Category
            {
                Id = Guid.NewGuid(),
                UniqueName = name.Trim().ToLegalUrl(),
                Name = name.Trim(),
                CreatedAt = SystemTime.Now(),
                Stories = new List<Story>()
            };
        }

        public ITag CreateTag(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return new Tag
            {
                Id = Guid.NewGuid(),
                UniqueName = name.Trim().ToLegalUrl(),
                Name = name.Trim(),
                CreatedAt = SystemTime.Now(),
                UserTags = new List<UserTag>(),
                StoryTags = new List<StoryTag>()
            };
        }

        public IStory CreateStory(ICategory forCategory, IUser byUser, string fromIpAddress, string title, string description, string url)
        {
            Check.Argument.IsNotNull(forCategory, "forCategory");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");
            Check.Argument.IsNotEmpty(title, "title");
            Check.Argument.IsNotEmpty(description, "description");
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            DateTime now = SystemTime.Now();

            var story = new Story
            {
                Id = Guid.NewGuid(),
                UniqueName = title.Trim().ToLegalUrl().RemovePolishNationalChars(),
                Title = title.Trim(),
                HtmlDescription = description.Trim(),
                TextDescription = description.StripHtml().Trim(),
                Url = url,
                IPAddress = fromIpAddress,
                CreatedAt = now,
                LastActivityAt = now,
                UrlHash = url.ToUpperInvariant().Hash(),
                User = (User)byUser,
                Category = (Category)forCategory,
                CommentSubscribtions = new List<CommentSubscribtion>(),
                StoryComments = new List<StoryComment>(),
                StoryMarkAsSpams = new List<StoryMarkAsSpam>(),
                StoryTags = new List<StoryTag>(),
                StoryViews = new List<StoryView>(),
                StoryVotes = new List<StoryVote>()
            };
            return story;
        }

        public IStoryView CreateStoryView(IStory forStory, DateTime at, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var view = new StoryView
            {
                Story = (Story)forStory,
                IPAddress = fromIpAddress,
                Timestamp = at,
            };

            return view;
        }

        public IVote CreateStoryVote(IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            PerformCheck(forStory, at, byUser, fromIpAddress);

            var vote = new StoryVote
            {
                Story = (Story)forStory,
                User = (User)byUser,
                IPAddress = fromIpAddress,
                Timestamp = at
            };
            return vote;
        }

        public IMarkAsSpam CreateMarkAsSpam(IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            PerformCheck(forStory, at, byUser, fromIpAddress);

            var spamStory = new StoryMarkAsSpam
            {
                Story = (Story)forStory,
                User = (User)byUser,
                IPAddress = fromIpAddress,
                Timestamp = at
            };

            return spamStory;

        }

        public IComment CreateComment(IStory forStory, string content, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotEmpty(content, "content");
            PerformCheck(forStory, at, byUser, fromIpAddress);

            var comment = new StoryComment
            {
                Id = Guid.NewGuid(),
                HtmlBody = content.Trim(),
                TextBody = content.StripHtml().Trim(),
                Story = (Story)forStory,
                User = (User)byUser,
                IPAddress = fromIpAddress,
                CreatedAt = at
            };
            return comment;
        }

        public ICommentSubscribtion CreateCommentSubscribtion(IStory forStory, IUser byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            var subscribtion = new CommentSubscribtion
            {
                Story = (Story)forStory,
                User = (User)byUser
            };
            return subscribtion;
        }

        public IRecommendation CreateRecommendation(string recommendationLink, string recommendationTitle, string imageLink,
                string imageTitle, DateTime startTime, DateTime endTime, string email, int position, bool notificationIsSent, bool isBanner)
        {
            Check.Argument.IsNotEmpty(recommendationLink, "RecommendationLink");
            Check.Argument.IsNotEmpty(recommendationTitle, "RecommendationTitle");
            Check.Argument.IsNotEmpty(imageLink, "ImageLink");
            Check.Argument.IsNotEmpty(imageTitle, "ImageTitle");
            //Check.Argument.IsNotInvalidDate(startTime, "StartTime");
            //Check.Argument.IsNotInvalidDate(endTime, "EndTime");
            //Check.Argument.IsNotInPast(endTime, "EndTime");
            //Check.Argument.IsNotNegativeOrZero(position, "Position");
            DateTime now = SystemTime.Now();

            var recommendation = new Recommendation
            {

                Id = Guid.NewGuid(),
                RecommendationLink = recommendationLink,
                RecommendationTitle = recommendationTitle,
                ImageLink = imageLink,
                ImageTitle = imageTitle,
                CreatedAt = now,
                StartTime = startTime,
                EndTime = endTime,
                Position = position,
                Email = email,
                NotificationIsSent = notificationIsSent,
                IsBanner = isBanner
            };

            return recommendation;
        }

        public ICommingEvent CreateCommingEvent(string eventUserEmail, string eventLink, string eventName, DateTime eventTime, string eventPlace, string eventLead, bool isApproved)
        {
            Check.Argument.IsNotEmpty(eventLink, "EventLink");
            Check.Argument.IsNotEmpty(eventName, "EventName");

            DateTime now = SystemTime.Now();

            var commingEvent = new CommingEvent
            {
                Id = Guid.NewGuid(),
                EventLink = eventLink,
                EventName = eventName,
                EventDate = eventTime,
                EventPlace = eventPlace,
                EventLead = eventLead,
                CreatedAt = now,
                Email = eventUserEmail,
                IsApproved = isApproved
            };

            return commingEvent;
        }

        private static void PerformCheck(IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

        }

    }
}