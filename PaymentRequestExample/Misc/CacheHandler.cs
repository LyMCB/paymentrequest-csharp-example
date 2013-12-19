using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

namespace PaymentRequestExample.Misc
{
    public static class CacheHandler
    {
        private static ObjectCache cache = MemoryCache.Default;

        public static void AddToCache(string cacheName, Object value)
        {
            cache.Add(cacheName, value, new CacheItemPolicy
            {
                Priority = CacheItemPriority.Default,
                AbsoluteExpiration = DateTime.Now.AddDays(1)

            });
        }

        public static object Get(string cacheName, bool cacheoverwrite = false)
        {
            if (cacheoverwrite)
            {
                cache.Remove(cacheName);
                return null;
            }

            return cache.Get(cacheName);
        }

        public static void Remove(string key)
        {
            cache.Remove(key);
        }


        public static void RefreshList(string ip, string methodname)
        {
            int page = 1;

            while (true)
            {
                string cachename = ip + methodname + page;

                bool exists = CacheHandler.Get(cachename) != null ? true : false;

                if (!exists)
                {
                    return;
                }

                else
                {
                    CacheHandler.Remove(cachename);
                    page++;
                }

            }

        }
    }
}