﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Core.Minecaft
{
    public class ApiService
    {
        protected string Url { set; get; }

        protected Stream HttpGet(IEnumerable<KeyValuePair<string, string>> param)
        {
            string url = this.Url;
            if (param != null)
            {
                url += "?" + String.Join("&", from p in param select p.Key + "=" + p.Value);
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

        protected Stream HttpPost(IEnumerable<KeyValuePair<string, string>> param, string content = "application/x-www-form-urlencoded")
        {
            string prms = String.Join("&", from p in param select p.Key + "=" + p.Value);
            return HttpPost(prms, content);
        }

        protected Stream HttpPost(string obj, string content = "application/x-www-form-urlencoded")
        {
            var request = (HttpWebRequest)WebRequest.Create(this.Url);
            request.Method = "POST";
            request.Connection = content;

            byte[] d = Encoding.UTF8.GetBytes(obj);
            request.ContentLength = d.Length;

            var stream = request.GetRequestStream();
            stream.Write(d, 0, d.Length);
            stream.Close();

            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }
    }
}
