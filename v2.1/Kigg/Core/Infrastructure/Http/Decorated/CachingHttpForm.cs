namespace Kigg.Infrastructure
{
    using System;

    public class CachingHttpForm : DecoratedHttpForm
    {
        private readonly float _cacheDurationInMinutes;

        public CachingHttpForm(IHttpForm innerHttpForm, float cacheDurationInMinutes) : base(innerHttpForm)
        {
            Check.Argument.IsNotNegativeOrZero(cacheDurationInMinutes, "cacheDurationInMinutes");

            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public override string Get(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string cacheKey = BuildCacheKey(url);

            string result;

            Cache.TryGet(cacheKey, out result);

            if (string.IsNullOrEmpty(result))
            {
                result = base.Get(url);

                if ((!string.IsNullOrEmpty(result)) && !Cache.Contains(cacheKey))
                {
                    Cache.Set(cacheKey, result, SystemTime.Now().AddMinutes(_cacheDurationInMinutes));
                }
            }

            return result;
        }

        public override void GetAsync(string url, Action<string> onComplete, Action<Exception> onError)
        {
            string cacheKey = BuildCacheKey(url);

            string result;

            Cache.TryGet(cacheKey, out result);

            if (string.IsNullOrEmpty(result))
            {
                base.GetAsync(
                                url,
                                response =>
                                {
                                    if ((!string.IsNullOrEmpty(response)) && !Cache.Contains(cacheKey))
                                    {
                                        Cache.Set(cacheKey, response, SystemTime.Now().AddMinutes(_cacheDurationInMinutes));
                                    }

                                    onComplete(response);
                                },
                                onError
                             );
            }
            else
            {
                onComplete(result);
            }
        }

        private static string BuildCacheKey(string url)
        {
            return "rawHtml:{0}".FormatWith(url);
        }
    }
}