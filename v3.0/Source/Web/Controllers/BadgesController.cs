namespace Kigg.Web
{
    using System.Web.Mvc;
    using Repository;
    using System.Linq;
    using System;

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

        public ActionResult Seeker()
        {
            Response.Headers.Add("X-Secret", "VWtyeXRhIGZsYWdhLiBPZHdpZWTFuiBhZHJlczogL2ZsYWctODIzODMyNEEtRjQyNS00MEM5LUFGNjQtOTY5RjVBODM5M0MzIGFieSBqxIUgemRvYnnEhyE=");
            var badgesViewData = CreateViewData<BadgesViewData>();
            return View(badgesViewData);
        }

        [Authorize]
        public ActionResult SeekerFlag()
        {
            var flagUuid = Guid.Parse("8238324A-F425-40C9-AF64-969F5A8393C3");
            var hasFlagAlready = _achievementRepository.HasFlag(flagUuid, this.CurrentUser);
            if (hasFlagAlready == false)
            {
                _achievementRepository.Award(flagUuid, new[] { this.CurrentUser }.AsQueryable());
            }
            return Redirect("/");
        }
    }
}