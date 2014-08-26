using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using McMDK2.Core.Data.Project.Internal;

namespace McMDK2.Core.Data.Project
{
    public interface IProject
    {
        /// <summary>
        /// プロジェクトの名前
        /// </summary>
        [JsonProperty]
        string Name { get; }

        /// <summary>
        /// プロジェクトのルートパス
        /// </summary>
        [JsonProperty]
        string Path { get; }

        /// <summary>
        /// プロジェクトのタイプ
        /// </summary>
        [JsonProperty]
        string Type { get; }

        /// <summary>
        /// プロジェクトに含まれるアイテム
        /// </summary>
        [JsonIgnore]
        ObservableCollection<Item> Items { get; }

        /// <summary>
        /// プロジェクトに含まれるアイテムが定義されているそれぞれのJSONへのパス
        /// </summary>
        [JsonProperty]
        List<string> ItemsPath { get; }

        /// <summary>
        /// ビルド時に呼ばれます。
        /// </summary>
        void Build();
    }
}
