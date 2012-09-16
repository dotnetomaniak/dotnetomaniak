namespace Kigg.Repository
{
    using System;

    using DomainObjects;
    using Infrastructure;

    public class CachingUserRepository : DecoratedUserRepository
    {
        private readonly int _noOfUsersCacheCount;
        private readonly float _usersCacheDurationInMinutes;
        private readonly float _userCacheDurationInMinutes;

        public CachingUserRepository(IUserRepository innerRepository, int noOfUsersCacheCount, float usersCacheDurationInMinutes, float userCacheDurationInMinutes) : base(innerRepository)
        {
            Check.Argument.IsNotNegativeOrZero(noOfUsersCacheCount, "noOfUsersCacheCount");
            Check.Argument.IsNotNegativeOrZero(usersCacheDurationInMinutes, "usersCacheDurationInMinutes");
            Check.Argument.IsNotNegativeOrZero(userCacheDurationInMinutes, "userCacheDurationInMinutes");

            _noOfUsersCacheCount = noOfUsersCacheCount;
            _usersCacheDurationInMinutes = usersCacheDurationInMinutes;
            _userCacheDurationInMinutes = userCacheDurationInMinutes;
        }

        public override decimal FindScoreById(Guid id, DateTime startTimestamp, DateTime endTimestamp)
        {
            Check.Argument.IsNotEmpty(id, "id");
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            string cacheKey = "topUser:{0}-{1}-{2}".FormatWith(id, startTimestamp.ToShortDateString(), endTimestamp.ToShortDateString());
            decimal score;

            Cache.TryGet(cacheKey, out score);

            if (score == 0)
            {
                score = base.FindScoreById(id, startTimestamp, endTimestamp);

                if ((score != 0) && (!Cache.Contains(cacheKey)))
                {
                    Cache.Set(cacheKey, score, SystemTime.Now().AddMinutes(_userCacheDurationInMinutes));
                }
            }

            return score;
        }

        public override PagedResult<IUser> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            string cacheKey = "topUsers:{0}-{1}-{2}-{3}".FormatWith(startTimestamp.ToShortDateString(), endTimestamp.ToShortDateString(), start, max);

            int lastCount = (start + max);
            bool useCache = (lastCount <= _noOfUsersCacheCount);

            PagedResult<IUser> result = null;

            if (useCache)
            {
                Cache.TryGet(cacheKey, out result);
            }

            if (result == null)
            {
                result = base.FindTop(startTimestamp, endTimestamp, start, max);

                if (!result.IsEmpty && useCache && !Cache.Contains(cacheKey))
                {
                    Cache.Set(cacheKey, result, SystemTime.Now().AddMinutes(_usersCacheDurationInMinutes));
                }
            }

            return result;
        }
    }
}