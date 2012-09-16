namespace Kigg.Web
{
    using System;
    using System.Net;
    using System.Web;

    using Infrastructure;

    public class IPBlock : BaseHttpModule
    {
        public override void OnBeginRequest(HttpContextBase context)
        {
            Uri requestedUrl = context.Request.Url;

            string assets = "{0}://{1}/Assets".FormatWith(requestedUrl.Scheme, requestedUrl.Host);
            string blocked = "{0}://{1}/ErrorPages/AccessDenied.aspx".FormatWith(requestedUrl.Scheme, requestedUrl.Host);

            if ((!requestedUrl.ToString().StartsWith(assets, StringComparison.OrdinalIgnoreCase)) &&
                (!requestedUrl.ToString().StartsWith(blocked, StringComparison.OrdinalIgnoreCase)))
            {
                string ip = context.Request.UserHostAddress;

                if (!string.IsNullOrEmpty(ip))
                {
                    bool shouldBlock = IoC.Resolve<IBlockedIPCollection>().Contains(ip);

                    if (shouldBlock)
                    {
                        Log.Warning("Blocked Ip Address detected: {0}.", ip);
                        throw new HttpException((int) HttpStatusCode.Forbidden, "Ip Address blocked.");
                    }
                }
            }
        }
    }
}