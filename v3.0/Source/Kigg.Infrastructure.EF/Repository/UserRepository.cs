using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        private readonly DotnetomaniakContext _context;

        public UserRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(IUser entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            User user = (User)entity;

            // Can't allow duplicate user name
            if (FindByUserName(user.UserName) != null)
            {
                throw new ArgumentException("\"{0}\" już istnieje. Wprowadź inną nazwę użytkownika.".FormatWith(user.UserName));
            }

            // Ensure that same email doesn't exist for non openid user
            if (!user.IsOpenIDAccount())
            {
                if (FindByEmail(user.Email) != null)
                {
                    throw new ArgumentException("\"{0}\" już istnieje. Wprowadź inny adres email.".FormatWith(user.Email));
                }
            }

            base.Add(user);
        }

        public void Remove(IUser entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            User user = (User)entity;

            _context.RemoveRange(_context.StoryViews.Where(v => v.Story.UserId == user.Id));
            _context.RemoveRange(_context.CommentSubscribtions.Where(cs => cs.UserId == user.Id || cs.Story.UserId == user.Id));
            _context.RemoveRange(_context.StoryComments.Where(c => c.UserId == user.Id || c.Story.UserId == user.Id));
            _context.RemoveRange(_context.StoryVotes.Where(v => v.UserId == user.Id || v.Story.UserId == user.Id));
            _context.RemoveRange(_context.StoryMarkAsSpams.Where(s => s.UserId == user.Id || s.Story.UserId == user.Id));
            _context.RemoveRange(_context.StoryTags.Where(st => st.Story.UserId == user.Id));
            _context.RemoveRange(_context.Stories.Where(s => s.UserId == user.Id));
            _context.RemoveRange(_context.UserTags.Where(ut => ut.UserId == user.Id));

            base.Remove(user);
        }

        public virtual IUser FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return _context.Users.SingleOrDefault(u => u.Id == id);
        }

        public virtual IUser FindByFbId(string fbId)
        {
            Check.Argument.IsNotEmpty(fbId, "fbId");

            return _context.Users.SingleOrDefault(x => x.FbId == fbId);
        }

        public virtual IUser FindByUserName(string userName)
        {
            Check.Argument.IsNotEmpty(userName, "userName");

            return _context.Users.SingleOrDefault(u => u.UserName == userName.Trim());
        }

        public virtual IUser FindByEmail(string email)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");

            return _context.Users.FirstOrDefault(u => u.Email == email.ToLowerInvariant());
        }

        public virtual decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp)
        {
            Check.Argument.IsNotEmpty(id, "id");
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            return _context.UserScores.Any(us => (us.UserId == id) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp)) ?
                   _context.UserScores.Where(us => (us.UserId == id) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp)).Sum(us => us.Score) :
                   0;
        }

        public virtual PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var userWithScore = _context.UserScores
                                        .Where(us => (us.User.Role == Roles.User) && (!us.User.IsLockedOut) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp))
                                        .GroupBy(us => us.UserId)
                                        .Select(g => new { UserId = g.Key, Total = g.Sum(us => us.Score) });

            var users = from user in _context.Users
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

            int total = _context.Users.Count(u => u.IsActive && !u.IsLockedOut && u.Role == Roles.User);

            IQueryable<User> users = _context.Users
                                             .Where(u => u.IsActive && !u.IsLockedOut && u.Role == Roles.User)
                                             .OrderBy(u => u.UserName)
                                             .ThenByDescending(u => u.LastActivityAt);

            return BuildPagedResult<IUser>(users.Skip(start).Take(max), total);
        }

        public virtual ICollection<string> FindIPAddresses(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            IQueryable<string> storyIps = _context.Stories
                                                  .Where(s => s.UserId == id)
                                                  .Select(s => s.IPAddress);

            IQueryable<string> voteIps = _context.StoryVotes
                                                 .Where(v => v.UserId == id)
                                                 .Select(v => v.IPAddress);

            IQueryable<string> commentIps = _context.StoryComments
                                                    .Where(c => c.UserId == id)
                                                    .Select(c => c.IPAddress);

            IQueryable<string> markAsSpamsIps = _context.StoryMarkAsSpams
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

        public IQueryable<IUser> FindAllThatMatches(Expression<Func<IUser, bool>> rule)
        {
            Check.Argument.IsNotNull(rule, "rule");

            var merger = new ExpressionMemberMerger();

            Expression<Func<User, IUser>> mem = m => (IUser)m;

            var expression = (Expression<Func<User, bool>>)merger.Visit(rule, mem);

            return _context.Users.Where(expression).Cast<IUser>();
        }
    }

    public class ExpressionMemberMerger : ExpressionVisitor
    {
        UnaryExpression _mem;
        ParameterExpression _paramToReplace;

        public Expression Visit<TMember, TParamType>(
            Expression<Func<TParamType, bool>> exp,
            Expression<Func<TMember, TParamType>> mem)
        {
            //get member expression
            _mem = (UnaryExpression)mem.Body;

            //get parameter in exp to replace
            _paramToReplace = exp.Parameters[0];

            //replace TParamType with TMember.Param
            var newExpressionBody = Visit(exp.Body);

            //create lambda
            return Expression.Lambda(newExpressionBody, mem.Parameters[0]);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            return p == _paramToReplace ? _mem : base.VisitParameter(p);
        }
    }
}