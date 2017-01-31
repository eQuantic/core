using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Caching;
using eQuantic.Core.Cache;

namespace eQuantic.Core.Web.Cache
{
    public class MemoryCacheManager : ICaching
    {
        public void Add<T>(T @object, string key, double timeout)
        {
            HttpContext.Current.Cache.Insert(
                key,
                @object,
                null,
                DateTime.Now.AddMinutes(timeout),
                System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        public void Add<T>(T @object, string key)
        {
            HttpContext.Current.Cache.Insert(
                key,
                @object,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        public void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        public bool IsNull(string key)
        {
            return HttpContext.Current.Cache[key] == null;
        }

        public T Get<T>(string key)
        {
            try
            {
                return (T)HttpContext.Current.Cache[key];
            }
            catch
            {
                return default(T);
            }
        }

        public string[] AllKeys()
        {
            return (from DictionaryEntry dict in HttpContext.Current.Cache
                    let key = dict.Key.ToString()
                    select key).ToArray();
        }
    }
}