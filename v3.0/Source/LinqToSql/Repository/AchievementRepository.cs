using System;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.LinqToSql.DomainObjects;
using Kigg.Repository;

namespace Kigg.LinqToSql.Repository
{
    public class AchievementRepository : BaseRepository<IUserAchievement,UserAchievement>, IAchievementRepository
    {
        public AchievementRepository(IDatabase database) : base(database)
        { }

        public AchievementRepository(IDatabaseFactory factory) : base(factory)
        { }

        public PagedResult<IAchievement> GetAll()
        {
            return BuildPagedResult<IAchievement>(Database.AchievementsDataSource, int.MaxValue);
        }

        public void Award(Guid id, IQueryable<IUser> nominatedUsers)
        {
            Array.ForEach(nominatedUsers.ToArray(), user =>
                                                        {
                                                            if (user.Achievements.Result.Count(a=>a.Achievement.Id == id) > 0)
                                                                return;

                                                            var uAchievement = new UserAchievement
                                                                                  {                                                                                      
                                                                                      DateAchieved = DateTime.UtcNow,
                                                                                      Displayed = false,
                                                                                      User = (User) user, 
                                                                                      Achievement = Database.AchievementsDataSource.First(a => a.Id == id)
                                                                                  };                                                            

                                                            Add((IUserAchievement)uAchievement);
                                                        });

            Database.SubmitChanges();
        }

        public int AwardedHowManyTimes(IAchievement achievement)
        {
            return Database.UserAchievementsDataSource.Where(x => x.AchievementId == achievement.Id).Count();
        }

        public bool HasFlag(Guid flagUuid, IUser user)
        {
            Check.Argument.IsNotNull(user, nameof(user));
            Check.Argument.IsNotEmpty(flagUuid, nameof(flagUuid));

            return Database.UserAchievementsDataSource.Any(x => x.UserId == user.Id && x.AchievementId == flagUuid);
        }
    }
}
