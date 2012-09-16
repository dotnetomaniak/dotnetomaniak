using System;
using System.Collections.Specialized;
using System.Drawing;

namespace Kigg.Web
{
    using System.Diagnostics;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public abstract class BaseHandler : IHttpHandler
    {
        public virtual bool IsReusable
        {
            [DebuggerStepThrough]
            get
            {
                return false;
            }
        }

        public static Color GetColor(NameValueCollection queryString, string key, string defaultValue)
        {
            string hexValue = string.IsNullOrEmpty(queryString[key]) ? defaultValue : queryString[key];

            if (!hexValue.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                hexValue = "#" + hexValue;
            }

            try
            {
                return ColorTranslator.FromHtml(hexValue);
            }
            catch
            {
                return ColorTranslator.FromHtml(defaultValue);
            }
        }

        public static int GetInteger(NameValueCollection queryString, string key, int defaultValue)
        {
            int value = defaultValue;

            if (queryString[key] != null)
            {
                if (!int.TryParse(queryString[key], out value))
                {
                    value = defaultValue;
                }
            }

            return value;
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        public abstract void ProcessRequest(HttpContextBase context);

        public static bool HandleIfNotModified(HttpContextBase context, string etag)
        {
            bool notModified = false;

            if (!string.IsNullOrEmpty(etag))
            {
                string ifNoneMatch = context.Request.Headers["If-None-Match"];

                if (!string.IsNullOrEmpty(ifNoneMatch) && (string.CompareOrdinal(ifNoneMatch, etag) == 0))
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotModified;
                    notModified = true;
                }
            }

            return notModified;
        }

        protected static UrlHelper CreateUrlHelper(HttpContextBase context)
        {
            RequestContext viewContext = new RequestContext(context, new RouteData());

            return new UrlHelper(viewContext);
        }
    }
}