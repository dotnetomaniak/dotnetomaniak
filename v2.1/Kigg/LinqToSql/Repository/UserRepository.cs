namespace Kigg.Repository.LinqToSql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DomainObjects;

    public class UserRepository : BaseRepository<IUser, User>, IUserRepository
    {
        public UserRepository(IDatabase database) : base(database)
        {
        }

        public UserRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public override void Add(IUser entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            User user = (User) entity;

            // Can't allow duplicate user name
            if (FindByUserName(user.UserName) != null)
            {
                throw new ArgumentException("\"{0}\" already exits. Specifiy a diffrent user name.".FormatWith(user.UserName));
            }

            // Ensure that same email doesn't exist for non openid user
            if (!user.IsOpenIDAccount())
            {
                if (FindByEmail(user.Email) != null)
                {
                    throw new ArgumentException("\"{0}\" already exits. Specifiy a diffrent email address.".FormatWith(user.Email));
                }
            }

            base.Add(user);
        }

        public override void Remove(IUser entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            User user = (User) entity;

            Database.DeleteAll(Database.StoryViewDataSource.Where(v => v.Story.UserId == user.Id));
            Database.DeleteAll(Database.CommentSubscribtionDataSource.Where(cs => cs.UserId == user.Id || cs.Story.UserId == user.Id));
            Database.DeleteAll(Database.CommentDataSource.Where(c => c.UserId == user.Id || c.Story.UserId == user.Id));
            Database.DeleteAll(Database.VoteDataSource.Where(v => v.UserId == user.Id || v.Story.UserId == user.Id));
            Database.DeleteAll(Database.MarkAsSpamDataSource.Where(s => s.UserId == user.Id || s.Story.UserId == user.Id));
            Database.DeleteAll(Database.StoryTagDataSource.Where(st => st.Story.UserId == user.Id));
            Database.DeleteAll(Database.StoryDataSource.Where(s => s.UserId == user.Id));
            Database.DeleteAll(Database.UserTagDataSource.Where(ut => ut.UserId == user.Id));

            base.Remove(user);
        }

        public virtual IUser FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.UserDataSource.SingleOrDefault(u => u.Id == id);
        }

        public virtual IUser FindByUserName(string userName)
        {
            Check.Argument.IsNotEmpty(userName, "userName");

            return Database.UserDataSource.SingleOrDefault(u => u.UserName == userName.Trim());
        }

        public virtual IUser FindByEmail(string email)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");

            return Database.UserDataSource.FirstOrDefault(u => u.Email == email.ToLowerInvariant());
        }

        public virtual decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp)
        {
            Check.Argument.IsNotEmpty(id, "id");
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            return Database.UserScoreDataSource.Any(us => (us.UserId == id) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp)) ?
                   Database.UserScoreDataSource.Where(us => (us.UserId == id) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp)).Sum(us => us.Score) :
                   0;
        }

        public virtual PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var userWithScore = Database.UserScoreDataSource
                                        .Where(us => (us.User.Role == Roles.User) && (!us.User.IsLockedOut) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp))
                                        .GroupBy(us => us.UserId)
                                        .Select(g => new { UserId = g.Key, Total = g.Sum(us => us.Score) });

            var users = from user in Database.UserDataSource
                        join score in userWithScore
                        on user.Id equals score.UserId
                        where score.Total > 0
                        orderby score.Total descending, user.LastActivityAt descending
                        select user;

            return BuildPagedResult<IUser>(users.Skip(start).Take(max), users.Count());
        }

        public virtual PagedResult<IUser> FindAll(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.UserDataSource.Count(u => u.IsActive && !u.IsLockedOut && u.Role == Roles.User);

            IQueryable<User> users = Database.UserDataSource
                                             .Where(u => u.IsActive && !u.IsLockedOut && u.Role == Roles.User)
                                             .OrderBy(u => u.UserName)
                                             .ThenByDescending(u => u.LastActivityAt);

            return BuildPagedResult<IUser>(users.Skip(start).Take(max), total);
        }

        public virtual ICollection<string> FindIPAddresses(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            IQueryable<string> storyIps = Database.StoryDataSource
                                                  .Where(s => s.UserId == id)
                                                  .Select(s => s.IPAddress);

            IQueryable<string> voteIps = Database.VoteDataSource
                                                 .Where(v => v.UserId == id)
                                                 .Select(v => v.IPAddress);

            IQueryable<string> commentIps = Database.CommentDataSource
                                                    .Where(c => c.UserId == id)
                                                    .Select(c => c.IPAddress);

            IQueryable<string> markAsSpamsIps = Database.MarkAsSpamDataSource
                                                        .Where(s => s.UserId == id)
                                                        .Select(s => s.IPAddress);

            ICollection<string> all = storyIps.Union(voteIps)
                                              .Union(commentIps)
                                              .Union(markAsSpamsIps)
                                              .Distinct()
                                              .ToList()
                                              .AsReadOnly();

            return all;
        }
    }
}