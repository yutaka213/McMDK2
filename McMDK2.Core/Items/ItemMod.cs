using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Items
{
    /// <summary>
    /// Mod(*.mod) のデータを保持します。
    /// </summary>
    [DataContract]
    public class ItemMod
    {
        /// <summary>
        /// Mod(*.mod)ごとの固有のID <para />
        /// 明示的に生成しない場合は、Guid.NewGuid()などを使用して生成する必要があります。
        /// </summary>
        [DataMember]
        public string Id { set; get; }

        /// <summary>
        /// Mod(*.mod)のプロパティ値 <para />
        /// *.javaなどの生成時に使用される、入力ボックスに入力された値とキー値
        /// </summary>
        [DataMember]
        public Dictionary<string, object> Properties { set; get; }

        /// <summary>
        /// このModのデータを生成/処理するプラグインのユニークなID
        /// </summary>
        [DataMember]
        public string PluginId { set; get; }

        /// <summary>
        /// このModのデータを生成/処理するプラグインのバージョン
        /// </summary>
        [DataMember]
        public string PluginVersion { set; get; }
    }
}
