namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    
    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public partial class UserRepository : BaseRepository<IUser, User>, IUserRepository
    {
        public UserRepository(IDatabase database)
            : base(database)
        {
        }

        public UserRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public override void Add(IUser entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            var user = (User)entity;

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

            var user = (User)entity;

            user.RemoveAllTags();
            user.RemoveAllCommentSubscriptions();

            Database.DeleteAllOnSubmit(Database.StoryViewDataSource.Where(v => v.Story.User.Id == user.Id));
            Database.DeleteAllOnSubmit(Database.CommentDataSource.Where(c => c.User.Id == user.Id || c.Story.User.Id == user.Id));
            Database.DeleteAllOnSubmit(Database.VoteDataSource.Where(v => v.User.Id == user.Id || v.Story.User.Id == user.Id));
            Database.DeleteAllOnSubmit(Database.MarkAsSpamDataSource.Where(s => s.User.Id == user.Id || s.Story.User.Id == user.Id));

            //Convert to List immediatly to avoid issues with databases
            //that do not support MARS 
            var submittedStories = Database.StoryDataSource
                                           .Where(s => s.User.Id == user.Id)
                                           .ToList()
                                           .AsReadOnly();

            submittedStories.ForEach(s => s.RemoveAllTags());
            submittedStories.ForEach(s => s.RemoveAllCommentSubscribers());
            submittedStories.ForEach(s => Database.DeleteOnSubmit(s));

            base.Remove(user);
        }

#if(DEBUG)
        public virtual IUser FindById(Guid id)
#else
        public IUser FindById(Guid id)
#endif
        {
            Check.Argument.IsNotEmpty(id, "id");

            return DataContext!=null ? FindByIdQuery.Invoke(DataContext, id) : Database.UserDataSource.FirstOrDefault(u => u.Id == id);
        }

#if(DEBUG)
        public virtual IUser FindByUserName(string userName)
#else
        public IUser FindByUserName(string userName)
#endif
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            
            return DataContext != null ? FindByUserNameQuery.Invoke(DataContext, userName.Trim()) : Database.UserDataSource.FirstOrDefault(u => u.UserName == userName.Trim());
        }

#if(DEBUG)
        public virtual IUser FindByEmail(string email)
#else
        public IUser FindByEmail(string email)
#endif
        {
            Check.Argument.IsNotInvalidEmail(email, "email");
            var emailLowerInvariant = email.ToLowerInvariant();
            return DataContext!=null ? FindByEmailQuery.Invoke(DataContext, emailLowerInvariant) : Database.UserDataSource.FirstOrDefault(u => u.Email == emailLowerInvariant);
        }

#if(DEBUG)
        public virtual decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp)
#else
        public decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp)
#endif
        {
            Check.Argument.IsNotEmpty(id, "id");
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            var hasScore =
                Database.UserScoreDataSource.Any(
                    us => (us.User.Id == id) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp));
            
            if (hasScore)
            {
                return DataContext != null
                           ? FindScoreByIdQuery.Invoke(DataContext, id, startTimestamp, endTimestamp)
                           : Database.UserScoreDataSource.Where(us =>(us.User.Id == id) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp))
                                                         .Sum(us => us.Score);
            }
            return 0;
        }

#if(DEBUG)
        public virtual PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
#else
        public PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
#endif
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            IQueryable<User> users;
            int count;
            if(DataContext != null)
            {
                var param = new FindTopQueryParameter
                                {
                                    startTimestamp = startTimestamp,
                                    endTimestamp = endTimestamp,
                                    start = start,
                                    max = max
                                };
                users = FindTopQuery.Invoke(DataContext, param);
                count = FindTopCountQuery.Invoke(DataContext, startTimestamp, endTimestamp);
            }
            else
            {
                var userWithScore = Database.UserScoreDataSource
                                        .Where(us => (us.User.AssignedRole == (int)Roles.User) && (!us.User.IsLockedOut) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp))
                                        .GroupBy(us => us.User.Id)
                                        .Select(g => new { UserId = g.Key, Total = g.Sum(us => us.Score) });

                users = (from user in Database.UserDataSource
                        join score in userWithScore
                        on user.Id equals score.UserId
                        where score.Total > 0
                        orderby score.Total descending, user.LastActivityAt descending
                        select user);
                count = users.Count();
                users = users.Skip(start).Take(max);
            }
            
            return BuildPagedResult<IUser>(users.AsEnumerable(), count);
        }

#if(DEBUG)
        public virtual PagedResult<IUser> FindAll(int start, int max)
#else
        public PagedResult<IUser> FindAll(int start, int max)
#endif
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.UserDataSource.Count(u => u.IsActive && !u.IsLockedOut && u.AssignedRole == (int)Roles.User);
            IQueryable<User> users;
            if (DataContext != null)
            {
                users = FindAllQuery.Invoke(DataContext, start, max);
            }
            else
            {
                users = Database.UserDataSource
                    .Where(u => u.IsActive && !u.IsLockedOut && u.AssignedRole == (int) Roles.User)
                    .OrderBy(u => u.UserName)
                    .ThenByDescending(u => u.LastActivityAt);
            }
            return BuildPagedResult<IUser>(users.AsEnumerable(), total);
        }

#if(DEBUG)
        public virtual ICollection<string> FindIPAddresses(Guid id)
#else
        public ICollection<string> FindIPAddresses(Guid id)
#endif
        {
            Check.Argument.IsNotEmpty(id, "id");
            ICollection<string> all;
            if(DataContext!=null)
            {
                all = FindUserIpAddressesById(DataContext, id).ToList().AsReadOnly();
            }
            else
            {
                IQueryable<string> storyIps = Database.StoryDataSource
                                                  .Where(s => s.User.Id == id)
                                                  .Select(s => s.IpAddress);

                IQueryable<string> voteIps = Database.VoteDataSource
                                                     .Where(v => v.UserId == id)
                                                     .Select(v => v.IpAddress);

                IQueryable<string> commentIps = Database.CommentDataSource
                                                        .Where(c => c.User.Id == id)
                                                        .Select(c => c.IpAddress);

                IQueryable<string> markAsSpamsIps = Database.MarkAsSpamDataSource
                                                            .Where(s => s.UserId == id)
                                                            .Select(s => s.IpAddress);

                all = storyIps.Union(voteIps)
                              .Union(commentIps)
                              .Union(markAsSpamsIps)
                              .Distinct()
                              .ToList()
                              .AsReadOnly();
            }
            

            return all;
        }
    }
}