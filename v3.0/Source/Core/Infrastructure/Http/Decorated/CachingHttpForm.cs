namespace Kigg.Infrastructure
{
    using System;
    using System.Text;

    public class CachingHttpForm : DecoratedHttpForm
    {
        private readonly float _cacheDurationInMinutes;

        public CachingHttpForm(IHttpForm innerHttpForm, float cacheDurationInMinutes) : base(innerHttpForm)
        {
            Check.Argument.IsNotNegativeOrZero(cacheDurationInMinutes, "cacheDurationInMinutes");

            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public override HttpFormResponse Get(HttpFormGetRequest getRequest)
        {
            Check.Argument.IsNotNull(getRequest, "getRequest");

            string cacheKey = BuildCacheKey(getRequest);

            HttpFormResponse response;

            Cache.TryGet(cacheKey, out response);

            if (response == null)
            {
                response = base.Get(getRequest);

                if ((response != null) && !Cache.Contains(cacheKey))
                {
                    Cache.Set(cacheKey, response, SystemTime.Now().AddMinutes(_cacheDurationInMinutes));
                }
            }

            return response;
        }

        public override void GetAsync(HttpFormGetRequest getRequest)
        {
            GetAsync(getRequest, delegate { }, delegate { });
        }

        public override void GetAsync(HttpFormGetRequest getRequest, Action<HttpFormResponse> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotNull(getRequest, "getRequest");

            string cacheKey = BuildCacheKey(getRequest);

            HttpFormResponse response;

            Cache.TryGet(cacheKey, out response);

            if (response == null)
            {
                base.GetAsync(
                                getRequest,
                                r =>
                                {
                                    if ((r!= null) && !Cache.Contains(cacheKey))
                                    {
                                        Cache.Set(cacheKey, r, SystemTime.Now().AddMinutes(_cacheDurationInMinutes));
                                    }

                                    onComplete(r);
                                },
                                onError
                             );
            }
            else
            {
                onComplete(response);
            }
        }

        private static string BuildCacheKey(HttpFormGetRequest getRequest)
        {
            Check.Argument.IsNotInvalidWebUrl(getRequest.Url, "getRequest.Url");

            StringBuilder cacheKeyValue = new StringBuilder(getRequest.Url);

            if (!string.IsNullOrEmpty(getRequest.UserName))
            {
                cacheKeyValue.Append("|" + getRequest.UserName);
            }

            if (!string.IsNullOrEmpty(getRequest.Password))
            {
                cacheKeyValue.Append("|" + getRequest.Password);
            }

            if (!string.IsNullOrEmpty(getRequest.ContentType))
            {
                cacheKeyValue.Append("|" + getRequest.ContentType);
            }

            if (getRequest.Headers.Count > 0)
            {
                foreach(string key in getRequest.Headers)
                {
                    cacheKeyValue.Append("|{0}:{1}".FormatWith(key, getRequest.Headers[key]));
                }
            }

            if (getRequest.Cookies.Count > 0)
            {
                foreach (string key in getRequest.Cookies)
                {
                    cacheKeyValue.Append("|{0}:{1}".FormatWith(key, getRequest.Cookies[key]));
                }
            }

            return "rawHtml:{0}".FormatWith(cacheKeyValue.ToString().Hash());
        }
    }
}