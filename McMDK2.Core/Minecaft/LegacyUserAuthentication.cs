using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Core.Minecaft
{
    [Obsolete]
    public class LegacyUserAuthentication : ApiService
    {
        public LegacyUserAuthentication()
        {
            this.Url = "https://login.minecraft.net";
        }

        public Dictionary<string, string> Login(string user, string password)
        {
            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("user", user));
            param.Add(new KeyValuePair<string, string>("password", password));
            param.Add(new KeyValuePair<string, string>("version", "14"));

            var stream = this.HttpPost(param);
            string r = new System.IO.StreamReader(stream).ReadToEnd();

            var dic = new Dictionary<string, string>();
            dic.Add("ProfileID", r.Split(':')[4]);
            dic.Add("ProfileName", r.Split(':')[2]);
            dic.Add("SessionToken", r.Split(':')[3]);

            return dic;
        }
    }
}
