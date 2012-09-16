namespace Kigg.Repository
{
    using System;

    using DomainObjects;

    public interface IUniqueNameEntityRepository<TEntity> : IRepository<TEntity> where TEntity : IUniqueNameEntity
    {
        TEntity FindById(Guid id);

        TEntity FindByUniqueName(string uniqueName);
    }
}