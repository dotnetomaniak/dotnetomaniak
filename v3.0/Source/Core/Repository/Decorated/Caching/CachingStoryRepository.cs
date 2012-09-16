namespace Kigg.Repository
{
    using DomainObjects;
    using Infrastructure;

    public class CachingStoryRepository : DecoratedStoryRepository
    {
        private readonly int _noOfPublishedStoryCacheCount;
        private readonly float _publishedStoryCacheDurationInMinutes;
        private readonly int _noOfUpcomingStoryCacheCount;
        private readonly float _upcomingStoryCacheDurationInMinutes;

        public CachingStoryRepository(IStoryRepository innerRepository, int noOfPublishedStoriesCacheCount, float publishedStoriesCacheDurationInMinutes, int noOfUpcomingStoriesCacheCount, float upcomingStoriesCacheDurationInMinutes) : base(innerRepository)
        {
            Check.Argument.IsNotNegativeOrZero(noOfPublishedStoriesCacheCount, "noOfPublishedStoriesCacheCount");
            Check.Argument.IsNotNegativeOrZero(publishedStoriesCacheDurationInMinutes, "publishedStoriesCacheDurationInMinutes");

            Check.Argument.IsNotNegativeOrZero(noOfUpcomingStoriesCacheCount, "noOfUpcomingStoriesCacheCount");
            Check.Argument.IsNotNegativeOrZero(upcomingStoriesCacheDurationInMinutes, "upcomingStoriesCacheDurationInMinutes");

            _noOfPublishedStoryCacheCount = noOfPublishedStoriesCacheCount;
            _publishedStoryCacheDurationInMinutes = publishedStoriesCacheDurationInMinutes;

            _noOfUpcomingStoryCacheCount = noOfUpcomingStoriesCacheCount;
            _upcomingStoryCacheDurationInMinutes = upcomingStoriesCacheDurationInMinutes;
        }

        public override PagedResult<IStory> FindPublished(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            string cacheKey = "publishedStories:{0}-{1}".FormatWith(start, max);

            int lastCount = (start + max);
            bool checkCache = (lastCount <= _noOfPublishedStoryCacheCount);

            PagedResult<IStory> result = null;

            if (checkCache)
            {
                Cache.TryGet(cacheKey, out result);
            }

            if (result == null)
            {
                result = base.FindPublished(start, max);

                if (!result.IsEmpty && checkCache && !Cache.Contains(cacheKey))
                {
                    Cache.Set(cacheKey, result, SystemTime.Now().AddMinutes(_publishedStoryCacheDurationInMinutes));
                }
            }

            return result;
        }

        public override PagedResult<IStory> FindUpcoming(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            string cacheKey = "upcomingStories:{0}-{1}".FormatWith(start, max);

            int lastCount = (start + max);
            bool useCache = (lastCount <= _noOfUpcomingStoryCacheCount);

            PagedResult<IStory> result = null;

            if (useCache)
            {
                Cache.TryGet(cacheKey, out result);
            }

            if (result == null)
            {
                result = base.FindUpcoming(start, max);

                if (!result.IsEmpty && useCache && !Cache.Contains(cacheKey))
                {
                    Cache.Set(cacheKey, result, SystemTime.Now().AddMinutes(_upcomingStoryCacheDurationInMinutes));
                }
            }

            return result;
        }
    }
}