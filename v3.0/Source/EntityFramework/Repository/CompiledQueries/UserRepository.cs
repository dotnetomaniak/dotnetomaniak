using Kigg.DomainObjects;

namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;
    using System.Data.Objects;

    using DomainObjects;

    public partial class UserRepository
    {
        private static readonly Func<Database, string, User>
            FindByUserNameQuery = CompiledQuery.Compile<Database, string, User>(
                (db, userName) => db.UserSet.FirstOrDefault(u => u.UserName == userName));

        private static readonly Func<Database, string, User>
            FindByEmailQuery = CompiledQuery.Compile<Database, string, User>(
                (db, email) => db.UserSet.FirstOrDefault(u => u.Email == email));

        private static readonly Func<Database, Guid, User>
            FindByIdQuery = CompiledQuery.Compile<Database, Guid, User>(
                (db, userId) => db.UserSet.FirstOrDefault(u => u.Id == userId));

        private static readonly Func<Database, Guid, DateTime, DateTime, decimal>
            FindScoreByIdQuery = CompiledQuery.Compile<Database, Guid, DateTime, DateTime, decimal>(
                (db, userId, startTimestamp, endTimestamp) => db.UserScoreSet
                                                                .Where(us => (us.User.Id == userId) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp))
                                                                .Sum(us => us.Score));

        private static readonly Func<Database, FindTopQueryParameter, IQueryable<User>>
            FindTopQuery = CompiledQuery.Compile<Database, FindTopQueryParameter, IQueryable<User>>(
               (db, param) =>
                   (from user in db.UserSet
                   join score in db.UserScoreSet
                   .Where(us => (us.User.AssignedRole == (int)Roles.User) && (!us.User.IsLockedOut) && (us.Timestamp >= param.startTimestamp && us.Timestamp <= param.endTimestamp))
                   .GroupBy(us => us.User.Id)
                   .Select(g => new { UserId = g.Key, Total = g.Sum(us => us.Score) })
                   on user.Id equals score.UserId
                   where score.Total > 0
                   orderby score.Total descending, user.LastActivityAt descending
                   select user).Skip(param.start).Take(param.max));

        private static readonly Func<Database, DateTime, DateTime, int>
            FindTopCountQuery = CompiledQuery.Compile<Database, DateTime, DateTime, int>(
               (db, startTimestamp, endTimestamp) =>
                   (from user in db.UserSet
                    join score in db.UserScoreSet
                    .Where(us => (us.User.AssignedRole == (int)Roles.User) && (!us.User.IsLockedOut) && (us.Timestamp >= startTimestamp && us.Timestamp <= endTimestamp))
                    .GroupBy(us => us.User.Id)
                    .Select(g => new { UserId = g.Key, Total = g.Sum(us => us.Score) })
                    on user.Id equals score.UserId
                    where score.Total > 0
                    orderby score.Total descending, user.LastActivityAt descending
                    select user).Count());

        private static readonly Func<Database, int, int, IQueryable<User>>
            FindAllQuery = CompiledQuery.Compile<Database, int, int, IQueryable<User>>(
                (db, start, max) => db.UserSet.Where(u => u.IsActive && !u.IsLockedOut && u.AssignedRole == (int)Roles.User)
                                      .OrderBy(u => u.UserName)
                                      .ThenByDescending(u => u.LastActivityAt)
                                      .Skip(start).Take(max));

        private static readonly Func<Database, Guid, IQueryable<string>>
            FindUserIpAddressesById = CompiledQuery.Compile<Database, Guid, IQueryable<string>>(
                (db, userId) => db.StorySet.Where(s => s.User.Id == userId).Select(s => s.IpAddress)
                                           .Union(db.VoteSet.Where(v => v.UserId == userId).Select(v => v.IpAddress))
                                           .Union(db.CommentSet.Where(c => c.User.Id == userId).Select(c => c.IpAddress))
                                           .Union(db.MarkAsSpamSet.Where(s => s.UserId == userId).Select(s => s.IpAddress))
                                           .Distinct());


        private struct FindTopQueryParameter
        {
            internal DateTime startTimestamp;
            internal DateTime endTimestamp;
            internal int start;
            internal int max;
        }
    }
}
