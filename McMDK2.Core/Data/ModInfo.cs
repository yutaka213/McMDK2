using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

#pragma warning disable 1591

namespace McMDK2.Core.Data
{
    /// <summary>
    /// mcmod.infoのデータを保持します。
    /// </summary>
    public class ModInfo
    {
        /// <summary>
        /// ModIDを設定します。
        /// </summary>
        [JsonProperty("modId")]
        public string ModId { set; get; }

        /// <summary>
        /// Mod名を設定します。
        /// </summary>
        [JsonProperty("name")]
        public string Name { set; get; }

        /// <summary>
        /// Modの紹介文を設定します。
        /// </summary>
        [JsonProperty("description")]
        public string Description { set; get; }

        /// <summary>
        /// ModのUrlを設定します。<para />
        /// 一般的には配布先Urlを指定します。
        /// </summary>
        [JsonProperty("url")]
        public string Url { set; get; }

        /// <summary>
        /// 現在は使用されていません。
        /// </summary>
        [JsonProperty("updateUrl")]
        public string UpdateUrl { set; get; }

        /// <summary>
        /// Modのロゴファイルのパスを設定します。
        /// </summary>
        [JsonProperty("logoFile")]
        public string LogoFile { set; get; }

        /// <summary>
        /// Modのバージョンを設定します。
        /// </summary>
        [JsonProperty("version")]
        public string Version { set; get; }

        /// <summary>
        /// Modの作者を設定します。
        /// </summary>
        [JsonProperty("authorList")]
        public List<string> AuthorList { set; get; }

        /// <summary>
        /// Modのクレジットを設定します。
        /// </summary>
        [JsonProperty("credits")]
        public string Credits { set; get; }

        /// <summary>
        /// Modの依存関係を設定します。
        /// </summary>
        [JsonProperty("parent")]
        public string Parent { set; get; }

        /// <summary>
        /// 現在は使用されていません。
        /// </summary>
        [JsonProperty("screenshots")]
        public string[] ScreenShots { set; get; }

        public ModInfo()
        {
            this.AuthorList = new List<string>();
        }
    }
}
