using System.Linq;
using System.Web;
using eQuantic.Core.Cache;

namespace eQuantic.Core.Web.Cache
{
    public class RequestCacheManager : ICaching
    {
        public void Add<T>(T @object, string key)
        {
            HttpContext.Current.Items.Add(key, @object);
        }

        public void Add<T>(T @object, string key, double timeout)
        {
            HttpContext.Current.Items.Add(key, @object);
        }

        public void Remove(string key)
        {
            HttpContext.Current.Items.Remove(key);
        }

        public bool IsNull(string key)
        {
            return !HttpContext.Current.Items.Contains(key);
        }

        public T Get<T>(string key)
        {
            return (T)HttpContext.Current.Items[key];
        }

        public string[] AllKeys()
        {
            return HttpContext.Current.Items.Keys.Cast<string>().ToArray();
        }
    }
}