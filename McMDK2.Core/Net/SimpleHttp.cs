using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace McMDK2.Core.Net
{
    public static class SimpleHttp
    {
        /// <summary>
        /// HTTP GETを行います。
        /// </summary>
        public static string Get(string url, string contentType = "application/x-www-form-urlencoded", IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            if (url == null)
                throw new ArgumentException("url");

            if (!String.IsNullOrEmpty(SimpleCache.GetCache(url + "?" + GetParameter(parameters))))
            {
                return SimpleCache.GetCache(url + "?" + GetParameter(parameters));
            }
            var request = (HttpWebRequest)WebRequest.Create(url + "?" + GetParameter(parameters));
            request.Method = "GET";
            request.ContentType = contentType;

            var response = (HttpWebResponse)request.GetResponse();
            var r = (new StreamReader(response.GetResponseStream())).ReadToEnd();
            SimpleCache.AddCache(url + "?" + GetParameter(parameters), r);
            return r;
        }

        /// <summary>
        /// HTTP POST行います。
        /// </summary>
        public static string Post(string url, string contentType = "application/x-www-form-urlencoded", IEnumerable<KeyValuePair<string, object>> body = null)
        {
            if (url == null)
                throw new ArgumentException("url");

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = contentType;

            byte[] data = Encoding.UTF8.GetBytes(SimpleHttp.GetParameter(body));
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return (new StreamReader(response.GetResponseStream())).ReadToEnd();
        }

        public static string Put(string url, string contentType = "application/x-www-form-urlencoded", IEnumerable<KeyValuePair<string, object>> body = null)
        {
            if (url == null)
                throw new ArgumentException("url");

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = contentType;

            byte[] data = Encoding.UTF8.GetBytes(SimpleHttp.GetParameter(body));
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return (new StreamReader(response.GetResponseStream())).ReadToEnd();
        }

        public static string Delete(string url, string contentType = "application/x-www-form-urlencoded")
        {
            if (url == null)
                throw new ArgumentException("url");

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.ContentType = contentType;

            var response = (HttpWebResponse)request.GetResponse();

            return (new StreamReader(response.GetResponseStream())).ReadToEnd();
        }

        private static string GetParameter(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            if (parameters != null)
                return String.Join("&", from p in parameters select HttpUtility.UrlEncode(p.Key) + "=" + HttpUtility.UrlEncode(p.Value.ToString()));
            return "";
        }
    }
}
