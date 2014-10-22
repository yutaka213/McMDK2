using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 各種アイテムの基底クラスです。
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// アイテムのIDを取得します。
        /// </summary>
        string Id { get; }

        /// <summary>
        /// アイコンのパスを取得します。
        /// </summary>
        string IconPath { get; }
    }
}
