namespace Kigg.Infrastructure
{
    using System.Collections.Specialized;

    public class ContentService : IContentService
    {
        private readonly IHttpForm _httpForm;
        private readonly IHtmlToStoryContentConverter _converter;
        private readonly string _shortUrlProviderFormat;

        public ContentService(IHttpForm httpForm, IHtmlToStoryContentConverter converter, string shortUrlProviderFormat)
        {
            Check.Argument.IsNotNull(httpForm, "httpForm");
            Check.Argument.IsNotNull(converter, "converter");
            Check.Argument.IsNotEmpty(shortUrlProviderFormat, "shortUrlProviderFormat");

            _httpForm = httpForm;
            _converter = converter;
            _shortUrlProviderFormat = shortUrlProviderFormat;
        }

        public virtual StoryContent Get(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string html = _httpForm.Get(url);

            return string.IsNullOrEmpty(html) ? StoryContent.Empty : _converter.Convert(url, html);
        }

        public virtual void Ping(string url, string title, string fromUrl, string excerpt, string siteTitle)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");
            Check.Argument.IsNotEmpty(title, "title");
            Check.Argument.IsNotEmpty(fromUrl, "fromUrl");
            Check.Argument.IsNotEmpty(excerpt, "excerpt");
            Check.Argument.IsNotEmpty(siteTitle, "siteTitle");

            NameValueCollection formFields = new NameValueCollection
                                                 {
                                                     { "title", title.UrlEncode() },
                                                     { "url", fromUrl.UrlEncode() },
                                                     { "excerpt", excerpt.UrlEncode() },
                                                     { "blog_name", siteTitle.UrlEncode() }
                                                 };

            _httpForm.PostAsync(url, formFields);
        }

        public virtual string ShortUrl(string url)
        {
            Check.Argument.IsNotEmpty(url, "url");

            string shortUrl = _httpForm.Get(_shortUrlProviderFormat.FormatWith(url.UrlEncode()));

            return string.IsNullOrEmpty(shortUrl) ? url : ((shortUrl.Length < url.Length) ? shortUrl : url);
        }
    }
}