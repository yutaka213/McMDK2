using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 拡張子と関連付けられたItem Viewerの基本クラスです。<para />
    /// このクラスは UserControl のコードビハインドを記述したくない場合に使用してください。<para />
    /// 指定したビューの UserControl.DataContext に設定されます。
    /// </summary>
    public interface ItemViewEx
    {
        /// <summary>
        /// プロジェクト エクスプローラー内のアイテムを選択した際に、新規タブで呼ばれます。
        /// </summary>
        /// <param name="path">対象となるファイルのパス</param>
        void Initialize(string path);

        /// <summary>
        /// このViewが開かれているタブが閉じられる時に呼ばれます。
        /// </summary>
        /*virtual*/
        void Closing();

        /// <summary>
        /// 「保存」ボタンが押された際に呼ばれます。
        /// </summary>
        void Save();
    }
}
