using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using Kigg.Core.DomainObjects;
using Kigg.DomainObjects;
using Kigg.Infrastructure;
using Kigg.Repository;
using Kigg.Web.ViewData;
using UnitOfWork = Kigg.Infrastructure.UnitOfWork;

namespace Kigg.Web
{
    public class RecommendationController : BaseController
    {
        private readonly IDomainObjectFactory _factory;
        private readonly IRecommendationRepository _recommendationRepository;
        private readonly IEmailSender _emailSender;

        public RecommendationController(IDomainObjectFactory factory, IRecommendationRepository recommendationRepository, IEmailSender emailSender)
        {
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(recommendationRepository, "recommendationRepository");
            Check.Argument.IsNotNull(emailSender, "emailSender");
            
            _factory = factory;
            _recommendationRepository = recommendationRepository;
            _emailSender = emailSender;
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetAd(string id)
        {            
            id = id.NullSafe();
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator reklamy nie może być pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny reklamy artykułu."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteś zalogowany."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz praw do woływania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IRecommendation recommendation = _recommendationRepository.FindById(id.ToGuid());

                    if (recommendation == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podana rekalama nie istnieje." };
                    }
                    else
                    {
                        return Json(
                                        new
                                        {
                                            id = recommendation.Id.Shrink(),
                                            recommendationLink = recommendation.RecommendationLink,
                                            recommendationTitle = recommendation.RecommendationTitle,
                                            imageLink = recommendation.ImageLink,
                                            imageTitle = recommendation.ImageTitle,
                                            startTime = recommendation.StartTime.ToString("yyyy-MM-dd"),
                                            endTime = recommendation.EndTime.ToString("yyyy-MM-dd"),
                                            position = recommendation.Position,
                                            email = recommendation.Email,
                                            notificationIsSent = recommendation.NotificationIsSent,
                                            isBanner = recommendation.IsBanner
                                        }
                                    );
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("pobierania reklamy") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Compress]
        public ActionResult EditAd(AdvertiseViewData model)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                new Validation(() => CurrentUser.CanModerate() == false, "Nie masz praw do wykonowania tej operacji."),
                new Validation(() => string.IsNullOrEmpty(model.RecommendationLink.NullSafe()), "Link rekomendacji nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(model.RecommendationTitle.NullSafe()), "Tytuł rekomendacji nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(model.ImageLink.NullSafe()), "Link obrazka nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(model.ImageTitle.NullSafe()), "Tytuł obrazka nie może być pusty."),
                new Validation(() => model.StartTime >= model.EndTime, "Data zakończenia reklamy musi być większa od daty początkowej"),
                new Validation(() => string.IsNullOrEmpty(model.Email), "Adres e-mail nie może być pusty."),
                new Validation(() => !model.Email.NullSafe().IsEmail(), "Niepoprawny adres e-mail.")
                );

            var bannerType = string.IsNullOrWhiteSpace(model.IsBanner) == false;
            if (viewData == null)
            {
                try
                {
                    if (bannerType)
                    {
                        var request = WebRequest.Create(Server.MapPath(string.Format("/Assets/Images/{0}", model.ImageLink)));
                        var response = request.GetResponse();
                        var image = Image.FromStream(response.GetResponseStream());

                        if (image.Width != 960)
                        {
                            viewData = new JsonViewData {errorMessage = string.Format("Oczekiwana szerokość banera to 960px, twoja to: {0}", image.Width)};
                            return Json(viewData);
                        }
                    }
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        if (model.Id == null || model.Id.IsEmpty())
                        {
                            IRecommendation recommendation = _factory.CreateRecommendation(
                                model.RecommendationLink.Trim(),
                                model.RecommendationTitle.Trim(),
                                model.ImageLink.Trim(),
                                model.ImageTitle.Trim(),
                                model.StartTime,
                                model.EndTime,
                                model.Email,
                                model.Position,
                                model.NotificationIsSent,
                                bannerType);
                            _recommendationRepository.Add(recommendation);

                            unitOfWork.Commit();

                            Log.Info("Recommendation registered: {0}", recommendation.RecommendationTitle);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                        else
                        {
                            IRecommendation recommendation = _recommendationRepository.FindById(model.Id.ToGuid());

                            if (recommendation == null)
                            {
                                viewData = new JsonViewData { errorMessage = "Podana reklama nie istnieje." };
                            }
                            else
                            {
                                _recommendationRepository.EditAd(
                                    recommendation,
                                    model.RecommendationLink.NullSafe(),
                                    model.RecommendationTitle.NullSafe(),
                                    model.ImageLink.NullSafe(),
                                    model.ImageTitle.NullSafe(),
                                    model.StartTime,
                                    model.EndTime,
                                    model.Email,
                                    model.Position,
                                    model.NotificationIsSent,
                                    bannerType);

                                unitOfWork.Commit();

                                viewData = new JsonViewData { isSuccessful = true };
                            }
                        }
                    }
                }
                catch (ArgumentException argument)
                {
                    viewData = new JsonViewData { errorMessage = argument.Message };
                }
                catch (WebException e)
                {
                    viewData = new JsonViewData { errorMessage = "Podany link do zdjęcia jest nieprawidłowy" };
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("") };
                }
            }

            return Json(viewData);
        }

        public ViewResult Ads()
        {
            IQueryable<IRecommendation> recommendations = _recommendationRepository.GetAllVisible().Where(x=>x.IsBanner == false);
            var viewModel = CreateViewData<RecommendationsViewData>();
            viewModel.Recommendations = recommendations.Select(x=>CreateRecommendationViewData(x)).ToList();
            int defaultAds = 3 - recommendations.Count();
            if (defaultAds > 0)
            {
                recommendations = _recommendationRepository.GetAllDefault(defaultAds);
                viewModel.Recommendations = viewModel.Recommendations.Union(recommendations.Select(x => CreateRecommendationViewData(x))).ToList();
            }
            return View("RecommendationsBox", viewModel);
        }

        public ViewResult Banner()
        {
            IQueryable<IRecommendation> recommendations = _recommendationRepository.GetAllVisible().Where(x => x.IsBanner);
            var viewModel = CreateViewData<RecommendationsViewData>();
            viewModel.Recommendations = recommendations.Select(x => CreateRecommendationViewData(x)).ToList();
            return View("BannerBox", viewModel);
        }

        private static RecommendationViewData CreateRecommendationViewData(IRecommendation x)
        {
            return new RecommendationViewData()
            {
                UrlLink = x.RecommendationLink,
                UrlTitle = x.RecommendationTitle,
                ImageName = x.ImageLink,
                ImageAlt = x.ImageTitle,
                Position = x.Position,
                EndTime = x.EndTime,                
                Email = x.Email,
                NotificationIsSent = x.NotificationIsSent,
                IsBanner = x.IsBanner,
                Id = x.Id.Shrink()
            };
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult DeleteAd(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz praw do wołania tej metody."),
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator reklamy nie może być pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator reklamy."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteś zalogowany.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IRecommendation recommendation = _recommendationRepository.FindById(id.ToGuid());

                        if (recommendation == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Reklama nie istnieje." };
                        }
                        else
                        {
                            _recommendationRepository.Remove(recommendation);
                            unitOfWork.Commit();

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData
                    {
                        errorMessage = FormatStrings.UnknownError.FormatWith("usuwania reklamy")
                    };
                }
            }

            return Json(viewData);
        }

        [AutoRefresh, Compress]
        public ActionResult AdList()
        {
            var viewModel = CreateViewData<RecommendationsViewData>();
            if (IsCurrentUserAuthenticated && CurrentUser.CanModerate())
            {
                IQueryable<IRecommendation> recommendations = _recommendationRepository.GetAll();                
                viewModel.Recommendations = recommendations.Select(x => CreateRecommendationViewData(x)).ToList();
            }
            else
            {
                ThrowNotFound("Nie ma takiej strony");
            }
            return View("RecommendationListBox", viewModel);
        }

        public IQueryable<IRecommendation> FindRecommendetionToSendNotification(int intervalToCheckEndingRecommendationInDays)
        {
            return _recommendationRepository.FindRecommendationToSendNotification(intervalToCheckEndingRecommendationInDays);
        }

    }
}
