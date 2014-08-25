using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using McMDK2.Plugin;

namespace McMDK2.Utils.Data.Project.Internal
{
    /// <summary>
    /// Modに使用されるアイテムの基本クラスです
    /// </summary>
    public class Item
    {
        [JsonProperty]
        public ItemCategory ItemType { private set; get; }

        [JsonProperty]
        public string UniqueId { set; get; }

        [JsonProperty]
        public string Path { set; get; }

        [JsonIgnore]
        public bool CanPreview { private set; get; }

        public Item(ItemCategory category, bool canPreview = true)
        {
            this.ItemType = category;
            this.CanPreview = canPreview;
        }
    }
}
