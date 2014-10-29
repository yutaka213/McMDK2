using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Net
{
    public class SimpleCache
    {
        private static List<Cache> Caches = new List<Cache>();

        // 12 hour
        private static TimeSpan Expires = new TimeSpan(12, 0, 0);

        public static void AddCache(string url, string content)
        {
            Caches.Add(new Cache(url, content));
        }

        public static string GetCache(string url)
        {
            var cache = Caches.SingleOrDefault(w => w.Url == url);
            if (cache == null || (cache != null && cache.UpdatedTime + Expires <= DateTime.Now))
            {
                return null;
            }
            return Caches.Single(w => w.Url == url) == null ? null : Caches.Single(w => w.Url == url).Content;
        }

        private class Cache
        {
            public string Url { private set; get; }

            public DateTime UpdatedTime { private set; get; }

            public string Content { private set; get; }

            public Cache(string url, string content)
            {
                this.Url = url;
                this.UpdatedTime = DateTime.Now;
                this.Content = content;
            }
        }
    }
}
