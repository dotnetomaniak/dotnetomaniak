using System;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class AchievementRepository: BaseRepository<UserAchievement>, IAchievementRepository
    {
        private readonly DotnetomaniakContext _context;

        public AchievementRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public PagedResult<IAchievement> GetAll()
        {
            return BuildPagedResult<IAchievement>(_context.Achievements.ToList(), int.MaxValue);
        }

        public void Award(Guid badgeId, IQueryable<IUser> nominatedUsers)
        {
            Array.ForEach(nominatedUsers.ToArray(), user =>
            {
                if (user.Achievements.Result.Count(a => a.Achievement.Id == badgeId) > 0)
                    return;

                var uAchievement = new UserAchievement
                {
                    DateAchieved = DateTime.UtcNow,
                    Displayed = false,
                    User = (User)user,
                    Achievement = _context.Achievements.First(a => a.Id == badgeId)
                };

                Add(uAchievement);
            });

            _context.SaveChanges();
        }

        public int AwardedHowManyTimes(IAchievement achievement)
        {
            return _context.UserAchievements.Count(ua => ua.AchievementId == achievement.Id);
        }

        public bool HasFlag(Guid flagUuid, IUser currentUser)
        {
            Check.Argument.IsNotNull(currentUser, nameof(currentUser));
            Check.Argument.IsNotEmpty(flagUuid, nameof(flagUuid));
            
            return _context.UserAchievements.Any(x => x.UserId == currentUser.Id && x.AchievementId == flagUuid);
        }
    }
}