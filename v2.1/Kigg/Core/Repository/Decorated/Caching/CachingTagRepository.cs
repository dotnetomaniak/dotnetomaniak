namespace Kigg.Repository
{
    using System.Collections.Generic;

    using DomainObjects;
    using Infrastructure;

    public class CachingTagRepository : DecoratedTagRepository
    {
        private readonly float _cacheDurationInMinutes;

        public CachingTagRepository(ITagRepository innerRepository, float cacheDurationInMinutes) : base(innerRepository)
        {
            Check.Argument.IsNotNegativeOrZero(cacheDurationInMinutes, "cacheDurationInMinutes");

            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public override ICollection<ITag> FindByUsage(int top)
        {
            Check.Argument.IsNotNegativeOrZero(top, "top");

            string cacheKey = "tagsByUsage:{0}".FormatWith(top);

            ICollection<ITag> result;

            Cache.TryGet(cacheKey, out result);

            if (result == null)
            {
                result = base.FindByUsage(top);

                if ((!result.IsNullOrEmpty()) && (!Cache.Contains(cacheKey)))
                {
                    Cache.Set(cacheKey, result, SystemTime.Now().AddMinutes(_cacheDurationInMinutes));
                }
            }

            return result;
        }
    }
}