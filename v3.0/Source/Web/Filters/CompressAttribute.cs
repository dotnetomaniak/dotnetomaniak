using Kigg.Infrastructure;

namespace Kigg.Web
{
    using System;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class CompressAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // Only apply compression when there is no exception or exception is handled
            if ((filterContext.Exception == null) || ((filterContext.Exception != null) && filterContext.ExceptionHandled))
            {
                Log.Info("Compress attribute executed before: {0}", filterContext.HttpContext.Request.RawUrl);
                filterContext.HttpContext.CompressResponse();
                Log.Info("Compress attribute executed after: {0}", filterContext.HttpContext.Request.RawUrl);
            }

            base.OnResultExecuted(filterContext);
        }
    }
}