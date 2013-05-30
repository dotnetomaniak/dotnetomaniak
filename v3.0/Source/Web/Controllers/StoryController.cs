namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DomainObjects;
    using Infrastructure;
    using Repository;
    using Service;
    using System.Xml;

    public class StoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryService _storyService;
        private readonly IContentService _contentService;
        private readonly ISocialServiceRedirector[] _socialServiceRedirectors;

        public StoryController(ICategoryRepository categoryRepository, ITagRepository tagRepository, IStoryRepository storyRepository, IStoryService storyService, IContentService contentService, ISocialServiceRedirector[] socialServiceRedirectors)
        {
            Check.Argument.IsNotNull(categoryRepository, "categoryRepository");
            Check.Argument.IsNotNull(tagRepository, "tagRepository");
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(storyService, "storyService");
            Check.Argument.IsNotNull(contentService, "contentService");
            Check.Argument.IsNotEmpty(socialServiceRedirectors, "socialServiceRedirectors");

            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _storyRepository = storyRepository;
            _storyService = storyService;
            _contentService = contentService;

            _socialServiceRedirectors = socialServiceRedirectors;
        }

        public reCAPTCHAValidator CaptchaValidator
        {
            get;
            set;
        }

        public DefaultColors CounterColors
        {
            get;
            set;
        }

        [Compress]
        public ActionResult Published(int? page)
        {
            StoryListViewData viewData = CreateStoryListViewData<StoryListViewData>(page);
            PagedResult<IStory> pagedResult = _storyRepository.FindPublished(PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

            viewData.Stories = pagedResult.Result;
            viewData.TotalStoryCount = pagedResult.Total;

            viewData.Title = "{0} - Najnowsze artyku³y o .NET".FormatWith(Settings.SiteTitle);
            if (page.HasValue && page > 1)
                viewData.Title += " - Strona {0}".FormatWith(page);

            viewData.MetaDescription = "Czytaj najnowsze artyku³y o .NET";
            if (page.HasValue && page > 1)
            {
                viewData.MetaDescription += " - Strona {0}".FormatWith(page);
            }
            viewData.RssUrl = string.IsNullOrEmpty(Settings.PublishedStoriesFeedBurnerUrl) ? Url.RouteUrl("FeedPublished") : Settings.PublishedStoriesFeedBurnerUrl;
            viewData.AtomUrl = Url.RouteUrl("FeedPublished", new { format = "Atom" });
            viewData.Subtitle = "Wszystkie";
            viewData.NoStoryExistMessage = "Brak opublikowanych artyku³ów.";

            return View("List", viewData);
        }

        [OutputCache(CacheProfile = "JsonStoryCache"), Compress]
        public ActionResult GetPublished(int? start, int? max)
        {
            PagedResult<StorySummary> summary = null;

            try
            {
                summary = ConvertToSummary(_storyRepository.FindPublished, start, max);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }

            summary = summary ?? new PagedResult<StorySummary>();

            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        public ActionResult Category(string name, int? page)
        {
            name = name.NullSafe();

            if (string.IsNullOrEmpty(name))
            {
                return RedirectToRoute("Published");
            }

            ICategory category = _categoryRepository.FindByUniqueName(name);

            if (category == null)
            {
                ThrowNotFound("Kategoria nie istnieje.");
            }

            StoryListViewData viewData = CreateStoryListViewData<StoryListViewData>(page);
            string uniqueName = name;

            if (category != null)
            {
                PagedResult<IStory> pagedResult = _storyRepository.FindPublishedByCategory(category.Id, PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);
                viewData.Stories = pagedResult.Result;
                viewData.TotalStoryCount = pagedResult.Total;

                name = category.Name;
                uniqueName = category.UniqueName;
            }

            viewData.Title = "{0} - Najnowsze artyku³y o .NET w dziale {1}".FormatWith(Settings.SiteTitle, name);
            if (page.HasValue && page > 1)
            {
                viewData.Title += " - Strona {0}".FormatWith(page);
            }

            viewData.MetaDescription = "Najnowsze artyku³y o .NET w dziale {0}".FormatWith(name);
            if (page.HasValue && page > 1)
            {
                viewData.MetaDescription += " - Strona {0}".FormatWith(page);
            }
            viewData.RssUrl = Url.Action("Category", "Feed", new { name = uniqueName });
            viewData.AtomUrl = Url.Action("Category", "Feed", new { name = uniqueName, format = "Atom" });
            viewData.Subtitle = name;
            viewData.NoStoryExistMessage = "Brak opublikowanych artyku³ów w \"{0}\".".FormatWith(name.HtmlEncode());

            return View("List", viewData);
        }

        [Compress]
        public ActionResult Upcoming(int? page)
        {
            StoryListViewData viewData = CreateStoryListViewData<StoryListViewData>(page);
            PagedResult<IStory> pagedResult = _storyRepository.FindUpcoming(PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

            viewData.Stories = pagedResult.Result;
            viewData.TotalStoryCount = pagedResult.Total;

            viewData.Title = "{0} - Nadchodz¹ce artyku³y".FormatWith(Settings.SiteTitle);
            viewData.MetaDescription = "Nadchodz¹ce artyku³y";
            viewData.RssUrl = string.IsNullOrEmpty(Settings.UpcomingStoriesFeedBurnerUrl) ? Url.RouteUrl("FeedUpcoming") : Settings.UpcomingStoriesFeedBurnerUrl;
            viewData.AtomUrl = Url.RouteUrl("FeedUpcoming", new { format = "Atom" });
            viewData.Subtitle = "Nadchodz¹ce artyku³y";
            viewData.NoStoryExistMessage = "Brak nadchodz¹cych artyku³ów.";

            return View("List", viewData);
        }

        [OutputCache(CacheProfile = "JsonStoryCache"), Compress]
        public ActionResult GetUpcoming(int? start, int? max)
        {
            PagedResult<StorySummary> summary = null;

            try
            {
                summary = ConvertToSummary(_storyRepository.FindUpcoming, start, max);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }

            summary = summary ?? new PagedResult<StorySummary>();

            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        public ActionResult New(int? page)
        {
            StoryListViewData viewData = CreateStoryListViewData<StoryListViewData>(page);
            viewData.Title = "{0} - Nowe artyku³y".FormatWith(Settings.SiteTitle);
            viewData.MetaDescription = "Nowe artyku³y";
            viewData.Subtitle = "Nowe";

            if (!IsCurrentUserAuthenticated || !CurrentUser.CanModerate())
            {
                viewData.NoStoryExistMessage = "Nie masz uprawnieñ do przegl¹dania nowych artyku³ów.";
            }
            else
            {
                PagedResult<IStory> pagedResult = _storyRepository.FindNew(PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

                viewData.Stories = pagedResult.Result;
                viewData.TotalStoryCount = pagedResult.Total;

                viewData.NoStoryExistMessage = "Brak nowych artyku³ów.";
            }

            return View("List", viewData);
        }

        [AutoRefresh, Compress]
        public ActionResult Unapproved(int? page)
        {
            StoryListViewData viewData = CreateStoryListViewData<StoryListViewData>(page);
            viewData.Title = "{0} - Niezatwierdzone artyku³y".FormatWith(Settings.SiteTitle);
            viewData.MetaDescription = "Niezatwierdzone artyku³y";
            viewData.Subtitle = "Niezatwierdzone";

            if (!IsCurrentUserAuthenticated || !CurrentUser.CanModerate())
            {
                viewData.NoStoryExistMessage = "Nie masz uprawnieñ do przegl¹dania niezatwierdzonych artyku³ów.";
            }
            else
            {
                PagedResult<IStory> pagedResult = _storyRepository.FindUnapproved(PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

                viewData.Stories = pagedResult.Result;
                viewData.TotalStoryCount = pagedResult.Total;

                viewData.NoStoryExistMessage = "Brak niezatwierdzonych artyku³ów.";
            }

            return View("List", viewData);
        }

        [AutoRefresh, Compress]
        public ActionResult Publishable(int? page)
        {
            StoryListViewData viewData = CreateStoryListViewData<StoryListViewData>(page);
            viewData.Title = "{0} - Do opublikowania".FormatWith(Settings.SiteTitle);
            viewData.MetaDescription = "Do opublikowania";
            viewData.Subtitle = "Do opublikowania";

            if (!IsCurrentUserAuthenticated || !CurrentUser.CanModerate())
            {
                viewData.NoStoryExistMessage = "Nie masz uprawnieñ do przegl¹dania artyku³ów do opublikowania.";
            }
            else
            {
                DateTime currentTime = SystemTime.Now();
                DateTime minimumDate = currentTime.AddHours(-Settings.MaximumAgeOfStoryInHoursToPublish);
                DateTime maximumDate = currentTime.AddHours(-Settings.MinimumAgeOfStoryInHoursToPublish);

                PagedResult<IStory> pagedResult = _storyRepository.FindPublishable(minimumDate, maximumDate, PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

                viewData.Stories = pagedResult.Result;
                viewData.TotalStoryCount = pagedResult.Total;

                viewData.NoStoryExistMessage = "Brak artyku³ów do apublikowania.";
            }

            return View("List", viewData);
        }

        [Compress]
        public ActionResult Tags(string name, int? page)
        {
            name = name.NullSafe();

            if (string.IsNullOrEmpty(name))
            {
                return RedirectToRoute("Published");
            }

            ITag tag = _tagRepository.FindByUniqueName(name);

            if (tag == null)
            {
                ThrowNotFound("Tag nie istnieje.");
            }

            StoryListViewData viewData = CreateStoryListViewData<StoryListViewData>(page);
            string uniqueName = name;

            if (tag != null)
            {
                PagedResult<IStory> pagedResult = _storyRepository.FindByTag(tag.Id, PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

                viewData.Stories = pagedResult.Result;
                viewData.TotalStoryCount = pagedResult.Total;

                name = tag.Name;
                uniqueName = tag.UniqueName;
            }

            viewData.Title = "{0} - Artyku³y z tagiem {1}".FormatWith(Settings.SiteTitle, name);
            viewData.MetaDescription = "Artyku³y z tagiem {0}".FormatWith(name);
            viewData.RssUrl = Url.Action("Tags", "Feed", new { name = uniqueName });
            viewData.AtomUrl = Url.Action("Tags", "Feed", new { name = uniqueName, format = "Atom" });
            viewData.Subtitle = name;
            viewData.NoStoryExistMessage = "Brak artyku³ów z \"{0}\".".FormatWith(name.HtmlEncode());

            return View("List", viewData);
        }

        [AutoRefresh, ValidateInput(false), Compress]
        public ActionResult Search(string q, int? page)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToRoute("Published");
            }

            StoryListSearchViewData viewData = CreateStoryListViewData<StoryListSearchViewData>(page);
            PagedResult<IStory> pagedResult = _storyRepository.Search(q, PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

            viewData.Stories = pagedResult.Result;
            viewData.TotalStoryCount = pagedResult.Total;
            viewData.Query = q;

            viewData.Title = "{0} - Wyniki wyszukiwania dla \"{1}\"".FormatWith(Settings.SiteTitle, q);
            viewData.MetaDescription = "Wyniki wyszukiwania dla {0}".FormatWith(q);
            viewData.RssUrl = Url.Action("Search", "Feed", new { q });
            viewData.AtomUrl = Url.Action("Search", "Feed", new { q, format = "Atom" });
            viewData.Subtitle = "Wyniki wyszukiwania : {0}".FormatWith(q);
            viewData.NoStoryExistMessage = "Brak artyku³ów dla \"{0}\".".FormatWith(q.HtmlEncode());

            return View("List", viewData);
        }

        [Compress]
        public ActionResult Detail(string name)
        {
            name = name.NullSafe();

            if (string.IsNullOrEmpty(name))
            {
                return RedirectToRoute("Published");
            }

            IStory story = _storyRepository.FindByUniqueName(name);

            if (story == null)
            {
                ThrowNotFound("Artyku³ nie istnieje.");
            }

            StoryDetailViewData viewData = CreateStoryViewData<StoryDetailViewData>();
            viewData.CaptchaEnabled = !CurrentUser.ShouldHideCaptcha();


            if (story != null)
            {
                viewData.Title = "{0} - {1}".FormatWith(Settings.SiteTitle, story.Title);
                viewData.MetaDescription = story.StrippedDescription();
                viewData.Story = story;
                viewData.CounterColors = CounterColors;

            }

            return View(viewData);
        }

        public ActionResult Questions(string tags)
        {
            IEnumerable<XmlNode> entries = null;
            try
            {                
                foreach (var tag in tags.Split(new[] { ',' }))
                {
                    var address = "http://devpytania.pl/szukaj?type=rss&q=[{0}]".FormatWith(tag);
                    var rssReader = new XmlTextReader(address);
                    var rssDoc = new XmlDocument();

                    rssDoc.Load(rssReader);
                    entries = rssDoc.SelectNodes("//item").Cast<XmlNode>();
                    if (entries != null && entries.Count() > 0)
                        break;
                }
            }
            catch { }

            return View(entries);
        }

        [AcceptVerbs(HttpVerbs.Get), ValidateInput(false), Compress]
        public ActionResult Submit(string url, string title)
        {
            bool isValidUrl = url.IsWebUrl();

            if (isValidUrl)
            {
                IStory story = _storyRepository.FindByUrl(url);

                if (story != null)
                {
                    // Story already exist, so take the user to that story
                    return RedirectToRoute("Detail", new { name = story.UniqueName });
                }
            }

            bool autoDiscover = Settings.AutoDiscoverContent || (IsCurrentUserAuthenticated && !CurrentUser.IsPublicUser());

            StoryContentViewData viewData = CreateViewData<StoryContentViewData>();

            viewData.Url = url;
            viewData.Title = title;
            viewData.AutoDiscover = autoDiscover;
            viewData.CaptchaEnabled = !CurrentUser.ShouldHideCaptcha();

            if (isValidUrl)
            {
                StoryContent content = _contentService.Get(url);

                if (content != StoryContent.Empty)
                {
                    // Only replace when no title is specified
                    if (string.IsNullOrEmpty(title))
                    {
                        viewData.Title = content.Title;
                    }

                    if (autoDiscover)
                    {
                        viewData.Description = content.Description;
                    }
                }
            }

            return View("New", viewData);
        }

        [OutputCache(CacheProfile = "JsonUrlCache"), Compress]
        public ActionResult Retrieve(string url)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(url), "Url nie mo¿e byæ pusty."),
                                                            new Validation(() => !url.IsWebUrl(), "Niepoprawny format Url.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindByUrl(url);

                    if (story != null)
                    {
                        string existingUrl = Url.RouteUrl("Detail", new { name = story.UniqueName });

                        viewData = new JsonContentViewData { alreadyExists = true, existingUrl = existingUrl };
                    }
                    else
                    {
                        StoryContent content = _contentService.Get(url);

                        viewData = (content == StoryContent.Empty) ?
                                    new JsonViewData { errorMessage = "Podany Url nie istnieje." } :
                                    new JsonContentViewData { isSuccessful = true, title = content.Title.HtmlDecode(), description = content.Description.HtmlDecode() };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("pobierania strony.") };
                }
            }

            return Json(viewData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Submit(string url, string title, string category, string description, string tags)
        {
            string captchaChallenge = null;
            string captchaResponse = null;
            bool captchaEnabled = !CurrentUser.ShouldHideCaptcha();

            if (captchaEnabled)
            {
                captchaChallenge = HttpContext.Request.Form[CaptchaValidator.ChallengeInputName];
                captchaResponse = HttpContext.Request.Form[CaptchaValidator.ResponseInputName];
            }

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => captchaEnabled && string.IsNullOrEmpty(captchaChallenge), "Pole Captcha nie mo¿e byæ puste."),
                                                            new Validation(() => captchaEnabled && string.IsNullOrEmpty(captchaResponse), "Pole Captcha nie mo¿e byæ puste."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany"),
                                                            new Validation(() => captchaEnabled && !CaptchaValidator.Validate(CurrentUserIPAddress, captchaChallenge, captchaResponse), "Nieudana weryfikacja Captcha")
                                                          );

            if (viewData == null)
            {
                try
                {
                    StoryCreateResult result = _storyService.Create(
                                                                        CurrentUser,
                                                                        url.NullSafe(),
                                                                        title.NullSafe(),
                                                                        category.NullSafe(),
                                                                        description.NullSafe(),
                                                                        tags.NullSafe(),
                                                                        CurrentUserIPAddress,
                                                                        HttpContext.Request.UserAgent,
                                                                        ((HttpContext.Request.UrlReferrer != null) ? HttpContext.Request.UrlReferrer.ToString() : null),
                                                                        HttpContext.Request.ServerVariables,
                                                                        story => string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName }))
                                                                    );

                    viewData = new JsonCreateViewData
                                   {
                                       isSuccessful = string.IsNullOrEmpty(result.ErrorMessage),
                                       errorMessage = result.ErrorMessage,
                                       url = result.DetailUrl
                                   };
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("dodania artyku³u") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Click(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikatory artyku³u.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        _storyService.View(story, CurrentUser, CurrentUserIPAddress);

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("klikania") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Promote(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        if (!story.CanPromote(CurrentUser))
                        {
                            viewData = story.HasPromoted(CurrentUser) ?
                                       new JsonViewData { errorMessage = "Ju¿ wypromowa³eœ ten artyku³." } :
                                       new JsonViewData { errorMessage = "Nie mo¿esz promowaæ tego artyku³u." };
                        }
                        else
                        {
                            _storyService.Promote(story, CurrentUser, CurrentUserIPAddress);

                            viewData = new JsonVoteViewData { isSuccessful = true, votes = story.VoteCount, text = GetText(story.VoteCount) };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("promowaniu artyku³u") };
                }
            }

            return Json(viewData);
        }

        private static string GetText(int count)
        {
            if (count == 1)
                return ".netomaniak";
            return count < 5 ? ".netomaniaki" : ".netomaniaków";
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Demote(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        if (!story.CanDemote(CurrentUser))
                        {
                            viewData = new JsonViewData { errorMessage = "Nie mo¿esz degradowaæ tego artyku³u." };
                        }
                        else
                        {
                            _storyService.Demote(story, CurrentUser);

                            viewData = new JsonVoteViewData { isSuccessful = true, votes = story.VoteCount, text = GetText(story.VoteCount) };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("degradowania artyku³u") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult MarkAsSpam(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        if (!story.CanMarkAsSpam(CurrentUser))
                        {
                            viewData = story.HasMarkedAsSpam(CurrentUser) ?
                                        new JsonViewData { errorMessage = "Ju¿ zaznaczy³eœ ten artyku³ jako spam." } :
                                        new JsonViewData { errorMessage = "Nie masz uprawnieñ do zaznaczania tego artyku³u jako spam." };
                        }
                        else
                        {
                            _storyService.MarkAsSpam(story, string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })), CurrentUser, CurrentUserIPAddress);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("oznaczania artyku³u jako spam") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Publish()
        {
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany."),
                                                            new Validation(() => !CurrentUser.IsAdministrator(), "Nie masz praw do wo³ywania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    _storyService.Publish();

                    viewData = new JsonViewData { isSuccessful = true };
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("publikowania artyku³u") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetStory(string id)
        {
            id = id.NullSafe();
            IStory story = _storyRepository.FindById(id.ToGuid());
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany."),
                                                            new Validation(() => !CurrentUser.HasRightsToEditStory(story), "Nie masz praw do wo³ania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        return Json(
                                        new
                                        {
                                            id = story.Id.Shrink(),
                                            name = story.UniqueName,
                                            createdAt = story.CreatedAt.ToString("G", Constants.CurrentCulture),
                                            title = story.Title,
                                            description = story.TextDescription,
                                            category = story.BelongsTo.UniqueName,
                                            tags = string.Join(", ", story.Tags.Select(t => t.Name).ToArray())
                                        }
                                    );
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("pobierania artyku³u") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Compress]
        public ActionResult Update(string id, string name, Nullable<DateTime> createdAt, string title, string category, string description, string tags)
        {
            id = id.NullSafe();
            IStory story = _storyRepository.FindById(id.ToGuid());
            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany."),
                                                            new Validation(() => !CurrentUser.HasRightsToEditStory(story), "Nie masz praw do wo³ania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {                  
                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        _storyService.Update(story, name.NullSafe(), createdAt.HasValue ? createdAt.Value.ToUniversalTime() : DateTime.MinValue, title.NullSafe(), category.NullSafe(), description.NullSafe(), tags.NullSafe());

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("edycji artyku³u") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Delete(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz praw do wo³ania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        _storyService.Delete(story, CurrentUser);

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("usuwania artyku³u") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Approve(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz praw do wo³ania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        if (story.IsApproved())
                        {
                            viewData = new JsonViewData { errorMessage = "Podany artyku³ ju¿ zosta³ zatwierdzony jako spam." };
                        }
                        else
                        {
                            _storyService.Approve(story, string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })), CurrentUser);

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("aprobowania artyku³u") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult ConfirmSpam(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Indentyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawy identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ obecnie zalogowany."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz uprawnieñ do wywo³ania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        _storyService.Spam(story, string.Concat(Settings.RootUrl, Url.RouteUrl("Detail", new { name = story.UniqueName })), CurrentUser);

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("zatwierdzania artyku³u jako spam") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult GenerateMiniature(string id)
        {
            id = id.NullSafe();

            JsonViewData viewData = Validate<JsonViewData>(
                                                            new Validation(() => string.IsNullOrEmpty(id), "Identyfikator artyku³u nie mo¿e byæ pusty."),
                                                            new Validation(() => id.ToGuid().IsEmpty(), "Niepoprawny identyfikator artyku³u."),
                                                            new Validation(() => !IsCurrentUserAuthenticated, "Nie jesteœ zalogowany."),
                                                            new Validation(() => !CurrentUser.CanModerate(), "Nie masz praw do wo³ania tej metody.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IStory story = _storyRepository.FindById(id.ToGuid());

                    if (story == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Podany artyku³ nie istnieje." };
                    }
                    else
                    {
                        string path = ThumbnailHelper.GetThumbnailVirtualPathForStoryOrCreateNew(story.Url, story.Id.Shrink(), ThumbnailSize.Medium, createMediumThumbnail: true, doNotCheckForExistingMiniature: true);

                        viewData = new JsonCreateViewData { isSuccessful = true, url = path };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("odswie¿ania miniaturki") };
                }
            }

            return Json(viewData);
        }

        public ActionResult PromotedBy(string name, int? page)
        {
            IUser user = UserRepository.FindById(name.ToGuid());
            PagedResult<IStory> pagedResult = _storyRepository.FindPromotedByUser(user.Id, PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

            StoryListUserViewData viewData = CreateStoryListViewData<StoryListUserViewData>(page);

            viewData.Stories = pagedResult.Result;
            viewData.TotalStoryCount = pagedResult.Total;
            viewData.RssUrl = Url.Action("PromotedBy", "Feed", new { name });
            viewData.AtomUrl = Url.Action("PromotedBy", "Feed", new { name, format = "Atom" });
            viewData.NoStoryExistMessage = "Brak artyku³ów promowanych przez \"{0}\".".FormatWith(user.UserName.HtmlEncode());
            viewData.SelectedTab = UserDetailTab.Promoted;
            viewData.TheUser = user;

            return View("UserStoryList", viewData);
        }

        public ActionResult PostedBy(string name, int? page)
        {
            IUser user = UserRepository.FindById(name.ToGuid());
            PagedResult<IStory> pagedResult = _storyRepository.FindPostedByUser(user.Id, PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

            StoryListUserViewData viewData = CreateStoryListViewData<StoryListUserViewData>(page);

            viewData.Stories = pagedResult.Result;
            viewData.TotalStoryCount = pagedResult.Total;
            viewData.RssUrl = Url.Action("PostedBy", "Feed", new { name });
            viewData.AtomUrl = Url.Action("PostedBy", "Feed", new { name, format = "Atom" });
            viewData.NoStoryExistMessage = "Brak artyku³ów opublikowanych przez \"{0}\".".FormatWith(user.UserName.HtmlEncode());
            viewData.SelectedTab = UserDetailTab.Posted;
            viewData.TheUser = user;

            return View("UserStoryList", viewData);
        }

        public ActionResult CommentedBy(string name, int? page)
        {
            IUser user = UserRepository.FindById(name.ToGuid());
            PagedResult<IStory> pagedResult = _storyRepository.FindCommentedByUser(user.Id, PageCalculator.StartIndex(page, Settings.HtmlStoryPerPage), Settings.HtmlStoryPerPage);

            StoryListUserViewData viewData = CreateStoryListViewData<StoryListUserViewData>(page);

            viewData.Stories = pagedResult.Result;
            viewData.TotalStoryCount = pagedResult.Total;
            viewData.RssUrl = Url.Action("CommentedBy", "Feed", new { name });
            viewData.AtomUrl = Url.Action("CommentedBy", "Feed", new { name, format = "Atom" });
            viewData.NoStoryExistMessage = "Brak artyku³ów skomentowanych przez \"{0}\".".FormatWith(user.UserName.HtmlEncode());
            viewData.SelectedTab = UserDetailTab.Commented;
            viewData.TheUser = user;

            return View("UserStoryList", viewData);
        }

        public string GetThumbnailPath(string storyId, string size)
        {
            var thumbnailSize = ThumbnailSize.Small;
            if (!string.IsNullOrWhiteSpace(size) && size.ToLower().Equals("medium"))
                thumbnailSize = ThumbnailSize.Medium;

            IStory story = _storyRepository.FindById(storyId.NullSafe().ToGuid());

            if (story != null)
                return ThumbnailHelper.GetThumbnailVirtualPathForStoryOrCreateNew(story.Url, storyId, thumbnailSize);

            return "";
        }

        private T CreateStoryListViewData<T>(int? page) where T : StoryListViewData, new()
        {
            T viewData = CreateStoryViewData<T>();

            viewData.CurrentPage = page ?? 1;
            viewData.StoryPerPage = Settings.HtmlStoryPerPage;
            viewData.FacebookUrl = "http://facebook.com/dotnetomaniakpl";
            viewData.GooglePlusUrl = "https://plus.google.com/110925305542873177884?prsrc=3";

            return viewData;
        }

        private T CreateStoryViewData<T>() where T : BaseStoryViewData, new()
        {
            T viewData = CreateViewData<T>();

            viewData.SocialServices = GetSocialServicesNames();
            viewData.PromoteText = Settings.PromoteText;
            viewData.DemoteText = Settings.DemoteText;
            viewData.CountText = Settings.CountText;

            return viewData;
        }

        private PagedResult<StorySummary> ConvertToSummary(Func<int, int, PagedResult<IStory>> method, int? start, int? max)
        {
            Func<int?, string, int?> getValue = (value, key) =>
                                                {
                                                    if (!value.HasValue)
                                                    {
                                                        if (HttpContext.Request.QueryString[key] != null)
                                                        {
                                                            int temp;

                                                            if (int.TryParse(HttpContext.Request.QueryString[key], out temp))
                                                            {
                                                                value = temp;
                                                            }
                                                        }
                                                    }

                                                    return value;
                                                };

            start = getValue(start, "start");
            max = getValue(max, "max");

            if (!start.HasValue || (start <= 0))
            {
                start = 1;
            }

            start -= 1;

            if (!max.HasValue || (max <= 0))
            {
                max = 25;
            }

            if (max > 50)
            {
                max = 50;
            }

            PagedResult<IStory> pagedResult = method(start.Value, max.Value);
            List<StorySummary> summaries = pagedResult.Result.Select(s => new StorySummary { Title = s.Title, ThumbnailUrl = ThumbnailHelper.GetThumbnailVirtualPathForStory(s.Id.Shrink(), ThumbnailSize.Medium), Url = Url.RouteUrl("Detail", new { name = s.UniqueName }), Description = s.StrippedDescription() }).ToList();

            return new PagedResult<StorySummary>(summaries, pagedResult.Total);
        }

        private string[] GetSocialServicesNames()
        {
            return _socialServiceRedirectors.Select(s => s.GetType().Name.Replace("Redirector", string.Empty).ToLowerInvariant()).ToArray();
        }
    }
}