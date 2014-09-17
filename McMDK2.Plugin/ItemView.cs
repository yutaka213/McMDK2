using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 拡張子と関連付けられたItem Viewerの基本クラスです。
    /// </summary>
    public abstract class ItemView : UserControl
    {
        /// <summary>
        /// プロジェクト エクスプローラー内のアイテムを選択した際に、新規タブで呼ばれます。
        /// </summary>
        /// <param name="path">対象となるファイルのパス</param>
        public abstract void Build(string path);
    }
}
