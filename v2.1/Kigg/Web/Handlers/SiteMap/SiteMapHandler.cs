namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Xml.Linq;

    using DomainObjects;
    using Infrastructure;
    using Repository;

    public enum SiteMapChangeFrequency
    {
        Daily = 0,
        Always = 1,
        Hourly = 2,
        Weekly = 3,
        Monthly = 4,
        Yearly = 5,
        Never = 6
    }

    public enum SiteMapUpdatePriority
    {
        Normal = 0,
        Critical = 1,
        High = 2,
        Low = 3
    }

    public class SiteMapHandler : BaseHandler
    {
        private const int CriticalPriorityPageLimit = 5;

        private static readonly XNamespace _ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private static readonly XNamespace _googleMobile = "http://www.google.com/schemas/sitemap-mobile/1.0";
        private static readonly XNamespace _googleNews = "http://www.google.com/schemas/sitemap-news/0.9";

        public SiteMapHandler()
        {
            IoC.Inject(this);
        }

        public IConfigurationSettings Settings
        {
            get;
            set;
        }

        public IUserRepository UserRepository
        {
            get;
            set;
        }

        public ICategoryRepository CategoryRepository
        {
            get;
            set;
        }

        public ITagRepository TagRepository
        {
            get;
            set;
        }

        public IStoryRepository StoryRepository
        {
            get;
            set;
        }

        public int PublishedStoryMaxCount
        {
            get;
            set;
        }

        public int UpcomingStoryMaxCount
        {
            get;
            set;
        }

        public float CacheDurationInMinutes
        {
            get;
            set;
        }

        public bool GenerateETag
        {
            get;
            set;
        }

        public bool Compress
        {
            get;
            set;
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            bool forNews = context.Request.Path.EndsWith("newssitemap.axd", StringComparison.OrdinalIgnoreCase);
            HandlerCacheItem cacheItem = forNews ? GetNewsSiteMap(context) : GetRegularSiteMap(context);

            if (GenerateETag)
            {
                if (HandleIfNotModified(context, cacheItem.ETag))
                {
                    return;
                }
            }

            if (Compress)
            {
                context.CompressResponse();
            }

            HttpResponseBase response = context.Response;

            response.ContentType = "text/xml";
            response.Write(cacheItem.Content);

            if (CacheDurationInMinutes > 0)
            {
                if (GenerateETag)
                {
                    response.Cache.SetETag(cacheItem.ETag);
                }

                context.CacheResponseFor(TimeSpan.FromMinutes(CacheDurationInMinutes));
            }
        }

        private static XElement CreateEntry(HttpContextBase context, string rootUrl, string routeName, object values, IFormattable lastModified, SiteMapChangeFrequency frequency, SiteMapUpdatePriority priority, bool forMobile)
        {
            string url = string.Concat(rootUrl, GenerateUrlFrom(context, routeName, values));

            return BuildEntry(url, lastModified, frequency, priority, forMobile);
        }

        private static XElement CreateEntry(HttpContextBase context, string rootUrl, string controllerName, string actionName, object values, IFormattable lastModified, SiteMapChangeFrequency frequency, SiteMapUpdatePriority priority, bool forMobile)
        {
            string url = string.Concat(rootUrl, GenerateUrlFrom(context, controllerName, actionName, values));

            return BuildEntry(url, lastModified, frequency, priority, forMobile);
        }

        private static string GenerateUrlFrom(HttpContextBase context, string routeName, object values)
        {
            UrlHelper urlHelper = CreateUrlHelper(context);

            return (values == null) ? urlHelper.RouteUrl(routeName) : urlHelper.RouteUrl(routeName, new RouteValueDictionary(values));
        }

        private static string GenerateUrlFrom(HttpContextBase context, string controllerName, string actionName, object values)
        {
            UrlHelper urlHelper = CreateUrlHelper(context);

            return (values == null) ? urlHelper.Action(actionName, controllerName) : urlHelper.Action(actionName, controllerName, new RouteValueDictionary(values));
        }

        private static XElement BuildEntry(string url, IFormattable lastModified, SiteMapChangeFrequency frequency, SiteMapUpdatePriority priority, bool forMobile)
        {
            XElement x = new XElement(_ns + "url", new XElement(_ns + "loc", url));

            if (forMobile)
            {
                x.Add(new XElement(_googleMobile + "mobile"));
            }
            else
            {
                string priorityString;

                switch (priority)
                {
                    case SiteMapUpdatePriority.Critical:
                        {
                            priorityString = "0.9";
                            break;
                        }

                    case SiteMapUpdatePriority.High:
                        {
                            priorityString = "0.7";
                            break;
                        }

                    case SiteMapUpdatePriority.Low:
                        {
                            priorityString = "0.3";
                            break;
                        }

                    default:
                        {
                            priorityString = "0.5";
                            break;
                        }
                }

                x.Add(new XElement(_ns + "lastmod", lastModified.ToString("yyyy-MM-dd", Constants.CurrentCulture)), new XElement(_ns + "changefreq", frequency.ToString().ToLowerInvariant()), new XElement(_ns + "priority", priorityString));
            }

            return x;
        }

        private HandlerCacheItem GetNewsSiteMap(HttpContextBase context)
        {
            const string DateFormat = "yyyy-MM-ddThh:mm:ssZ";
            const string CacheKey = "newsSitemap";

            Log.Info("Generating {0}".FormatWith(CacheKey));

            HandlerCacheItem cacheItem;

            Cache.TryGet(CacheKey, out cacheItem);

            if (cacheItem == null)
            {
                XElement urlSet = new XElement(_ns + "urlset", new XAttribute("xmlns", _ns.ToString()), new XAttribute(XNamespace.Xmlns + "news", _googleNews.ToString()));

                PagedResult<IStory> pagedResult = StoryRepository.FindPublished(0, 1000); // Google only supports 1000 story

                int i = 0;

                foreach (IStory story in pagedResult.Result)
                {
                    SiteMapUpdatePriority priority = (i < 50) ? SiteMapUpdatePriority.Critical : SiteMapUpdatePriority.Normal;

                    AddStoryInNewsSiteMap(context, urlSet, story, priority, DateFormat);

                    i += 1;
                }

                XDocument doc = new XDocument();
                doc.Add(urlSet);

                cacheItem = new HandlerCacheItem { Content = doc.ToXml() };

                if ((CacheDurationInMinutes > 0) && (!Cache.Contains(CacheKey)))
                {
                    Cache.Set(CacheKey, cacheItem, SystemTime.Now().AddMinutes(CacheDurationInMinutes));
                }
            }

            Log.Info("{0} generated".FormatWith(CacheKey));

            return cacheItem;
        }

        private void AddStoryInNewsSiteMap(HttpContextBase context, XContainer target, IStory story, SiteMapUpdatePriority priority, string dateFormat)
        {
            XElement url = CreateEntry(context, Settings.RootUrl, "Detail", new { name = story.UniqueName }, story.LastActivityAt, SiteMapChangeFrequency.Daily, priority, false);

            string keyWords = story.BelongsTo.Name;

            if (story.HasTags())
            {
                keyWords += ", " + string.Join(", ", story.Tags.Select(t => t.Name).ToArray());
            }

            url.Add(new XElement(_googleNews + "news", new XElement(_googleNews + "publication_date", story.PublishedAt.Value.ToString(dateFormat, Constants.CurrentCulture)), new XElement(_googleNews + "keywords", keyWords)));

            target.Add(url);
        }

        private HandlerCacheItem GetRegularSiteMap(HttpContextBase context)
        {
            bool forMobile = context.Request.Path.EndsWith("mobilesitemap.axd", StringComparison.OrdinalIgnoreCase);

            string cacheKey = forMobile ? "mobileSiteMap" : "siteMap";

            Log.Info("Generating {0}".FormatWith(cacheKey));

            HandlerCacheItem cacheItem;

            Cache.TryGet(cacheKey, out cacheItem);

            if (cacheItem == null)
            {
                XElement urlSet = new XElement(_ns + "urlset", new XAttribute("xmlns", _ns.ToString()));

                if (forMobile)
                {
                    urlSet.Add(new XAttribute(XNamespace.Xmlns + "mobile", _googleMobile.ToString()));
                }

                DateTime currentDate = SystemTime.Now();
                string rootUrl = Settings.RootUrl;

                AddStoriesInRegularSiteMap(context, forMobile, urlSet);
                AddPublishedPagesInRegularSiteMap(context, forMobile, urlSet, currentDate);
                AddUpcomingPagesInRegularSiteMap(context, forMobile, urlSet, currentDate);
                AddCategoryPagesInRegularSiteMap(context, forMobile, urlSet, currentDate);
                AddTagPagesInRegularSiteMap(context, forMobile, urlSet, currentDate);
                AddUserPagesInRegularSiteMap(context, forMobile, urlSet, currentDate);

                urlSet.Add(CreateEntry(context, rootUrl, "Submit", null, currentDate, SiteMapChangeFrequency.Monthly, SiteMapUpdatePriority.Low, forMobile));
                urlSet.Add(CreateEntry(context, rootUrl, "Faq", null, currentDate, SiteMapChangeFrequency.Monthly, SiteMapUpdatePriority.Low, forMobile));
                urlSet.Add(CreateEntry(context, rootUrl, "About", null, currentDate, SiteMapChangeFrequency.Monthly, SiteMapUpdatePriority.Low, forMobile));
                urlSet.Add(CreateEntry(context, rootUrl, "Contact", null, currentDate, SiteMapChangeFrequency.Monthly, SiteMapUpdatePriority.Low, forMobile));

                XDocument doc = new XDocument();
                doc.Add(urlSet);

                cacheItem = new HandlerCacheItem { Content = doc.ToXml() };

                if ((CacheDurationInMinutes > 0) && !Cache.Contains(cacheKey))
                {
                    Cache.Set(cacheKey, cacheItem, SystemTime.Now().AddMinutes(CacheDurationInMinutes));
                }
            }

            Log.Info("{0} generated".FormatWith(cacheKey));

            return cacheItem;
        }

        private void AddStoriesInRegularSiteMap(HttpContextBase context, bool forMobile, XContainer target)
        {
            Action<IStory> addStory = story => target.Add(CreateEntry(context, Settings.RootUrl, "Detail", new { name = story.UniqueName }, story.LastActivityAt, SiteMapChangeFrequency.Weekly, SiteMapUpdatePriority.Critical, forMobile));

            ICollection<IStory> publishedStories = StoryRepository.FindPublished(0, PublishedStoryMaxCount).Result;

            foreach (IStory story in publishedStories)
            {
                addStory(story);
            }

            ICollection<IStory> upcomingStories = StoryRepository.FindUpcoming(0, UpcomingStoryMaxCount).Result;

            foreach (IStory story in upcomingStories)
            {
                addStory(story);
            }
        }

        private void AddPublishedPagesInRegularSiteMap(HttpContextBase context, bool forMobile, XContainer target, IFormattable currentDate)
        {
            string rootUrl = Settings.RootUrl;

            int publishPageCount = PageCalculator.TotalPage(StoryRepository.CountByPublished(), Settings.HtmlStoryPerPage);
            int publishPageCounter = 1;

            while (publishPageCounter <= publishPageCount)
            {
                target.Add(CreateEntry(context, rootUrl, "Published", new { page = publishPageCounter }, currentDate, SiteMapChangeFrequency.Daily, ((publishPageCounter <= CriticalPriorityPageLimit) ? SiteMapUpdatePriority.Critical : SiteMapUpdatePriority.Normal), forMobile));
                publishPageCounter += 1;
            }
        }

        private void AddUpcomingPagesInRegularSiteMap(HttpContextBase context, bool forMobile, XContainer target, IFormattable currentDate)
        {
            string rootUrl = Settings.RootUrl;

            int upcomingPageCount = PageCalculator.TotalPage(StoryRepository.CountByUpcoming(), Settings.HtmlStoryPerPage);
            int upcomingPageCounter = 1;

            while (upcomingPageCounter <= upcomingPageCount)
            {
                target.Add(CreateEntry(context, rootUrl, "Upcoming", new { page = upcomingPageCounter }, currentDate, SiteMapChangeFrequency.Hourly, ((upcomingPageCounter <= CriticalPriorityPageLimit) ? SiteMapUpdatePriority.High : SiteMapUpdatePriority.Normal), forMobile));
                upcomingPageCounter += 1;
            }
        }

        private void AddCategoryPagesInRegularSiteMap(HttpContextBase context, bool forMobile, XContainer target, IFormattable currentDate)
        {
            int rowPerPage = Settings.HtmlStoryPerPage;
            string rootUrl = Settings.RootUrl;

            foreach (ICategory category in CategoryRepository.FindAll())
            {
                int categoryPageCount = PageCalculator.TotalPage(StoryRepository.CountByCategory(category.Id), rowPerPage);
                int categoryPageCounter = 1;

                while (categoryPageCounter <= categoryPageCount)
                {
                    target.Add(CreateEntry(context, rootUrl, "Story", "Category", new { name = category.UniqueName, page = categoryPageCounter }, currentDate, SiteMapChangeFrequency.Daily, ((categoryPageCounter <= CriticalPriorityPageLimit) ? SiteMapUpdatePriority.High : SiteMapUpdatePriority.Normal), forMobile));
                    categoryPageCounter += 1;
                }
            }
        }

        private void AddTagPagesInRegularSiteMap(HttpContextBase context, bool forMobile, XContainer target, IFormattable currentDate)
        {
            int rowPerPage = Settings.HtmlStoryPerPage;
            string rootUrl = Settings.RootUrl;

            foreach (ITag tag in TagRepository.FindByUsage(Settings.TopTags))
            {
                int tagPageCount = PageCalculator.TotalPage(StoryRepository.CountByTag(tag.Id), rowPerPage);
                int tagPageCounter = 1;

                while (tagPageCounter <= tagPageCount)
                {
                    target.Add(CreateEntry(context, rootUrl, "Story", "Tags", new { name = tag.UniqueName, page = tagPageCounter }, currentDate, SiteMapChangeFrequency.Daily, ((tagPageCounter <= CriticalPriorityPageLimit) ? SiteMapUpdatePriority.High : SiteMapUpdatePriority.Normal), forMobile));
                    tagPageCounter += 1;
                }
            }
        }

        private void AddUserPagesInRegularSiteMap(HttpContextBase context, bool forMobile, XContainer target, DateTime currentDate)
        {
            string rootUrl = Settings.RootUrl;

            target.Add(CreateEntry(context, rootUrl, "Users", new { page = 1 }, currentDate, SiteMapChangeFrequency.Hourly, SiteMapUpdatePriority.Normal, forMobile));

            ICollection<IUser> topMovers = UserRepository.FindTop(currentDate.AddDays(-1), currentDate, 0, Settings.TopUsers).Result;

            foreach (IUser user in topMovers)
            {
                target.Add(CreateEntry(context, rootUrl, "User", new { name = user.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }, currentDate, SiteMapChangeFrequency.Daily, SiteMapUpdatePriority.Normal, forMobile));
            }
        }
    }
}