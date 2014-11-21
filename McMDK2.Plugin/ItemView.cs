using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 拡張子と関連付けられたItem Viewerの基本クラスです。<para />
    /// このクラスは UserControl 単体完結用です。<para />
    /// 何らかの理由でUserControlのコードビハインドを記述したくない場合は、ItemViewExを継承したクラスを使用してください。
    /// </summary>
    public abstract class ItemView : UserControl
    {
        /// <summary>
        /// プロジェクト エクスプローラー内のアイテムを選択した際に、新規タブで呼ばれます。
        /// </summary>
        /// <param name="path">対象となるファイルのパス</param>
        public abstract void Initialize(string path);

        /// <summary>
        /// このViewが開かれているタブが閉じられる時に呼ばれます。
        /// </summary>
        public abstract/*virtual*/ void Closing();

        /// <summary>
        /// 「保存」ボタンが押された時に呼ばれます。
        /// </summary>
        public abstract void Save();
    }
}
