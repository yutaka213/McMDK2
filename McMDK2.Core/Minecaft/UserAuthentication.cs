using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

#pragma warning disable 1591

namespace McMDK2.Core.Minecaft
{
    public class UserAuthentication : ApiService
    {
        private const string uuid = "D0050BD3-1302-4DCC-97B0-D8AF20298010";
        private const string request1 = "\"agent\":\"Minecraft\", \"username\":\"{0}\", \"password\":\"{1}\", \"clientToken\":\"{2}\", \"requestUser\":true";
        private const string request2 = "\"clientToken\":\"{0}\", \"accessToken\":\"{1}\"";

        public UserAuthentication()
        {
            this.Url = "";
        }

        /// <summary>
        /// パスワードを使って、Mojang Accountへログインします。
        /// </summary>
        public UserProfile LoginWithPassword(string username, string password)
        {
            this.Url = "https://authserver.mojang.com/authenticate";
            string json = String.Format(request1, username, password, uuid);

            var stream = this.HttpPost("{" + json + "}", "application/json");

            var jt = JToken.Parse(new System.IO.StreamReader(stream).ReadToEnd());

            var up = new UserProfile
            {
                AccessToken = (string)jt["accessToken"],
                ClientToken = (string)jt["clientToken"]
            };
            {
                var inner = jt["selectedProfile"];
                var profile = new Profile
                {
                    Id = (string)inner["id"],
                    Name = (string)inner["name"]
                };
                up.SelectedProfile = profile;
            }

            {
                var inner = (JArray)jt["availableProfiles"];
                var list = new List<Profile>();
                foreach (var item in (JToken)inner)
                {
                    var profile = new Profile
                    {
                        Id = (string)item["id"],
                        Name = (string)item["name"]
                    };
                    list.Add(profile);
                }
                up.AvailableProfiles = list.ToArray();
            }
            up.UserId = (string)jt["user"]["id"];

            return up;
        }

        public UserProfile LoginWithAuthToken()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// トークンが正常であるかを確認します。
        /// </summary>
        public bool CheckTokenValidity(string clientToken, string accessToken)
        {
            this.Url = "https://authserver.mojang.com/validate";
            string json = String.Format(request2, clientToken, accessToken);

            try
            {
                var stream = this.HttpPost("{" + json + "}", "application/json");
                new System.IO.StreamReader(stream).ReadToEnd();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public UserProfile SelectGameProfile()
        {
            throw new NotImplementedException();
        }
    }

    public class UserProfile
    {
        public string AccessToken { set; get; }

        public string ClientToken { set; get; }

        public Profile SelectedProfile { set; get; }

        public Profile[] AvailableProfiles { set; get; }

        public string UserId { set; get; }
    }

    public class Profile
    {
        public string Id { set; get; }

        public string Name { set; get; }
    }
}
