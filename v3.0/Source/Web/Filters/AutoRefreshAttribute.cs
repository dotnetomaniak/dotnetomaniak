namespace Kigg.Web
{
    using System;
    using System.Diagnostics;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AutoRefreshAttribute : ActionFilterAttribute
    {
        private const int DefaultDurationInSeconds = (15 * 60);

        private readonly int _durationInSeconds;

        private readonly string _actionName;
        private readonly string _controllerName;
        private readonly RouteValueDictionary _values;

        public AutoRefreshAttribute(string actionName, string controllerName, RouteValueDictionary values, int durationInSecond)
        {
            Check.Argument.IsNotNegativeOrZero(durationInSecond, "durationInSecond");
            Check.Argument.IsNotEmpty(actionName, "actionName");
            Check.Argument.IsNotEmpty(controllerName, "controllerName");
            Check.Argument.IsNotNull(values, "values");

            _durationInSeconds = durationInSecond;
            _actionName = actionName;
            _controllerName = controllerName;
            _values = values;
        }

        public AutoRefreshAttribute(string actionName, string controllerName, RouteValueDictionary values) : this(actionName, controllerName, values, DefaultDurationInSeconds)
        {
        }

        public AutoRefreshAttribute(string actionName, string controllerName, object values, int durationInSeconds)
        {
            Check.Argument.IsNotNegativeOrZero(durationInSeconds, "durationInSeconds");
            Check.Argument.IsNotEmpty(actionName, "actionName");
            Check.Argument.IsNotEmpty(controllerName, "controllerName");
            Check.Argument.IsNotNull(values, "values");

            _durationInSeconds = durationInSeconds;
            _actionName = actionName;
            _controllerName = controllerName;
            _values = new RouteValueDictionary(values);
        }

        public AutoRefreshAttribute(string actionName, string controllerName, object values) : this(actionName, controllerName, values, DefaultDurationInSeconds)
        {
        }

        public AutoRefreshAttribute(string actionName, RouteValueDictionary values, int durationInSeconds)
        {
            Check.Argument.IsNotNegativeOrZero(durationInSeconds, "durationInSeconds");
            Check.Argument.IsNotEmpty(actionName, "actionName");
            Check.Argument.IsNotNull(values, "values");

            _durationInSeconds = durationInSeconds;
            _actionName = actionName;
            _values = values;
        }

        public AutoRefreshAttribute(string actionName, RouteValueDictionary values) : this(actionName, values, DefaultDurationInSeconds)
        {
        }

        public AutoRefreshAttribute(string actionName, object values, int durationInSeconds)
        {
            Check.Argument.IsNotNegativeOrZero(durationInSeconds, "durationInSeconds");
            Check.Argument.IsNotEmpty(actionName, "actionName");
            Check.Argument.IsNotNull(values, "values");

            _durationInSeconds = durationInSeconds;
            _actionName = actionName;
            _values = new RouteValueDictionary(values);
        }

        public AutoRefreshAttribute(string actionName, object values) : this(actionName, values, DefaultDurationInSeconds)
        {
        }

        public AutoRefreshAttribute(string actionName, string controllerName, int durationInSeconds)
        {
            Check.Argument.IsNotNegativeOrZero(durationInSeconds, "durationInSeconds");
            Check.Argument.IsNotEmpty(actionName, "actionName");
            Check.Argument.IsNotEmpty(controllerName, "controllerName");

            _durationInSeconds = durationInSeconds;
            _actionName = actionName;
            _controllerName = controllerName;
        }

        public AutoRefreshAttribute(string actionName, string controllerName) : this(actionName, controllerName, DefaultDurationInSeconds)
        {
        }

        public AutoRefreshAttribute(string actionName, int durationInSeconds)
        {
            Check.Argument.IsNotNegativeOrZero(durationInSeconds, "durationInSeconds");
            Check.Argument.IsNotEmpty(actionName, "actionName");

            _durationInSeconds = durationInSeconds;
            _actionName = actionName;
        }

        public AutoRefreshAttribute(string actionName) : this(actionName, DefaultDurationInSeconds)
        {
        }

        public AutoRefreshAttribute(int durationInSeconds)
        {
            Check.Argument.IsNotNegativeOrZero(durationInSeconds, "durationInSeconds");

            _durationInSeconds = durationInSeconds;
        }

        public AutoRefreshAttribute() : this(DefaultDurationInSeconds)
        {
        }

        public int DurationInSeconds
        {
            [DebuggerStepThrough]
            get
            {
                return _durationInSeconds;
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string url = BuildUrl(filterContext);

            filterContext.HttpContext.Response.AppendHeader("Refresh", "{0};Url={1}".FormatWith(_durationInSeconds, url));

            base.OnResultExecuting(filterContext);
        }

        public string BuildUrl(ControllerContext filterContext)
        {
            UrlHelper urlHelper = filterContext.Url();

            string actionName = _actionName;
            string controllerName = _controllerName;

            string url;

            if ((!string.IsNullOrEmpty(actionName)) && (!string.IsNullOrEmpty(controllerName)) && (_values != null))
            {
                url = urlHelper.Action(actionName, controllerName, _values);
            }
            else if ((!string.IsNullOrEmpty(actionName)) && (_values != null))
            {
                url = urlHelper.Action(actionName, _values);
            }
            else if ((!string.IsNullOrEmpty(actionName)) && (!string.IsNullOrEmpty(controllerName)))
            {
                url = urlHelper.Action(actionName, controllerName);
            }
            else
            {
                url = (!string.IsNullOrEmpty(actionName)) ? urlHelper.Action(actionName) : filterContext.HttpContext.Request.RawUrl;
            }

            return url;
        }
    }
}