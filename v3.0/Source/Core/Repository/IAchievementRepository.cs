using System;
using System.Linq;

namespace Kigg.Repository
{
    using DomainObjects;

    public interface IAchievementRepository
    {
        PagedResult<IAchievement> GetAll();

        void Award(Guid badgeId, IQueryable<IUser> nominatedUsers);
        int AwardedHowManyTimes(IAchievement achievement);
    }
}