using Kigg.Infrastructure;

namespace Kigg.Web
{
    using DomainObjects;

    public static class StoryExtension
    {
        public static string StrippedDescription(this IStory story)
        {
            return story.TextDescription.WrapAt(512);
        }

        public static string GetSmallThumbnailPath(this IStory story)
        {
            return ThumbnailHelper.GetThumbnailVirtualPathForStory(story.Id.Shrink(), ThumbnailSize.Small);
        }

        public static string GetMediumThumbnailPath(this IStory story)
        {
            return ThumbnailHelper.GetThumbnailVirtualPathForStory(story.Id.Shrink(), ThumbnailSize.Medium);
        }
    }
}