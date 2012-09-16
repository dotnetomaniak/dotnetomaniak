namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;
    using System.Diagnostics;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

    public class Cache : ICache
    {
        private readonly ICacheManager _manager;

        public Cache(string cacheManagerName) : this(CacheFactory.GetCacheManager(cacheManagerName))
        {
        }

        public Cache(ICacheManager manager)
        {
            Check.Argument.IsNotNull(manager, "manager");

            _manager = manager;
        }

        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _manager.Count;
            }
        }

        public void Clear()
        {
            _manager.Flush();
        }

        public bool Contains(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _manager.Contains(key);
        }

        public T Get<T>(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return (T) _manager.GetData(key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            value = default(T);

            if (_manager.Contains(key))
            {
                object existingValue = _manager.GetData(key);

                if (existingValue != null)
                {
                    value = (T) existingValue;

                    return true;
                }
            }

            return false;
        }

        public void Set<T>(string key, T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            RemoveIfExists(key);

            _manager.Add(key, value);
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            RemoveIfExists(key);

            _manager.Add(key, value, CacheItemPriority.Normal, null, new AbsoluteTime(absoluteExpiration.ToLocalTime()));
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNegativeOrZero(slidingExpiration, "absoluteExpiration");

            RemoveIfExists(key);

            _manager.Add(key, value, CacheItemPriority.Normal, null, new SlidingTime(slidingExpiration));
        }

        public void Remove(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            _manager.Remove(key);
        }

        internal void RemoveIfExists(string key)
        {
            if (_manager.Contains(key))
            {
                _manager.Remove(key);
            }
        }
    }
}