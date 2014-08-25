using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using McMDK2.Utils.Data.Project.Internal;

namespace McMDK2.Utils.Data.Project
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
        List<Item> Items { get; }

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
