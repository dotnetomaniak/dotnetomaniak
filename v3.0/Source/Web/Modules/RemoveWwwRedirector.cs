namespace Kigg.Web
{
    using System;
    using System.Net;
    using System.Web;

    public class RemoveWwwRedirector : BaseHttpModule
    {
        public override void OnBeginRequest(HttpContextBase context)
        {
            const string Prefix = "http://www.";

            string url = context.Request.Url.ToString();

            bool startsWith3W = url.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase);

            if (startsWith3W)
            {
                string newUrl = "http://" + url.Substring(Prefix.Length);

                HttpResponseBase response = context.Response;

                response.StatusCode = (int) HttpStatusCode.MovedPermanently;
                response.Status = "301 Moved Permanently";
                response.RedirectLocation = newUrl;
                response.SuppressContent = true;
                response.End();
            }
        }
    }
}