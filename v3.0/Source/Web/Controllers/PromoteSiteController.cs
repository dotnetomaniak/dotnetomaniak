namespace Kigg.Web.Controllers
{
    using System.Web.Mvc;
    using Core.Repository;

    public class PromoteSiteController : BaseController
    {
        private readonly IPromoteSiteRepository repository;
        public PromoteSiteController(IPromoteSiteRepository promoteItemRepository)
        {
            Check.Argument.IsNotNull(promoteItemRepository, "promtoeItemRepository");
            repository = promoteItemRepository;
        }

        [Compress]
        public ActionResult List()
        {
            var viewData = new PromoteSiteViewData
                               {
                                   SiteTitle = "{0} - Promocja".FormatWith(Settings.SiteTitle),
                                   Items = repository.FindAll()
                               };
            return View(viewData);
        }

    }
}
