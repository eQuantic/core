using System;
using System.Web;
using eQuantic.Core.Cache;
using Newtonsoft.Json;

namespace eQuantic.Core.Web.Cache
{
    public class CookieCacheManager : ICaching
    {
        public void Add<T>(T @object, string key)
        {
            Add(@object, key, double.MaxValue);
        }

        public void Add<T>(T @object, string key, double timeout)
        {
            HttpCookie httpCookie = new HttpCookie(key);
            httpCookie.Expires = timeout >= Double.MaxValue ? DateTime.MaxValue : DateTime.Now.AddMinutes(timeout);

            if (@object != null)
            {
                httpCookie.Value = JsonConvert.SerializeObject(@object);
            }
            else httpCookie.Value = null;
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        public void Remove(string key)
        {
            Add((object)null, key, -1440);
        }

        public bool IsNull(string key)
        {
            return HttpContext.Current.Request.Cookies[key] == null;
        }

        public T Get<T>(string key)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[key];
            if (httpCookie != null)
            {
                return JsonConvert.DeserializeObject<T>(httpCookie.Value);
            }
            return default(T);
        }

        public string[] AllKeys()
        {
            return HttpContext.Current.Request.Cookies.AllKeys;
        }
    }
}