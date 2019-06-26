using System.Linq;
using System.Linq.Expressions;

namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using DomainObjects;

    public abstract class DecoratedUserRepository : IUserRepository
    {
        private readonly IUserRepository _innerRepository;

        protected DecoratedUserRepository(IUserRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        [DebuggerStepThrough]
        public virtual void Add(IUser entity)
        {
            _innerRepository.Add(entity);
        }

        [DebuggerStepThrough]
        public virtual void Remove(IUser entity)
        {
            _innerRepository.Remove(entity);
        }

        public void Commit()
        {
            _innerRepository.Commit();
        }

        [DebuggerStepThrough]
        public virtual IUser FindById(Guid id)
        {
            return _innerRepository.FindById(id);
        }

        [DebuggerStepThrough]
        public virtual IUser FindByUserName(string userName)
        {
            return _innerRepository.FindByUserName(userName);
        }

        [DebuggerStepThrough]
        public virtual IUser FindByEmail(string email)
        {
            return _innerRepository.FindByEmail(email);
        }

        [DebuggerStepThrough]
        public virtual decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp)
        {
            return _innerRepository.FindScoreById(id, startTimestamp, endTimestamp);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
        {
            return _innerRepository.FindTop(startTimestamp, endTimestamp, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<IUser> FindAll(int start, int max)
        {
            return _innerRepository.FindAll(start, max);
        }

        [DebuggerStepThrough]
        public virtual ICollection<string> FindIPAddresses(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return _innerRepository.FindIPAddresses(id);
        }

        [DebuggerStepThrough]
        public IQueryable<IUser> FindAllThatMatches(Expression<Func<IUser, bool>> rule)
        {
            Check.Argument.IsNotNull(rule, "rule");

            return _innerRepository.FindAllThatMatches(rule);
        }


        public IUser FindByFbId(string fbId)
        {
            Check.Argument.IsNotNull(fbId, "fbId");

            return _innerRepository.FindByFbId(fbId);
        }
    }
}