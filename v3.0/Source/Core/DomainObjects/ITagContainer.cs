namespace Kigg.DomainObjects
{
    using System.Collections.Generic;

    public interface ITagContainer
    {
        ICollection<ITag> Tags
        {
            get;
        }

        int TagCount
        {
            get;
        }

        void AddTag(ITag tag);

        void RemoveTag(ITag tag);

        void RemoveAllTags();

        bool ContainsTag(ITag tag);
    }
}