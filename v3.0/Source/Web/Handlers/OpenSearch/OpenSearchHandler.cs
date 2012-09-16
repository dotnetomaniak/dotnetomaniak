namespace Kigg.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml.Linq;

    using Infrastructure;

    public class OpenSearchHandler : BaseHandler
    {
        public OpenSearchHandler()
        {
            IoC.Inject(this);
        }

        public IConfigurationSettings Settings
        {
            get;
            set;
        }

        public float CacheDurationInDays
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
            const string CacheKey = "openSearchDescription";

            XNamespace ns = "http://a9.com/-/spec/opensearch/1.1/";
            XNamespace moz = "http://www.mozilla.org/2006/browser/search/";

            HandlerCacheItem cacheItem;

            Cache.TryGet(CacheKey, out cacheItem);

            if (cacheItem == null)
            {
                XElement openSearch = new XElement(
                                                        ns + "OpenSearchDescription",
                                                        new XAttribute("xmlns", ns.ToString()),
                                                        new XAttribute(XNamespace.Xmlns + "moz", moz.ToString()),
                                                        new XElement(ns + "ShortName", Settings.SiteTitle),
                                                        new XElement(ns + "Description", Settings.MetaDescription),
                                                        new XElement(ns + "LongName", "{0} Web Search".FormatWith(Settings.SiteTitle)));

                UrlHelper url = CreateUrlHelper(context);

                string rootUrl = Settings.RootUrl;
                string htmlUrl = url.RouteUrl("Search");
                string rssUrl = url.RouteUrl("FeedSearch");
                string atomUrl = url.RouteUrl("FeedSearch", new { format = "Atom" });
                string tagUrl = url.RouteUrl("SuggestTags");

                openSearch.Add(
                                    new XElement(
                                                    ns + "Url",
                                                    new XAttribute("type", "text/html"),
                                                    new XAttribute("template", string.Concat("{0}{1}".FormatWith(rootUrl, htmlUrl), "?q={searchTerms}"))
                                                ),
                                    new XElement(
                                                    ns + "Url",
                                                    new XAttribute("type", "application/atom+xml"),
                                                    new XAttribute("template", string.Concat("{0}{1}".FormatWith(rootUrl, atomUrl), "/{searchTerms}"))
                                                ),
                                    new XElement(
                                                    ns + "Url",
                                                    new XAttribute("type", "application/rss+xml"),
                                                    new XAttribute("template", string.Concat("{0}{1}".FormatWith(rootUrl, rssUrl), "/{searchTerms}"))
                                                ),
                                    new XElement(
                                                    ns + "Url",
                                                    new XAttribute("type", "application/x-suggestions+json"),
                                                    new XAttribute("method", "GET"),
                                                    new XAttribute("template", string.Concat("{0}{1}".FormatWith(rootUrl, tagUrl), "?q={searchTerms}&client=browser"))
                                                )
                            );

                openSearch.Add(new XElement(moz + "SearchForm", rootUrl));

                string tags = string.Join(" ", Settings.MetaKeywords.Split(','));

                openSearch.Add(
                                    new XElement(ns + "Contact", Settings.WebmasterEmail),
                                    new XElement(ns + "Tags", tags)
                              );

                openSearch.Add(
                                    new XElement(ns + "Image", new XAttribute("height", "16"), new XAttribute("width", "16"), new XAttribute("type", "image/x-icon"), "{0}/Assets/Images/fav.ico".FormatWith(rootUrl)),
                                    new XElement(ns + "Image", new XAttribute("height", "64"), new XAttribute("width", "64"), new XAttribute("type", "image/png"), "{0}/Assets/Images/fav.png".FormatWith(rootUrl)),
                                    new XElement(ns + "Image", new XAttribute("type", "image/png"), "{0}/Assets/Images/logo2.png".FormatWith(rootUrl))
                              );

                openSearch.Add(
                                    new XElement(ns + "Query", new XAttribute("role", "example"), new XAttribute("searchTerms", "asp.net")),
                                    new XElement(ns + "Developer", "{0} Development Team".FormatWith(Settings.SiteTitle)),
                                    new XElement(ns + "Attribution", "Search data Copyright (c) {0}".FormatWith(Settings.SiteTitle)),
                                    new XElement(ns + "SyndicationRight", "open"),
                                    new XElement(ns + "AdultContent", "false"),
                                    new XElement(ns + "Language", "en-us"),
                                    new XElement(ns + "OutputEncoding", "UTF-8"),
                                    new XElement(ns + "InputEncoding", "UTF-8")
                              );

                XDocument doc = new XDocument();
                doc.Add(openSearch);

                cacheItem = new HandlerCacheItem { Content = doc.ToXml() };

                if ((CacheDurationInDays > 0) && !Cache.Contains(CacheKey))
                {
                    Cache.Set(CacheKey, cacheItem, SystemTime.Now().AddDays(CacheDurationInDays));
                }
            }

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

            response.ContentType = "application/opensearchdescription+xml";
            response.Write(cacheItem.Content);

            if (CacheDurationInDays > 0)
            {
                if (GenerateETag)
                {
                    response.Cache.SetETag(cacheItem.ETag);
                }

                context.CacheResponseFor(TimeSpan.FromDays(CacheDurationInDays));
            }
        }
    }
}