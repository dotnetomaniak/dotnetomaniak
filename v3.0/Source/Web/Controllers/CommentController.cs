namespace Kigg.Web
{
    using hbehr.recaptcha;
    using System;
    using System.Web.Mvc;

    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Service;

    [Compress]
    public class CommentController : BaseController
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryService _storyService;
        public Func<string, bool> CaptchaValidatorFunc = s => ReCaptcha.ValidateCaptcha(s);

        public CommentController(IStoryRepository storyRepository, IStoryService storyService)
        {
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(storyService, "storyService");

            _storyRepository = storyRepository;
            _storyService = storyService;
        }

        public reCAPTCHAValidator CaptchaValidator
        {
            get;
            set;
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Post(string id, string body, bool? subscribe)
        {
            id = id.NullSafe();
            body = body.NullSafe();

            string captchaChallenge = null;
            string captchaResponse = null;
            bool captchaEnabled = !CurrentUser.ShouldHideCaptcha();

            var validCaptcha = true;
            string userResponse = null;
            if (captchaEnabled)
            {
                //captchaChallenge = HttpContext.Request.Form[CaptchaValidator.ChallengeInputName];
                //captchaResponse = HttpContext.Request.Form[CaptchaValidator.ResponseInputName];
                userResponse = HttpContext.Request.Params["g-recaptcha-response"];
                validCaptcha = CaptchaValidatorFunc(userResponse);
            }

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artykułu nie może być pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artykułu."),
                                                            new Validation(() => string.IsNullOrEmpty(body.NullSafe()), "Komentarz nie może być pusty."),
                                                            new Validation(() => captchaEnabled && string.IsNullOrEmpty(userResponse),
                                                                "Pole Captcha nie może być puste."),
                                                            new Validation(() => captchaEnabled && !validCaptcha, "Weryfikacja Captcha nieudana."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteś zalogowany.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artykuł nie istnieje." };
                    }
                    else
                    {
                        CommentCreateResult result = _storyService.Comment(
                                                                            story,
                                                                            string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })),
                                                                            CurrentUser,
                                                                            body,
                                                                            subscribe ?? false,
                                                                            CurrentUserIPAddress,
                                                                            HttpContext.Request.UserAgent,
                                                                            ((HttpContext.Request.UrlReferrer != null) ? HttpContext.Request.UrlReferrer.ToString() : null),
                                                                            HttpContext.Request.ServerVariables
                                                                         );

                        viewData = string.IsNullOrEmpty(result.ErrorMessage) ? new JsonCreateViewData { isSuccessful = true } : new JsonViewData { errorMessage = result.ErrorMessage };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("dodawania komentarza.") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ConfirmSpam(string storyId, string commentId)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(storyId), "Identyfikator artykułu nie może być pusty."),
                                                            new Validation(() => storyId.ToGuid().IsEmpty(), "Niepoprawny identyfikator artykułu."),
                                                            new Validation(() => string.IsNullOrEmpty(commentId), "Identyfikator komentarza nie może być pusty."),
                                                            new Validation(() => commentId.ToGuid().IsEmpty(), "Niepoprawny identyfikator komentarza."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteś zalogowany."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz uprawnień do wołania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(storyId.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artykuł nie istnieje." };
                    }
                    else
                    {
                        IComment comment = story.FindComment(commentId.ToGuid());

                        if (comment == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Podany komentarz nie istnieje." };
                        }
                        else
                        {
                            _storyService.Spam(comment, string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })), CurrentUser);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("zaznaczania komentarza jako spam") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MarkAsOffended(string storyId, string commentId)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(storyId), "Identyfikator artykułu nie może być pusty."),
                                                            new Validation(() => storyId.ToGuid().IsEmpty(), "Niepoprawny identyfikator artykułu."),
                                                            new Validation(() => string.IsNullOrEmpty(commentId), "Identyfikator komentarza nie może być pusty."),
                                                            new Validation(() => commentId.ToGuid().IsEmpty(), "Niepoprawny identyfikator komentarza."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteś zalogowany."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz uprawnień do wywoływania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(storyId.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artykuł nie istnieje." };
                    }
                    else
                    {
                        IComment comment = story.FindComment(commentId.ToGuid());

                        if (comment == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Podany komentarz nie istnieje." };
                        }
                        else
                        {
                            _storyService.MarkAsOffended(comment, string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })), CurrentUser);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("zaznaczania komentarza jako obraŸliwy") };
                }
            }

            return Json(viewData);
        }
    }
}