namespace Kigg.Repository
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using DomainObjects;

    public interface IUserRepository : IRepository<IUser>
    {
        IUser FindById(Guid id);

        IUser FindByFbId(string fbId);

        IUser FindByUserName(string userName);

        IUser FindByEmail(string email);

        decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp);

        PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max);

        PagedResult<IUser> FindAll(int start, int max);

        ICollection<string> FindIPAddresses(Guid id);

        IQueryable<IUser> FindAllThatMatches(Expression<Func<IUser, bool>> rule);
    }
}