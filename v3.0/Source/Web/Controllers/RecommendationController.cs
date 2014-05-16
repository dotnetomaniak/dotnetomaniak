using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Kigg.Core.DomainObjects;
using Kigg.DomainObjects;
using Kigg.Infrastructure;
using Kigg.Repository;
using Kigg.Web.ViewData;
using UnitOfWork = Kigg.Infrastructure.UnitOfWork;
using System.Collections.Generic;

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
        public ActionResult EditAd(string id, string recommendationLink, string recommendationTitle, string imageLink,
            string imageTitle, DateTime startTime, DateTime endTime, string email, int position = 999, bool notificationIsSent = false)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                new Validation(() => CurrentUser.CanModerate() == false, "Nie masz praw do wykonowania tej operacji."),
                new Validation(() => string.IsNullOrEmpty(recommendationLink.NullSafe()), "Link rekomendacji nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(recommendationTitle.NullSafe()), "Tytuł rekomendacji nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(imageLink.NullSafe()), "Link obrazka nie może być pusty."),
                new Validation(() => string.IsNullOrEmpty(imageTitle.NullSafe()), "Tytuł obrazka nie może być pusty."),
                new Validation(() => startTime >= endTime, "Data zakończenia reklamy musi być większa od daty początkowej"),
                new Validation(() => string.IsNullOrEmpty(email), "Adres e-mail nie może być pusty."),
                new Validation(() => !email.NullSafe().IsEmail(), "Niepoprawny adres e-mail.")
                );
                        
            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        if (id == null || id.IsEmpty())
                        {
                            IRecommendation recommendation = _factory.CreateRecommendation(recommendationLink.Trim(),
                                recommendationTitle.Trim(), imageLink.Trim(), imageTitle.Trim(), startTime, endTime, email, position, notificationIsSent);
                            _recommendationRepository.Add(recommendation);

                            unitOfWork.Commit();

                            Log.Info("Recommendation registered: {0}", recommendation.RecommendationTitle);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                        else
                        {
                            IRecommendation recommendation = _recommendationRepository.FindById(id.ToGuid());

                            if (recommendation == null)
                            {
                                viewData = new JsonViewData { errorMessage = "Podana reklama nie istnieje." };
                            }
                            else
                            {
                                _recommendationRepository.EditAd(recommendation, recommendationLink.NullSafe(), recommendationTitle.NullSafe(), imageLink.NullSafe(), imageTitle.NullSafe(), startTime,
                                    endTime, email, position, notificationIsSent);

                                viewData = new JsonViewData { isSuccessful = true };
                            }
                        }
                    }
                }
                catch (ArgumentException argument)
                {
                    viewData = new JsonViewData { errorMessage = argument.Message };
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
            IQueryable<IRecommendation> recommendations = _recommendationRepository.GetAllVisible();
            var viewModel = CreateViewData<RecommendationsViewData>();
            viewModel.Recommendations = recommendations.Select(x=>CreateRecommendationViewData(x));
            int defaultAds = 3 - recommendations.Count();
            if (defaultAds > 0)
            {
                recommendations = _recommendationRepository.GetAllDefault(defaultAds);
                viewModel.Recommendations = viewModel.Recommendations.Union(recommendations.Select(x => CreateRecommendationViewData(x)));
            }
            return View("RecommendationsBox", viewModel);
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
                viewModel.Recommendations = recommendations.Select(x => CreateRecommendationViewData(x));
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
