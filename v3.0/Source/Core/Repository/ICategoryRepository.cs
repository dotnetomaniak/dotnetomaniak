namespace Kigg.Repository
{
    using System.Collections.Generic;

    using DomainObjects;

    public interface ICategoryRepository : IUniqueNameEntityRepository<ICategory>
    {
        ICollection<ICategory> FindAll();
    }
}