namespace Kigg.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class ControllerContextExtension
    {
        public static UrlHelper Url(this ControllerContext controllerContext)
        {
            return new UrlHelper(new RequestContext(controllerContext.HttpContext, controllerContext.RouteData));
        }
    }
}