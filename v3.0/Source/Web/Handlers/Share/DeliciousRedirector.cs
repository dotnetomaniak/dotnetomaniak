namespace Kigg.Web
{
    using System.Web;

    using DomainObjects;

    public class DeliciousRedirector : ISocialServiceRedirector
    {
        public void Redirect(HttpContextBase httpContext, IStory story)
        {
            httpContext.Response.Redirect("http://delicious.com/save?url={0}&title={1}&v=5".FormatWith(story.Url.UrlEncode(), story.Title.UrlEncode()));
        }
    }
}