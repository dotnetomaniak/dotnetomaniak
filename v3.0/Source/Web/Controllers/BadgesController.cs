namespace Kigg.Web
{
    using System.Web.Mvc;
    using Repository;
    using System.Linq;

    public class BadgesController : BaseController
    {
        private readonly IAchievementRepository _achievementRepository;

        public BadgesController(IAchievementRepository achievementRepository)
        {
            Check.Argument.IsNotNull(achievementRepository, "achievementRepository");

            _achievementRepository = achievementRepository;
        }

        public ActionResult All()
        {
            var all = _achievementRepository.GetAll();
            var badgesViewData = CreateViewData<BadgesViewData>();
            badgesViewData.Badges = all.Result.Select(
                x =>
                new BadgeViewData
                    {
                        Description = x.Description,
                        Name = x.Name,
                        Count = _achievementRepository.AwardedHowManyTimes(x)
                    });

            return View(badgesViewData);
        }
    }
}