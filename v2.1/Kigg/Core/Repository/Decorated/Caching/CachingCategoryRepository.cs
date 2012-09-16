namespace Kigg.Repository
{
    using System.Collections.Generic;

    using DomainObjects;
    using Infrastructure;

    public class CachingCategoryRepository : DecoratedCategoryRepository
    {
        private readonly float _cacheDurationInMinutes;

        public CachingCategoryRepository(ICategoryRepository innerRepository, float cacheDurationInMinutes) : base(innerRepository)
        {
            Check.Argument.IsNotNegativeOrZero(cacheDurationInMinutes, "cacheDurationInMinutes");

            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public override ICollection<ICategory> FindAll()
        {
            const string CacheKey = "categories:All";

            ICollection<ICategory> result;

            Cache.TryGet(CacheKey, out result);

            if (result == null)
            {
                result = base.FindAll();

                if ((!result.IsNullOrEmpty()) && (!Cache.Contains(CacheKey)))
                {
                    Cache.Set(CacheKey, result, SystemTime.Now().AddMinutes(_cacheDurationInMinutes));
                }
            }

            return result;
        }
    }
}