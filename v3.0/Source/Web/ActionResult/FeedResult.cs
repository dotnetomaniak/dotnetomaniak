namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Xml.Linq;

    using DomainObjects;
    using Infrastructure;

    public class FeedResult : ActionResult
    {
        private static readonly XNamespace atom = "http://www.w3.org/2005/Atom";
        private static readonly XNamespace openSearch = "http://a9.com/-/spec/opensearch/1.1/";

        private readonly FeedViewData _model;
        private readonly string _format;
        private readonly XNamespace _ns;

        public FeedResult(FeedViewData model, string format)
        {
            Check.Argument.IsNotNull(model, "model");

            _model = model;
            _format = format;

            _ns = model.RootUrl;
        }

        public FeedViewData Data
        {
            get
            {
                return _model;
            }
        }

        public string Format
        {
            get
            {
                return _format;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            bool isAtom = (string.Compare(_format, "Atom", StringComparison.OrdinalIgnoreCase) == 0);
            string xml = isAtom ? BuildAtom(context) : BuildRss(context);

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = isAtom ? "application/atom+xml" : "application/rss+xml";
            response.Write(xml);
        }

        private string BuildRss(ControllerContext context)
        {
            const string DateFormat = "r";

            UrlHelper urlHelper = context.Url();

            XDocument doc = new XDocument();

            XElement feed = CreateRootRssElement();
            XElement channel = CreateRssChannel(context.RequestContext, urlHelper, DateFormat);

            if (_model.CacheDurationInMinutes > 0)
            {
                channel.Add(new XElement("ttl", _model.CacheDurationInMinutes));
            }

            channel.Add(
                            new XElement(
                                            "image",
                                            new XElement("url", "{0}/Assets/Images/dotnetomaniak_logo-negatyw_small.png".FormatWith(_model.RootUrl)),
                                            new XElement("title", _model.Title),
                                            new XElement("link", string.Concat(_model.RootUrl, _model.Url))
                                        )
                        );

            if (!_model.Stories.IsNullOrEmpty())
            {
                AppendTagsInRss(channel, _model.Stories.SelectMany(s => s.Tags).Distinct().OrderBy(t => t.Name), urlHelper);

                foreach (IStory story in _model.Stories)
                {
                    AppendStoryInRss(channel, story, urlHelper, DateFormat);
                }
            }

            feed.Add(channel);
            doc.Add(feed);

            return doc.ToXml();
        }

        private XElement CreateRootRssElement()
        {
            return new XElement("rss", new XAttribute("version", "2.0"), new XAttribute(XNamespace.Xmlns + "atom", atom.ToString()), new XAttribute(XNamespace.Xmlns + "opensearch", openSearch.ToString()), new XAttribute(XNamespace.Xmlns + _model.SiteTitle, _ns.ToString()));
        }

        private XElement CreateRssChannel(RequestContext context, UrlHelper urlHelper, string dateFormat)
        {
            return new XElement(
                                    "channel",
                                    new XElement("title", _model.Title),
                                    new XElement("link", string.Concat(_model.RootUrl, _model.Url)),
                                    new XElement(
                                                    atom + "link",
                                                    new XAttribute("href", context.HttpContext.Request.Url),
                                                    new XAttribute("rel", "self"),
                                                    new XAttribute("type", "application/rss+xml")
                                                ),
                                    new XElement(
                                                    atom + "link",
                                                    new XAttribute("rel", "search"),
                                                    new XAttribute("href", urlHelper.Content("~/opensearch.axd")),
                                                    new XAttribute("type", "application/opensearchdescription+xml"),
                                                    new XAttribute("title", _model.SiteTitle)),
                                                    new XElement("description", _model.Description),
                                                    new XElement("webMaster", "{0} ({1} webmaster)".FormatWith(_model.Email, _model.SiteTitle)),
                                                    new XElement("lastBuildDate", SystemTime.Now().ToString(dateFormat, Constants.CurrentCulture)),
                                                    new XElement("language", "pl-PL"),
                                                    new XElement("copyright", "Copyright (c) {0}".FormatWith(_model.SiteTitle)),
                                                    new XElement("generator", "{0} RSS Generator({1})".FormatWith(_model.SiteTitle, Assembly.GetExecutingAssembly().GetName().Version)
                                                ),
                                    new XElement(openSearch + "totalResults", _model.TotalStoryCount),
                                    new XElement(openSearch + "startIndex", _model.Start),
                                    new XElement(openSearch + "itemsPerPage", _model.Max)
                                );
        }

        private void AppendStoryInRss(XContainer channel, IStory story, UrlHelper urlHelper, string dateFormat)
        {
            string detailUrl = string.Concat(_model.RootUrl, urlHelper.RouteUrl("Detail", new { name = story.UniqueName }));
            string storyDescription = PrepareDescription(story, detailUrl);

            XElement item = new XElement(
                                            "item",
                                            new XElement("guid", new XAttribute("isPermaLink", "true"), detailUrl),
                                            new XElement("link", detailUrl),
                                            new XElement("title", story.Title),
                                            new XElement("description", new XCData(storyDescription)),
                                            new XElement("comments", "{0}#comments".FormatWith(detailUrl))
                                        );

            if (story.IsPublished())
            {
                item.Add(new XElement("pubDate", story.PublishedAt.Value.ToString(dateFormat, Constants.CurrentCulture)));
            }

            if (story.HasTags())
            {
                AppendTagsInRss(item, story.Tags, urlHelper);
            }

            item.Add(new XElement(_ns + "link", detailUrl));
            item.Add(new XElement(_ns + "voteCount", story.VoteCount));
            item.Add(new XElement(_ns + "viewCount", story.ViewCount));
            item.Add(new XElement(_ns + "commentCount", story.CommentCount));
            item.Add(new XElement(_ns + "id", story.Id.Shrink()));

            ICategory category = story.BelongsTo;

            string categoryUrl = string.Concat(_model.RootUrl, urlHelper.Action("Category", "Story", new { name = category.UniqueName }));

            item.Add(new XElement(_ns + "category", new XAttribute("domain", categoryUrl), category.Name));

            string userUrl = string.Concat(_model.RootUrl, urlHelper.RouteUrl("User", new { name = story.PostedBy.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }));

            item.Add(new XElement(_ns + "contributer", new XAttribute("domain", userUrl), story.PostedBy.UserName));

            channel.Add(item);
        }

        private void AppendTagsInRss(XContainer target, IEnumerable<ITag> tags, UrlHelper urlHelper)
        {
            foreach (ITag tag in tags)
            {
                string tagUrl = string.Concat(_model.RootUrl, urlHelper.Action("Tags", "Story", new { name = tag.UniqueName }));

                target.Add(new XElement("category", new XAttribute("domain", tagUrl), tag.Name));
            }
        }

        private string BuildAtom(ControllerContext context)
        {
            const string DateFormat = "yyyy-MM-ddTHH:mm:ssZ";

            UrlHelper urlHelper = context.Url();

            XDocument doc = new XDocument();

            XElement feed = CreateRootAtomElement(context.RequestContext, urlHelper, DateFormat);

            if (!_model.Stories.IsNullOrEmpty())
            {
                AppendTagsInAtom(feed, _model.Stories.SelectMany(s => s.Tags).Distinct().OrderBy(t => t.Name), urlHelper);

                foreach (IStory story in _model.Stories)
                {
                    AppendStoryInAtom(feed, story, urlHelper, DateFormat);
                }
            }

            doc.Add(feed);

            return doc.ToXml();
        }

        private void AppendStoryInAtom(XContainer feed, IStory story, UrlHelper urlHelper, string dateFormat)
        {
            string detailUrl = string.Concat(_model.RootUrl, urlHelper.RouteUrl("Detail", new { name = story.UniqueName }));
            string storyDescription = PrepareDescription(story, detailUrl);

            string userUrl = string.Concat(_model.RootUrl, urlHelper.RouteUrl("User", new { name = story.PostedBy.Id.Shrink(), tab = UserDetailTab.Promoted, page = 1 }));

            XElement entry = new XElement(
                                            atom + "entry",
                                            new XElement(atom + "id", detailUrl),
                                            new XElement(atom + "title", story.Title),
                                            new XElement(atom + "updated", story.CreatedAt.ToString(dateFormat, Constants.CurrentCulture)),
                                            new XElement(atom + "content", new XAttribute("type", "html"), storyDescription),
                                            new XElement(atom + "link", new XAttribute("rel", "alternate"), new XAttribute("href", detailUrl)),
                                            new XElement(atom + "contributor", new XElement(atom + "name", story.PostedBy.UserName), new XElement(atom + "uri", userUrl))
                                          );

            if (story.IsPublished())
            {
                entry.Add(new XElement(atom + "published", story.PublishedAt.Value.ToString(dateFormat, Constants.CurrentCulture)));
            }

            if (story.HasTags())
            {
                AppendTagsInAtom(entry, story.Tags, urlHelper);
            }

            entry.Add(new XElement(_ns + "link", detailUrl));
            entry.Add(new XElement(_ns + "voteCount", story.VoteCount));
            entry.Add(new XElement(_ns + "viewCount", story.ViewCount));
            entry.Add(new XElement(_ns + "commentCount", story.CommentCount));

            ICategory category = story.BelongsTo;
            string categoryUrl = string.Concat(_model.RootUrl, urlHelper.Action("Category", "Story", new { name = category.UniqueName }));

            entry.Add(new XElement(_ns + "category", new XAttribute("term", category.Name), new XAttribute("scheme", categoryUrl)));

            feed.Add(entry);
        }

        private XElement CreateRootAtomElement(RequestContext context, UrlHelper urlHelper, string dateFormat)
        {
            return new XElement(
                                    atom + "feed",
                                    new XAttribute("xmlns", atom.ToString()),
                                    new XAttribute(XNamespace.Xmlns + "opensearch", openSearch.ToString()),
                                    new XAttribute(XNamespace.Xmlns + _model.SiteTitle, _ns.ToString()),
                                    new XAttribute(XNamespace.Xmlns + "lang", "pl-PL"),
                                    new XElement(atom + "title", _model.Title),
                                    new XElement(atom + "subtitle", new XAttribute("type", "text"), _model.Description),
                                    new XElement(atom + "link", new XAttribute("href", context.HttpContext.Request.Url), new XAttribute("rel", "self")),
                                    new XElement(atom + "link", new XAttribute("href", string.Concat(_model.RootUrl, _model.Url)), new XAttribute("rel", "alternate")),
                                    new XElement(atom + "link", new XAttribute("rel", "search"), new XAttribute("href", urlHelper.Content("~/opensearch.axd")), new XAttribute("type", "application/opensearchdescription+xml"), new XAttribute("title", _model.SiteTitle)),
                                    new XElement(atom + "updated", SystemTime.Now().ToString(dateFormat, Constants.CurrentCulture)),
                                    new XElement(atom + "id", string.Concat(_model.RootUrl, _model.Url)),
                                    new XElement(atom + "rights", "Copyright (c) {0}".FormatWith(_model.SiteTitle)),
                                    new XElement(atom + "generator", new XAttribute("uri", _model.RootUrl), new XAttribute("version", Assembly.GetExecutingAssembly().GetName().Version), "{0} Atom Generator".FormatWith(_model.SiteTitle)),
                                    new XElement(atom + "author", new XElement(atom + "name", "{0} webmaster".FormatWith(_model.SiteTitle)), new XElement(atom + "email", _model.Email)),
                                    new XElement(atom + "icon", "{0}/Assets/Images/fav.ico".FormatWith(_model.RootUrl)),
                                    new XElement(atom + "logo", "{0}/Assets/Images/dotnetomaniak_logo-negatyw_small.png".FormatWith(_model.RootUrl)),
                                    new XElement(openSearch + "totalResults", _model.TotalStoryCount),
                                    new XElement(openSearch + "startIndex", _model.Start),
                                    new XElement(openSearch + "itemsPerPage", _model.Max)
                                );
        }

        private void AppendTagsInAtom(XContainer target, IEnumerable<ITag> tags, UrlHelper urlHelper)
        {
            foreach (ITag tag in tags)
            {
                string tagUrl = string.Concat(_model.RootUrl, urlHelper.Action("Tags", "Story", new { name = tag.UniqueName }));

                target.Add(new XElement(atom + "category", new XAttribute("term", tag.Name), new XAttribute("scheme", tagUrl)));
            }
        }

        private string PrepareDescription(IStory story, string detailUrl)
        {
            string imageUrl = string.Concat(_model.RootUrl, "/image.axd", "?url=", story.Url.UrlEncode());

            string storyLink = "<a rev=\"vote-for\" href=\"{0}\"><img alt=\"{1}\" src=\"{2}\" style=\"border:0px\"/></a>".FormatWith(detailUrl.AttributeEncode(), Data.PromoteText, imageUrl.AttributeEncode());

            return "<div>" +
                        "<div>" +
                            "<div style=\"float:right\">" +
                                "<img alt =\"\" src=\"{0}\"/>".FormatWith(ThumbnailHelper.GetThumbnailVirtualPathForStory(story.Id.Shrink(), ThumbnailSize.Small, true).AttributeEncode()) +
                            "</div>" +
                            "<div>" +
                                "{0}".FormatWith(story.TextDescription) +
                            "</div>" +
                        "</div>" +
                        "<div style=\"padding-top:4px\">" +
                            "{0}".FormatWith(storyLink) +
                        "</div>" +
                    "</div>";
        }
    }
}