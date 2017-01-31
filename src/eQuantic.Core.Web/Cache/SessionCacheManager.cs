using System.Collections;
using System.Web;
using eQuantic.Core.Cache;

namespace eQuantic.Core.Web.Cache
{
    public class SessionCacheManager : ICaching
    {
        public void Add<T>(T @object, string key)
        {
            HttpContext.Current.Session[key] = @object;
        }

        public void Add<T>(T @object, string key, double timeout)
        {
            HttpContext.Current.Session[key] = @object;
        }

        public void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public bool IsNull(string key)
        {
            return HttpContext.Current.Session[key] == null;
        }

        public T Get<T>(string key)
        {
            return (T) HttpContext.Current.Session[key];
        }

        public string[] AllKeys()
        {
            string[] keys = new string[HttpContext.Current.Session.Keys.Count];
            ((ICollection)HttpContext.Current.Session.Keys).CopyTo(keys, 0);
            return keys;
        }
    }
}