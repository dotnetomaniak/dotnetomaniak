using System.Web;
using Kigg.DomainObjects;

namespace Kigg.Web
{
    public class WykopRedirector : ISocialServiceRedirector
    {
        public void Redirect(HttpContextBase httpContext, IStory story)
        {
            httpContext.Response.Redirect(
                "http://www.wykop.pl/dodaj?url={0}&title={1}&desc={2}".FormatWith(story.Url.UrlEncode(),
                                                                                  story.Title.UrlEncode(),
                                                                                  story.
                                                                                      TextDescription.UrlEncode()));
        }
    }
}
