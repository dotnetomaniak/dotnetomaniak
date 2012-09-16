namespace Kigg.DomainObjects
{
    using System;
    using System.Diagnostics;

    using Infrastructure;

    public static class StoryExtension
    {
        [DebuggerStepThrough]
        public static bool IsNew(this IStory story)
        {
            return !story.LastProcessedAt.HasValue;
        }

        [DebuggerStepThrough]
        public static bool IsPublished(this IStory story)
        {
            return story.PublishedAt.HasValue;
        }

        [DebuggerStepThrough]
        public static bool HasExpired(this IStory story)
        {
            IConfigurationSettings settings = IoC.Resolve<IConfigurationSettings>();

            DateTime max = SystemTime.Now().AddHours(-settings.MaximumAgeOfStoryInHoursToPublish);

            return story.CreatedAt <= max;
        }

        [DebuggerStepThrough]
        public static string AuthorsProfile(this IStory story)
        {
            return "11968639822986691342";
        }

        [DebuggerStepThrough]
        public static bool IsAuthorshipOn(this IStory story)
        {
            return Host(story) == "pawlos.blogspot.com";
        }

        [DebuggerStepThrough]
        public static bool IsApproved(this IStory story)
        {
            return story.ApprovedAt.HasValue;
        }

        [DebuggerStepThrough]
        public static bool HasComments(this IStory story)
        {
            return story.CommentCount > 0;
        }

        [DebuggerStepThrough]
        public static bool IsPostedBy(this IStory story, IUser byUser)
        {
            return (byUser != null) && (story.PostedBy.Id == byUser.Id);
        }

        [DebuggerStepThrough]
        public static string Host(this IStory story)
        {
            return new Uri(story.Url).Host;
        }

        [DebuggerStepThrough]
        public static string SmallThumbnail(this IStory story)
        {
            return IoC.Resolve<IThumbnail>().For(story.Url, ThumbnailSize.Small);
        }

        [DebuggerStepThrough]
        public static string MediumThumbnail(this IStory story)
        {
            return IoC.Resolve<IThumbnail>().For(story.Url, ThumbnailSize.Medium);
        }
    }
}