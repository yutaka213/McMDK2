using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

#pragma warning disable 1591

namespace McMDK2.Core.Minecaft
{
    /// <summary>
    /// Mojangのサーバー状況を取得します。<para />
    /// Green     = 生きてる <para />
    /// Red       = 死んでる <para />
    /// Undefined = もうダメ
    /// </summary>
    public class ServerStatus : ApiService
    {
        public ServerStatus()
        {
            this.Url = "http://status.mojang.com/check";
        }

        /// <summary>
        /// Mojang サーバーの全てのステータスを取得します。
        /// </summary>
        public Dictionary<string, Status> GetAllServerStatus()
        {
            var dic = new Dictionary<string, Status>();

            var stream = this.HttpGet(null);
            var sr = new System.IO.StreamReader(stream);

            var jarray = JArray.Parse(sr.ReadToEnd());
            foreach (var item in jarray)
            {
                foreach (JProperty prop in ((JObject)item).Properties())
                {
                    Status status;
                    if ((string)prop.Value == "green")
                    {
                        status = Status.Green;
                    }
                    else if ((string)prop.Value == "yellow")
                    {
                        status = Status.Yellow;
                    }
                    else if ((string)prop.Value == "red")
                    {
                        status = Status.Red;
                    }
                    else
                    {
                        status = Status.Undefined;
                    }
                    if (dic.ContainsKey(prop.Name))
                        continue;
                    dic.Add(prop.Name, status);
                }
            }

            return dic;
        }

        /// <summary>
        /// 指定したサーバーのステータスを取得します。
        /// </summary>
        public Status GetServerStatus(string server)
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("service", server));

            var stream = this.HttpGet(param);
            var sr = new System.IO.StreamReader(stream);

            var o = JObject.Parse(sr.ReadToEnd());
            var s = (string)o[server];
            if (s == "green")
                return Status.Green;
            if (s == "yellow")
                return Status.Yellow;
            if (s == "red")
                return Status.Red;
            return Status.Undefined;
        }
    }

    public enum Status
    {
        Green,

        Yellow,

        Red,

        Undefined
    }
}
