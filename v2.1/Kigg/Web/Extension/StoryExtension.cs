namespace Kigg.Web
{
    using DomainObjects;

    public static class StoryExtension
    {
        public static string StrippedDescription(this IStory story)
        {
            return story.TextDescription.WrapAt(512);
        }
    }
}