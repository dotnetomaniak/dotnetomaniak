namespace Kigg.Web
{
    using System.Web;

    using DomainObjects;

    public interface ISocialServiceRedirector
    {
        void Redirect(HttpContextBase httpContext, IStory story);
    }
}