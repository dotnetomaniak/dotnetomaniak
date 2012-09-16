namespace Kigg.DomainObjects
{
    using System;

    using Infrastructure;

    public static class StoryExtension
    {
        public static bool IsNew(this IStory story)
        {
            return !story.LastProcessedAt.HasValue;
        }

        public static bool IsPublished(this IStory story)
        {
            return story.PublishedAt.HasValue;
        }

        public static bool HasExpired(this IStory story)
        {
            IConfigurationSettings settings = IoC.Resolve<IConfigurationSettings>();

            DateTime max = SystemTime.Now().AddHours(-settings.MaximumAgeOfStoryInHoursToPublish);

            return story.CreatedAt <= max;
        }

        public static bool IsApproved(this IStory story)
        {
            return story.ApprovedAt.HasValue;
        }

        public static bool HasComments(this IStory story)
        {
            return story.CommentCount > 0;
        }

        public static bool IsPostedBy(this IStory story, IUser byUser)
        {
            return (byUser != null) && (story.PostedBy.Id == byUser.Id);
        }

        public static string Host(this IStory story)
        {
            return new Uri(story.Url).Host;
        }

        public static string SmallThumbnail(this IStory story)
        {
            return IoC.Resolve<IThumbnail>().For(story.Url, ThumbnailSize.Small);
        }

        public static string MediumThumbnail(this IStory story)
        {
            return IoC.Resolve<IThumbnail>().For(story.Url, ThumbnailSize.Medium);
        }
    }
}