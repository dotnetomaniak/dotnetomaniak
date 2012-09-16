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
                filterContext.HttpContext.CompressResponse();
            }

            base.OnResultExecuted(filterContext);
        }
    }
}