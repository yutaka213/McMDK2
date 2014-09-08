using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace McMDK2.UI.Controls
{
    /// <summary>
    /// System.Windows.Controls.TextBlock
    /// </summary>
    public class TextBlockControl : UIControl
    {
        /// <summary>
        /// コントロールの背景色を設定します。
        /// </summary>
        public /*Brush*/object Background { set; get; }

        /// <summary>
        /// コントロールのフォントファミリーを設定します。
        /// </summary>
        public /*FontFamily*/object FontFamiy { set; get; }

        /// <summary>
        /// コントロールのフォントサイズを設定します。
        /// </summary>
        public /*double*/object FontSize { set; get; }

        /// <summary>
        /// コントロールのフォント収縮特性を設定します。
        /// </summary>
        public /*FontStretch*/object FontStretch { set; get; }

        /// <summary>
        /// コントロールのフォントスタイルを設定します。
        /// </summary>
        public /*FontStyle*/object FontStyle { set; get; }

        /// <summary>
        /// コントロールのフォントの太さを設定します。
        /// </summary>
        public /*FontWeight*/object FontWeight { set; get; }

        /// <summary>
        /// テキストの色を設定します。
        /// </summary>
        public /*Brush*/object Foreground { set; get; }

        /// <summary>
        /// コントロール内の空白を設定します。
        /// </summary>
        public /*Thickness*/object Padding { set; get; }

        /// <summary>
        /// テキストの行の高さを設定します。
        /// </summary>
        public /*double*/object LineHeight { set; get; }

        /// <summary>
        /// テキストを設定します。
        /// </summary>
        public string Text { set; get; }

        /// <summary>
        /// テキストの水平方向の配置を示す値を設定します。
        /// </summary>
        public /*TextAlignment*/object TextAlignment { set; get; }

        /// <summary>
        /// テキストに適用する効果を設定します。
        /// </summary>
        public /*TextDecoration*/object TextDecorations { set; get; }

        /// <summary>
        /// テキストがコンテンツエリアいっぱいになったときに使用するトリミング方法を設定します。
        /// </summary>
        public /*TextTrimming*/object TextTrimming { set; get; }

        /// <summary>
        /// テキストの折り返し方法を設定します。
        /// </summary>
        public /*TextWrapping*/object TextWrapping { set; get; }

        public TextBlockControl()
            : base(GuiComponents.TextBlock)
        { }
    }
}
