using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace McMDK2.Core.Data
{
    public class ModInfo
    {
        [JsonProperty("modId")]
        public string ModId { set; get; }

        [JsonProperty("name")]
        public string Name { set; get; }

        [JsonProperty("description")]
        public string Description { set; get; }

        [JsonProperty("url")]
        public string Url { set; get; }

        [JsonProperty("updateUrl")]
        public string UpdateUrl { set; get; }

        [JsonProperty("logoFile")]
        public string LogoFile { set; get; }

        [JsonProperty("version")]
        public string Version { set; get; }

        [JsonProperty("authorList")]
        public List<string> AuthorList { set; get; }

        [JsonProperty("credits")]
        public string Credits { set; get; }

        [JsonProperty("parent")]
        public string Parent { set; get; }

        [JsonProperty("screenshots")]
        public string[] ScreenShots { set; get; }

        public ModInfo()
        {
            this.AuthorList = new List<string>();
        }
    }
}
