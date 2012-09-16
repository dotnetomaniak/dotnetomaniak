namespace Kigg.Web
{
    using System.Web;

    using DomainObjects;

    public class FaceBookRedirector : ISocialServiceRedirector
    {
        public void Redirect(HttpContextBase httpContext, IStory story)
        {
            httpContext.Response.Redirect("http://www.facebook.com/sharer.php?u={0}".FormatWith(story.Url.UrlEncode()));
        }
    }
}