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

        public static string GetSmallThumbnailPath(this IStory story, bool fullPath = false)
        {
            return ThumbnailHelper.GetThumbnailVirtualPathForStoryOrCreateNew(story.Url, story.Id.Shrink(), ThumbnailSize.Small, fullPath: fullPath);
        }

        public static string GetMediumThumbnailPath(this IStory story, bool fullPath = false)
        {
            return ThumbnailHelper.GetThumbnailVirtualPathForStoryOrCreateNew(story.Url, story.Id.Shrink(), ThumbnailSize.Medium, fullPath: fullPath, createMediumThumbnail:true);
        }
    }
}