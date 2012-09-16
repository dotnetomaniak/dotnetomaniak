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
        private readonly IStoryRepository _storyRepository;

        public SupportController(IStoryRepository storyRepository, IEmailSender emailSender)
        {
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(emailSender, "emailSender");

            _storyRepository = storyRepository;
            _emailSender = emailSender;
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
                                                                new Validation(() => string.IsNullOrEmpty(email), "Pole e-mail nie mo¿e byæ puste."),
                                                                new Validation(() => !email.IsEmail(), "Niepoprawny adres e-mail."),
                                                                new Validation(() => string.IsNullOrEmpty(name), "Nazwa nie mo¿e byæ puste."),
                                                                new Validation(() => name.Length < 4, "Nazwa nie mo¿e byæ krótsza ni¿ 4 znaki."),
                                                                new Validation(() => string.IsNullOrEmpty(message), "Wiadomoœæ nie mo¿e byæ pusta."),
                                                                new Validation(() => message.Length < 16, "Wiadomoœæ nie mo¿e byæ krótsza ni¿ 16 znaków.")
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
                viewData.ErrorMessage = "Nie masz uprawnieñ do ogl¹dania tej czêœci.";
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
            }

            return View(viewData);
        }

        private ActionResult PreparedView()
        {
            return View(CreateViewData<SupportViewData>());
        }
    }
}