namespace Kigg.Web
{
    using System;
    using System.Net;
    using System.Web;

    public class Offline : BaseHttpModule
    {
        public override void OnBeginRequest(HttpContextBase context)
        {
            Uri requestedUrl = context.Request.Url;

            string assets = "{0}://{1}/Assets".FormatWith(requestedUrl.Scheme, requestedUrl.Host);

            if (!requestedUrl.ToString().StartsWith(assets, StringComparison.OrdinalIgnoreCase))
            {
                throw new HttpException((int) HttpStatusCode.ServiceUnavailable, "We are currently doing some maintenance. Please check back soon.");
            }
        }
    }
}