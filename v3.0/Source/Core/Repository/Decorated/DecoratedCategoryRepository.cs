namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using DomainObjects;

    public abstract class DecoratedCategoryRepository : ICategoryRepository
    {
        private readonly ICategoryRepository _innerRepository;

        protected DecoratedCategoryRepository(ICategoryRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        [DebuggerStepThrough]
        public virtual void Add(ICategory entity)
        {
            _innerRepository.Add(entity);
        }

        [DebuggerStepThrough]
        public virtual void Remove(ICategory entity)
        {
            _innerRepository.Remove(entity);
        }

        public void Commit()
        {
            _innerRepository.Commit();
        }

        [DebuggerStepThrough]
        public virtual ICategory FindById(Guid id)
        {
            return _innerRepository.FindById(id);
        }

        [DebuggerStepThrough]
        public virtual ICategory FindByUniqueName(string uniqueName)
        {
            return _innerRepository.FindByUniqueName(uniqueName);
        }

        [DebuggerStepThrough]
        public virtual ICollection<ICategory> FindAll()
        {
            return _innerRepository.FindAll();
        }
    }
}