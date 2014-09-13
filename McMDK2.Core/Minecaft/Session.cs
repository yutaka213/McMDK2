using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace McMDK2.Core.Minecaft
{
    // See this -> launcher.jar/com/mojang/authlib/yggdrasil/YggdrasilMinecraftSessionService.class
    public class Session : ApiService
    {
        public Session()
        {

        }

        /// <summary>
        /// UUIDからUserProfileを取得します。 <para />
        /// 存在しないUUIDの場合、例外が投げられます。
        /// </summary>
        public User GetUserProfileFromUUID(string uuid)
        {
            this.Url = "https://sessionserver.mojang.com/session/minecraft/profile/" + uuid;
            var stream = this.HttpGet(null);
            string json = new System.IO.StreamReader(stream).ReadToEnd();

            var jt = JToken.Parse(json);
            User user = new User
            {
                Id = (string)jt["id"],
                Name = (string)jt["name"]
            };
            return user;
        }

        /// <summary>
        /// ユーザーネームからUserProfileを取得します。 <para />
        /// 存在しないユーザーネームの場合、例外が投げられます。
        /// </summary>
        public User GetUserProfileFromUsername(string username)
        {
            this.Url = "https://api.mojang.com/users/profiles/minecraft/" + username;
            var stream = this.HttpGet(null);
            string json = new System.IO.StreamReader(stream).ReadToEnd();

            var jt = JToken.Parse(json);
            User user = new User
            {
                Id = (string)jt["id"],
                Name = (string)jt["name"]
            };
            return user;

        }

        public class User
        {
            public string Id { set; get; }

            public string Name { set; get; }
        }
    }
}
