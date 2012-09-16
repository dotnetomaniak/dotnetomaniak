namespace Kigg.DomainObjects
{
    public static class TagContainerExtension
    {
        public static bool HasTags(this ITagContainer container)
        {
            return container.TagCount > 0;
        }
    }
}