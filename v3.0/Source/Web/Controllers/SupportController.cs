using System;

namespace Kigg.Web
{
    using System.Web.Mvc;
    using Repository;

    using DomainObjects;
    using Infrastructure;
    using System.Collections.Generic;

    public class SupportController : BaseController
    {
        private readonly IEmailSender _emailSender;
        private readonly ICommingEventRepository _upcommingEventsReposiotory;
        private readonly IStoryRepository _storyRepository;

        public SupportController(IStoryRepository storyRepository, IEmailSender emailSender, ICommingEventRepository upcommingEventsReposiotory)
        {
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(emailSender, "emailSender");
            Check.Argument.IsNotNull(upcommingEventsReposiotory, "upcommingEventsRepository");

            _storyRepository = storyRepository;
            _emailSender = emailSender;
            _upcommingEventsReposiotory = upcommingEventsReposiotory;
        }

        [Compress]
        public ActionResult Faq()
        {
            return PreparedView();
        }

        [Compress]
        public ActionResult PromoteSite()
        {
            var viewData = CreateViewData<PromoteSiteViewData>();
            viewData.Items = new Dictionary<string, List<PromoteItem>>
                                 {
                                     {
                                         "Logo",
                                         new List<PromoteItem>
                                             {
                                                 new PromoteItem {Url = Url.Image("dotnetomaniak_logo - negatyw_small.png"),
                                                                  Jpg = Url.Image("dotnetomaniak_logo - negatyw.jpg"),
                                                                  Png = Url.Image("dotnetomaniak_logo - negatyw.png"),
                                                                  Pdf = Url.Image("dotnetomaniak_logo - negatyw.pdf"),
                                                                  Eps = Url.Image("dotnetomaniak_logo - negatyw.eps")},
                                                 new PromoteItem {Url = Url.Image("dotnetomaniak_logo_small.png"),
                                                                  Jpg = Url.Image("dotnetomaniak_logo.jpg"),
                                                                  Png = Url.Image("dotnetomaniak_logo.png"),
                                                                  Pdf = Url.Image("dotnetomaniak_logo.pdf"),
                                                                  Eps = Url.Image("dotnetomaniak_logo.eps")},
                                             }
                                         },
                                     {
                                         "Wlepki",
                                         new List<PromoteItem>()
                                             {
                                                 new PromoteItem { Url = Url.Image("dotnetomaniak_wlepka - kontra_small.png"),
                                                                  Jpg = Url.Image("dotnetomaniak_wlepka - kontra.jpg"),
                                                                  Png = Url.Image("dotnetomaniak_wlepka - kontra.png"),
                                                                  Pdf = Url.Image("dotnetomaniak_wlepka - kontra.pdf"),
                                                                  Eps = Url.Image("dotnetomaniak_wlepka - kontra.eps")
                                                 },
                                                 new PromoteItem { Url = Url.Image("dotnetomaniak_wlepka_small.png"),
                                                                  Jpg = Url.Image("dotnetomaniak_wlepka.jpg"),
                                                                  Png = Url.Image("dotnetomaniak_wlepka.png"),
                                                                  Pdf = Url.Image("dotnetomaniak_wlepka.pdf"),
                                                                  Eps = Url.Image("dotnetomaniak_wlepka.eps")}
                                             }
                                         }
                                 };                                
                                  
            return View(viewData);
        }

        [AcceptVerbs(HttpVerbs.Get), Compress]
        public ActionResult Contact()
        {
            return PreparedView();
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Contact(string email, string name, string message)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(email), "Pole e-mail nie może być puste."),
                                                                new Validation(() => !email.IsEmail(), "Niepoprawny adres e-mail."),
                                                                new Validation(() => string.IsNullOrEmpty(name), "Nazwa nie może być pusta."),
                                                                new Validation(() => name.Length < 4, "Nazwa nie może być krótsza niż 4 znaki."),
                                                                new Validation(() => string.IsNullOrEmpty(message), "Wiadomość nie może być pusta."),
                                                                new Validation(() => message.Length < 16, "Wiadomość nie może być krótsza niż 16 znaków.")
                                                          );

            if (viewData == null)
            {
                _emailSender.NotifyFeedback(email, name, message);
                viewData = new JsonViewData { isSuccessful = true };
            }

            return Json(viewData);
        }

        [Compress]
        public ActionResult About()
        {
            return PreparedView();
        }

        public ActionResult ControlPanel()
        {
            ControlPanelViewData viewData = new ControlPanelViewData();

            if (!IsCurrentUserAuthenticated || !CurrentUser.CanModerate())
            {
                viewData.ErrorMessage = "Nie masz uprawnień do oglądania tej części strony.";
            }
            else
            {
                viewData.IsAdministrator = CurrentUser.IsAdministrator();
                viewData.NewCount = _storyRepository.CountByNew();
                viewData.UnapprovedCount = _storyRepository.CountByUnapproved();

                DateTime currentTime = SystemTime.Now();
                DateTime minimumDate = currentTime.AddHours(-Settings.MaximumAgeOfStoryInHoursToPublish);
                DateTime maximumDate = currentTime.AddHours(-Settings.MinimumAgeOfStoryInHoursToPublish);

                viewData.PublishableCount = _storyRepository.CountByPublishable(minimumDate, maximumDate);
                viewData.UnapprovedEventsCount = _upcommingEventsReposiotory.CountByUnapproved();
            }

            return View(viewData);
        }

        private ActionResult PreparedView()
        {
            return View(CreateViewData<SupportViewData>());
        }
    }
}