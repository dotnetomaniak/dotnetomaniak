namespace Kigg.DomainObjects
{
    using System.Diagnostics;

    public static class TagContainerExtension
    {
        [DebuggerStepThrough]
        public static bool HasTags(this ITagContainer container)
        {
            return container.TagCount > 0;
        }
    }
}