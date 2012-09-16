namespace Kigg.Infrastructure
{
    public class CachingContentService : DecoratedContentService
    {
        private readonly float _contentCacheDurationInMinutes;
        private readonly float _shortUrlCacheDurationInMinutes;

        public CachingContentService(IContentService innerService, float contentCacheDurationInMinutes, float shortUrlCacheDurationInMinutes) : base(innerService)
        {
            Check.Argument.IsNotNegativeOrZero(contentCacheDurationInMinutes, "contentCacheDurationInMinutes");
            Check.Argument.IsNotNegativeOrZero(shortUrlCacheDurationInMinutes, "shortUrlCacheDurationInMinutes");

            _contentCacheDurationInMinutes = contentCacheDurationInMinutes;
            _shortUrlCacheDurationInMinutes = shortUrlCacheDurationInMinutes;
        }

        public override StoryContent Get(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string cacheKey = "content:{0}".FormatWith(url);

            StoryContent result;

            Cache.TryGet(cacheKey, out result);

            if ((result == null) || (result == StoryContent.Empty))
            {
                result = base.Get(url);

                if ((result != StoryContent.Empty) && !Cache.Contains(cacheKey))
                {
                    Cache.Set(cacheKey, result, SystemTime.Now().AddMinutes(_contentCacheDurationInMinutes));
                }
            }

            return result;
        }

        public override string ShortUrl(string url)
        {
            Check.Argument.IsNotEmpty(url, "url");

            string cacheKey = "shortUrl:{0}".FormatWith(url);

            string shortUrl;

            Cache.TryGet(cacheKey, out shortUrl);

            if (string.IsNullOrEmpty(shortUrl))
            {
                shortUrl = base.ShortUrl(url);

                if ((!string.IsNullOrEmpty(shortUrl)) && (!Cache.Contains(cacheKey)))
                {
                    Cache.Set(cacheKey, shortUrl, SystemTime.Now().AddMinutes(_shortUrlCacheDurationInMinutes));
                }
            }

            return shortUrl;
        }
    }
}