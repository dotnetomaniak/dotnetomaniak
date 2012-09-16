namespace Kigg.DomainObjects
{
    public interface ITag : IUniqueNameEntity
    {
        string Name
        {
            get;
        }

        int StoryCount
        {
            get;
        }
    }
}