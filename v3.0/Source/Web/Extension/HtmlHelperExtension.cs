namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    public static class HtmlHelperExtension
    {
        public static string PageHeader(this HtmlHelper helper, string title)
        {
            return PageHeader(helper, title, null, null, null, null);
        }

        public static string PageHeader(this HtmlHelper helper, string title, string rssUrl, string atomUrl, string facebookUrl, string googlePlus)
        {
            StringBuilder headerHtml = new StringBuilder();

            headerHtml.Append("<div class=\"pageHeader\">");

            headerHtml.Append("<div class=\"pageTitle\">");

            if (!string.IsNullOrEmpty(title))
            {
                headerHtml.Append("<h2>{0}</h2>".FormatWith(helper.Encode(title)));
            }

            headerHtml.Append("</div>");
            headerHtml.Append(SyndicationIcons(helper, rssUrl, atomUrl, facebookUrl, googlePlus));

            headerHtml.Append("</div>");

            return headerHtml.ToString();
        }

        public static string ArticleHeader(this HtmlHelper helper, string articleHeader, string[] breadcrumbs)
        {
            StringBuilder articleHeaderHtml = new StringBuilder();
            articleHeaderHtml.Append("<div class=\"articleHeader\">");
            articleHeaderHtml.Append("<p>");
            for (int i = 0; i < breadcrumbs.Length-1; i++)
            {
                articleHeaderHtml.AppendFormat(
                    i == 0 ? "<a id=\"first-crumb\" href=\"#\">{0}</a>" : "<a href=\"#\">{0}</a>", breadcrumbs[i]);
            }
            if (breadcrumbs.Length > 1)
                articleHeaderHtml.AppendFormat("<span>{0}</span>", breadcrumbs[breadcrumbs.Length - 1]);
            articleHeaderHtml.Append("</p>");
            articleHeaderHtml.Append("</div>");
            if (string.IsNullOrEmpty(articleHeader) == false)
                articleHeaderHtml.AppendFormat("<h3>{0}</h3>", articleHeader);
            return articleHeaderHtml.ToString();
        }

        public static string SyndicationIcons(this HtmlHelper helper, string rssUrl, string atomUrl, string facebookUrl, string googlePlus)
        {
            StringBuilder html = new StringBuilder();

            if (!string.IsNullOrEmpty(rssUrl) || !string.IsNullOrEmpty(atomUrl))
            {
                UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

                html.Append("<div class=\"feed\">");

                Action<string, string> addIcon = (type, url) =>
                                                 {
                                                     if (type == "G+")
                                                     {
                                                         html.Append(
                                                             " <a href=\"{0}\" style=\"text-decoration:none;\"><img src=\"https://ssl.gstatic.com/images/icons/gplus-32.png\" alt=\"\" style=\"border:0;width:24px;height:24px;\"/></a>"
                                                                 .FormatWith(helper.AttributeEncode(url)));
                                                     }
                                                     else
                                                     {
                                                        string iconUrl = urlHelper.Image("{0}.png".FormatWith(type));

                                                        html.Append(" <a href=\"{0}\" target=\"_blank\"><img alt=\"{1}\" title=\"{1}\" src=\"{2}\"/></a>".FormatWith(helper.AttributeEncode(url), type, helper.AttributeEncode(iconUrl)));   
                                                     }                                                         
                                                 };

                if (!string.IsNullOrEmpty(atomUrl))
                {
                    addIcon("atom", atomUrl);
                }

                if (!string.IsNullOrEmpty(rssUrl))
                {
                    addIcon("rss", rssUrl);
                }

                if (!string.IsNullOrEmpty(facebookUrl))
                {
                    addIcon("facebook", facebookUrl);
                }

                if (!string.IsNullOrEmpty(googlePlus))
                {
                    addIcon("G+", googlePlus);
                }

                html.Append("</div>");
            }

            return html.ToString();
        }

        public static IDictionary<int, string> ToDictionary<TEnum>(this HtmlHelper helper) where TEnum : IComparable, IFormattable
        {
            Type enumType = typeof(TEnum);
            string[] names = Enum.GetNames(enumType);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            for (int i = 0; i < names.Length; i++)
            {
                int value = Convert.ToInt32((TEnum)Enum.Parse(enumType, names[i]), Constants.CurrentCulture);

                dictionary.Add(value, names[i]);
            }

            return dictionary;
        }

        public static string StoryListPager(this HtmlHelper helper)
        {
            StoryListViewData viewData = (StoryListViewData) helper.ViewContext.ViewData.Model;

            return Pager(helper, null, null, null, helper.ViewContext.RouteData.Values, "page", true, viewData.PageCount, 5, 2, viewData.CurrentPage);
        }

        public static MvcHtmlString Translated(this HtmlHelper helper, string id)
        {
            return MvcHtmlString.Create(Properties.Resource1.ResourceManager.GetString(id));
        }

        public static string UserStoryListPager(this HtmlHelper helper, UserDetailTab selectedTab)
        {
            StoryListViewData viewData = (StoryListViewData) helper.ViewContext.ViewData.Model;

            helper.ViewContext.RouteData.Values["tab"] = selectedTab;

            return Pager(helper, "User", null, null, helper.ViewContext.RouteData.Values, "page", true, viewData.PageCount, 5, 2, viewData.CurrentPage);
        }

        public static string Pager(this HtmlHelper helper, string routeName, string actionName, string controllerName, IDictionary<string, object> values, string pageParamName, bool appendQueryString, int pageCount, int noOfPageToShow, int noOfPageInEdge, int currentPage)
        {
            Func<string, int, string> getPageLink = (text, page) =>
                                                                    {
                                                                        RouteValueDictionary newValues = new RouteValueDictionary();

                                                                        foreach (KeyValuePair<string, object> pair in values)
                                                                        {
                                                                            if ((string.Compare(pair.Key, "controller", StringComparison.OrdinalIgnoreCase) != 0) &&
                                                                                (string.Compare(pair.Key, "action", StringComparison.OrdinalIgnoreCase) != 0))
                                                                            {
                                                                                newValues[pair.Key] = pair.Value;
                                                                            }
                                                                        }

                                                                        if (page > 0)
                                                                        {
                                                                            newValues[pageParamName] = page;
                                                                        }

                                                                        if (appendQueryString)
                                                                        {
                                                                            NameValueCollection queryString = helper.ViewContext.HttpContext.Request.QueryString;

                                                                            foreach (string key in queryString)
                                                                            {
                                                                                if (key != null)
                                                                                {
                                                                                    if (!newValues.ContainsKey(key))
                                                                                    {
                                                                                        if (!string.IsNullOrEmpty(queryString[key]))
                                                                                        {
                                                                                            newValues[key] = queryString[key];
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }

                                                                        string link;

                                                                        if (!string.IsNullOrEmpty(routeName))
                                                                        {
                                                                            link = helper.RouteLink(text, routeName, newValues).ToHtmlString();
                                                                        }
                                                                        else
                                                                        {
                                                                            actionName = actionName ?? values["action"].ToString();
                                                                            controllerName = controllerName ?? values["controller"].ToString();

                                                                            link = helper.ActionLink(text, actionName, controllerName, newValues, null).ToHtmlString();
                                                                        }

                                                                        return string.Concat(" ", link);
                                                                    };

            var pagerHtml = new StringBuilder();

            if (pageCount > 1)
            {
                pagerHtml.Append("<div class=\"pager\">");

                double half = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(noOfPageToShow) / 2));

                int start = Convert.ToInt32((currentPage > half) ? Math.Max(Math.Min((currentPage - half), (pageCount - noOfPageToShow)), 0) : 0);
                int end = Convert.ToInt32((currentPage > half) ? Math.Min(currentPage + half, pageCount) : Math.Min(noOfPageToShow, pageCount));

                //if (currentPage > 1)
                //{
                //    pagerHtml.Append(getPageLink("Previous", currentPage - 1));
                //}
                //else
                //{
                //    pagerHtml.Append(" <span class=\"disabled\">Previous</span>");
                //}

                if (start > 0)
                {
                    int startingEnd = Math.Min(noOfPageInEdge, start);

                    for (int i = 0; i < startingEnd; i++)
                    {
                        int pageNo = (i + 1);

                        pagerHtml.Append(getPageLink(pageNo.ToString(Constants.CurrentCulture), pageNo));
                    }

                    if (noOfPageInEdge < start)
                    {
                        pagerHtml.Append("<span>...</span>");
                    }
                }

                for (int i = start; i < end; i++)
                {
                    int pageNo = (i + 1);

                    if (pageNo == currentPage)
                    {
                        pagerHtml.Append(" <span class=\"active\">{0}</span>".FormatWith(pageNo));
                    }
                    else
                    {
                        pagerHtml.Append(getPageLink(pageNo.ToString(Constants.CurrentCulture), pageNo));
                    }
                }

                if (end < pageCount)
                {
                    if ((pageCount - noOfPageInEdge) > end)
                    {
                        pagerHtml.Append("<span>...</span>");
                    }

                    int endingStart = Math.Max(pageCount - noOfPageInEdge, end);

                    for (int i = endingStart; i < pageCount; i++)
                    {
                        int pageNo = (i + 1);
                        pagerHtml.Append(getPageLink(pageNo.ToString(Constants.CurrentCulture), pageNo));
                    }
                }

                //if (currentPage < pageCount)
                //{
                //    pagerHtml.Append(getPageLink("Next", currentPage + 1));
                //}
                //else
                //{
                //    pagerHtml.Append(" <span class=\"disabled\">Next</span>");
                //}

                pagerHtml.Append("</div>");
            }

            return pagerHtml.ToString();
        }
    }
}