using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 各Modのアイテム(e.g. Block, Item, Tools, Armors...)の基本クラスです。
    /// </summary>
    public interface IMod : IItem
    {
        /// <summary>
        /// Modの名前を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Modアイテムのバージョンを取得します。
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Modの説明を取得します。
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Modの項目編集時に使用する、Viewを取得します。
        /// </summary>
        ModView View { get; }

        /// <summary>
        /// Viewの項目に対応する*.javaソースファイルを取得します。 <para />
        /// 
        /// </summary>
        string SourceFile { get; }
    }
}
