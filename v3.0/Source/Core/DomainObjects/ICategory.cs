namespace Kigg.DomainObjects
{
    public interface ICategory : IUniqueNameEntity
    {
        string Name
        {
            get;
        }

        int StoryCount
        {
            get;
        }

        string Description { get; }

        bool IsActive { get; }
    }
}