namespace Kigg.Web
{
    using Kigg.DomainObjects;

    public class AchievementsViewData : BaseViewData
    {
        public int Count { get; set; }
        public PagedResult<IUserAchievement> Achievements { get; set; }
    }
}