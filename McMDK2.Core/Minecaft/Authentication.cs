using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace McMDK2.Core.Minecaft
{
    public class Authentication : ApiService
    {
        public Authentication(string user)
        {
            this.Url = "https://api.mojang.com/profiles/" + user.ToLower();
        }
    }
}
