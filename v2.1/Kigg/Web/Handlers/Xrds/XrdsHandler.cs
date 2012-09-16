namespace Kigg.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using Infrastructure;

    public class XrdsHandler : BaseHandler
    {
        public XrdsHandler()
        {
            IoC.Inject(this);
        }

        public IConfigurationSettings Settings
        {
            get;
            set;
        }

        public float CacheDurationInDays
        {
            get;
            set;
        }

        public bool GenerateETag
        {
            get;
            set;
        }

        public bool Compress
        {
            get;
            set;
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            const string CacheKey = "XrdsDescription";

            const string Xrds = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<xrds:XRDS xmlns:xrds=\"xri://$xrds\" xmlns:openid=\"http://openid.net/xmlns/1.0\" xmlns=\"xri://$xrd*($v*2.0)\">" +
                                    "<XRD>" +
                                        "<Service priority=\"1\">" +
                                            "<Type>http://specs.openid.net/auth/2.0/return_to</Type>" +
                                            "<URI>{0}</URI>" +
                                        "</Service>" +
                                    "</XRD>" +
                                "</xrds:XRDS>";

            HandlerCacheItem cacheItem;

            Cache.TryGet(CacheKey, out cacheItem);

            if (cacheItem == null)
            {
                UrlHelper urlHelper = CreateUrlHelper(context);
                string url = string.Concat(Settings.RootUrl, urlHelper.RouteUrl("OpenId"));
                string xml = Xrds.FormatWith(url);

                cacheItem = new HandlerCacheItem { Content = xml };

                if ((CacheDurationInDays > 0) && !Cache.Contains(CacheKey))
                {
                    Cache.Set(CacheKey, cacheItem, SystemTime.Now().AddDays(CacheDurationInDays));
                }
            }

            if (GenerateETag)
            {
                if (HandleIfNotModified(context, cacheItem.ETag))
                {
                    return;
                }
            }

            if (Compress)
            {
                context.CompressResponse();
            }

            HttpResponseBase response = context.Response;

            response.ContentType = "application/xrds+xml";
            response.Write(cacheItem.Content);

            if (CacheDurationInDays > 0)
            {
                if (GenerateETag)
                {
                    response.Cache.SetETag(cacheItem.ETag);
                }

                context.CacheResponseFor(TimeSpan.FromDays(CacheDurationInDays));
            }

            Log.Info("Xrds Requested.");
        }
    }
}