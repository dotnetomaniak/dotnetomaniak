namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public abstract class DecoratedCategoryRepository : ICategoryRepository
    {
        private readonly ICategoryRepository _innerRepository;

        protected DecoratedCategoryRepository(ICategoryRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        public virtual void Add(ICategory entity)
        {
            _innerRepository.Add(entity);
        }

        public virtual void Remove(ICategory entity)
        {
            _innerRepository.Remove(entity);
        }

        public virtual ICategory FindById(Guid id)
        {
            return _innerRepository.FindById(id);
        }

        public virtual ICategory FindByUniqueName(string uniqueName)
        {
            return _innerRepository.FindByUniqueName(uniqueName);
        }

        public virtual ICollection<ICategory> FindAll()
        {
            return _innerRepository.FindAll();
        }
    }
}