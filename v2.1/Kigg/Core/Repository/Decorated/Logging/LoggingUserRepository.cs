namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Infrastructure;

    public class LoggingUserRepository : DecoratedUserRepository
    {
        public LoggingUserRepository(IUserRepository innerRepository) : base(innerRepository)
        {
        }

        public override void Add(IUser entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Info("Adding user: {0}, {1}", entity.Id, entity.UserName);
            base.Add(entity);
            Log.Info("User added: {0}, {1}", entity.Id, entity.UserName);
        }

        public override void Remove(IUser entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Warning("Removing user: {0}, {1}", entity.Id, entity.UserName);
            base.Remove(entity);
            Log.Warning("User removed: {0}, {1}", entity.Id, entity.UserName);
        }

        public override IUser FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            Log.Info("Retrieving user with id: {0}", id);

            var result = base.FindById(id);

            if (result == null)
            {
                Log.Warning("Did not find any user with id: {0}", id);
            }
            else
            {
                Log.Info("User retrieved with id: {0}", id);
            }

            return result;
        }

        public override IUser FindByUserName(string userName)
        {
            Check.Argument.IsNotEmpty(userName, "userName");

            Log.Info("Retrieving user with userName: {0}", userName);

            var result = base.FindByUserName(userName);

            if (result == null)
            {
                Log.Warning("Did not find any user with userName: {0}", userName);
            }
            else
            {
                Log.Info("User retrieved with userName: {0}", userName);
            }

            return result;
        }

        public override IUser FindByEmail(string email)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");

            Log.Info("Retrieving user with email: {0}", email);

            var result = base.FindByEmail(email);

            if (result == null)
            {
                Log.Warning("Did not find any user with email: {0}", email);
            }
            else
            {
                Log.Info("User retrieved with email: {0}", email);
            }

            return result;
        }

        public override decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp)
        {
            Check.Argument.IsNotEmpty(id, "id");
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            Log.Info("Retrieving score of user: {0}, {1}, {2}", id, startTimestamp, endTimestamp);

            var result = base.FindScoreById(id, startTimestamp, endTimestamp);

            Log.Info("Score retrieved of user: {0}, {1}, {2}", id, startTimestamp, endTimestamp);

            return result;
        }

        public override PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving top users : {0}, {1}, {2}, {3}", startTimestamp, endTimestamp, start, max);

            var pagedResult = base.FindTop(startTimestamp, endTimestamp, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any top user : {0}, {1}, {2}, {3}", startTimestamp, endTimestamp, start, max);
            }
            else
            {
                Log.Info("Top users retrieved : {0}, {1}, {2}, {3}", startTimestamp, endTimestamp, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<IUser> FindAll(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving all users : {0}, {1}", start, max);

            var pagedResult = base.FindAll(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any user : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("Top users retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override ICollection<string> FindIPAddresses(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            Log.Info("Retrieving ip addresses of user : {0}", id);

            var result = base.FindIPAddresses(id);

            if (result.IsNullOrEmpty())
            {
                Log.Warning("Did not find any ip address of user : {0}", id);
            }
            else
            {
                Log.Info("Ip addresses retrieved of user : {0}", id);
            }

            return result;
        }
    }
}