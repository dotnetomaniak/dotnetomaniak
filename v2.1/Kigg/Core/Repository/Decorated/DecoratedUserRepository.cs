namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public abstract class DecoratedUserRepository : IUserRepository
    {
        private readonly IUserRepository _innerRepository;

        protected DecoratedUserRepository(IUserRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        public virtual void Add(IUser entity)
        {
            _innerRepository.Add(entity);
        }

        public virtual void Remove(IUser entity)
        {
            _innerRepository.Remove(entity);
        }

        public virtual IUser FindById(Guid id)
        {
            return _innerRepository.FindById(id);
        }

        public virtual IUser FindByUserName(string userName)
        {
            return _innerRepository.FindByUserName(userName);
        }

        public virtual IUser FindByEmail(string email)
        {
            return _innerRepository.FindByEmail(email);
        }

        public virtual decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp)
        {
            return _innerRepository.FindScoreById(id, startTimestamp, endTimestamp);
        }

        public virtual PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
        {
            return _innerRepository.FindTop(startTimestamp, endTimestamp, start, max);
        }

        public virtual PagedResult<IUser> FindAll(int start, int max)
        {
            return _innerRepository.FindAll(start, max);
        }

        public virtual ICollection<string> FindIPAddresses(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return _innerRepository.FindIPAddresses(id);
        }
    }
}