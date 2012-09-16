namespace Kigg.Repository
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;

    using DomainObjects;

    public abstract class DecoratedTagRepository : ITagRepository
    {
        private readonly ITagRepository _innerRepository;

        protected DecoratedTagRepository(ITagRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        [DebuggerStepThrough]
        public virtual void Add(ITag entity)
        {
            _innerRepository.Add(entity);
        }

        [DebuggerStepThrough]
        public virtual void Remove(ITag entity)
        {
            _innerRepository.Remove(entity);
        }

        [DebuggerStepThrough]
        public virtual ITag FindById(Guid id)
        {
            return _innerRepository.FindById(id);
        }

        [DebuggerStepThrough]
        public virtual ITag FindByUniqueName(string uniqueName)
        {
            return _innerRepository.FindByUniqueName(uniqueName);
        }

        [DebuggerStepThrough]
        public virtual ITag FindByName(string name)
        {
            return _innerRepository.FindByName(name);
        }

        [DebuggerStepThrough]
        public virtual ICollection<ITag> FindMatching(string name, int max)
        {
            return _innerRepository.FindMatching(name, max);
        }

        [DebuggerStepThrough]
        public virtual ICollection<ITag> FindByUsage(int top)
        {
            return _innerRepository.FindByUsage(top);
        }

        [DebuggerStepThrough]
        public virtual ICollection<ITag> FindAll()
        {
            return _innerRepository.FindAll();
        }
    }
}