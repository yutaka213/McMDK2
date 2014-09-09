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
    public interface IMod
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
        /// 固有のIDを取得します。
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Modの項目編集時に使用する、Viewを取得します。
        /// </summary>
        ModView View { get; }
    }
}
