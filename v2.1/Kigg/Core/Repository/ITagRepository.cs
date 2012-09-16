namespace Kigg.Repository
{
    using System.Collections.Generic;

    using DomainObjects;

    public interface ITagRepository : IUniqueNameEntityRepository<ITag>
    {
        ITag FindByName(string name);

        ICollection<ITag> FindMatching(string name, int max);

        ICollection<ITag> FindByUsage(int top);

        ICollection<ITag> FindAll();
    }
}