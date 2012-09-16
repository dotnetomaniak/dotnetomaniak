namespace Kigg.Web
{
    using System.Web;

    using DomainObjects;
    using Infrastructure;

    public class TwitterRedirector : ISocialServiceRedirector
    {
        private readonly IContentService _contentService;

        public TwitterRedirector(IContentService contentService)
        {
            Check.Argument.IsNotNull(contentService, "contentService");

            _contentService = contentService;
        }

        public void Redirect(HttpContextBase httpContext, IStory story)
        {
            string shortUrl = _contentService.ShortUrl(story.Url);

            httpContext.Response.Redirect("http://twitter.com/home/?status=Checkout+{0}+-{1}".FormatWith(shortUrl.UrlEncode(), story.Title.UrlEncode()));
        }
    }
}