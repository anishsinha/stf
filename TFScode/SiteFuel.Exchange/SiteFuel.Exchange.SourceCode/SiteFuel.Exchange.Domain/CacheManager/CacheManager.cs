using System;
using System.Runtime.Caching;

namespace SiteFuel.Exchange.Domain
{
    public static class CacheManager
    {
        private static ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// Get cache from memory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Cache key name</param>
        /// <returns>object</returns>
        public static T Get<T>(string key)
        {
            try
            {
                return (T)Cache[key];
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Set cache in memory
        /// </summary>
        /// <param name="key">Cache key name</param>
        /// <param name="data">Data to set in cache</param>
        /// <param name="cacheTime">Cache time in seconds</param>
        public static void Set(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(cacheTime);

            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Check wether cache is exists or not
        /// </summary>
        /// <param name="key">Cache key name</param>
        /// <returns>bool</returns>
        public static bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// Remove cache from memory
        /// </summary>
        /// <param name="key">Cache key name</param>
        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// Clears all cache objects from memory
        /// </summary>
        public static void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }
    }
}
