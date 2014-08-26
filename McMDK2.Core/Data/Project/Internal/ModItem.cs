using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using McMDK2.Plugin;

namespace McMDK2.Core.Data.Project.Internal
{
    /// <summary>
    /// McMDK2でのModのアイテムのうち、Mod本体(BlockやItemなど)を管理します。
    /// </summary>
    public class ModItem : Item
    {
        [JsonProperty]
        public Dictionary<string, string> Properties { set; get; }

        [JsonIgnore]
        public IPlugin Plugin { set; get; }

        [JsonProperty]
        public string PluginId { set; get; }

        public ModItem()
            : base(ItemCategory.Mod)
        {
            this.Properties = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            return this.PluginId + ":" + this.UniqueId;
        }
    }
}
