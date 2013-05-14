using System;
using System.Web.Mvc;
using Kigg.Core.DomainObjects;
using Kigg.DomainObjects;
using Kigg.Infrastructure;
using Kigg.LinqToSql.DomainObjects;
using Kigg.LinqToSql.Repository;
using Kigg.Repository;
using Kigg.Web.ViewData;
using UnitOfWork = Kigg.Infrastructure.UnitOfWork;

namespace Kigg.Web
{
    public class RecommendationController : BaseController
    {
        private readonly IDomainObjectFactory _factory;
        private readonly IRecommendationRepository _recommendationRepository;

        public RecommendationController(IDomainObjectFactory factory, IRecommendationRepository recommendationRepository)
        {
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(recommendationRepository, "recommendationRepository");

            _factory = factory;
            _recommendationRepository = recommendationRepository;
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Recomend(string recommendationLink, string recommendationTitle, string imageLink,
            string imageTitle)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                new Validation(() => string.IsNullOrEmpty(recommendationLink.NullSafe()),
                    "Link rekomendacji nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(recommendationTitle.NullSafe()),
                    "Tytuł rekomendacji nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(imageLink.NullSafe()), "Link obrazka nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(imageTitle.NullSafe()), "Tytuł obrazka nie może być pusty.")
                );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IRecommendation recommendation = _factory.CreateRecommendation(recommendationLink.Trim(),
                            recommendationTitle.Trim(), imageLink.Trim(), imageTitle.Trim());
                        _recommendationRepository.Add(recommendation);

                        unitOfWork.Commit();

                        Log.Info("Recommendation registered: {0}", recommendation.RecommendationTitle);

                        viewData = new JsonViewData {isSuccessful = true};
                    }
                }
                catch (ArgumentException argument)
                {
                    viewData = new JsonViewData {errorMessage = argument.Message};
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData {errorMessage = FormatStrings.UnknownError.FormatWith("")};
                }
            }

            return Json(viewData);
        }

        public ViewResult PickRecommendationData(string recommendationTitle)
        {
            return View(model);
        }
    }
}
