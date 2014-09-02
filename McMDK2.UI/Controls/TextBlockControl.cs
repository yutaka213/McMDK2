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
        public Brush Background { set; get; }

        /// <summary>
        /// コントロールのフォントファミリーを設定します。
        /// </summary>
        public FontFamily FontFamiy { set; get; }

        /// <summary>
        /// コントロールのフォントサイズを設定します。
        /// </summary>
        public double FontSize { set; get; }

        /// <summary>
        /// コントロールのフォント収縮特性を設定します。
        /// </summary>
        public FontStretch FontStrertch { set; get; }

        /// <summary>
        /// コントロールのフォントスタイルを設定します。
        /// </summary>
        public FontStyle FontStyle { set; get; }

        /// <summary>
        /// コントロールのフォントの太さを設定します。
        /// </summary>
        public FontWeight FontWeight { set; get; }

        /// <summary>
        /// テキストの色を設定します。
        /// </summary>
        public Brush Foreground { set; get; }

        /// <summary>
        /// コントロール内の空白を設定します。
        /// </summary>
        public Thickness Padding { set; get; }

        /// <summary>
        /// テキストの行の高さを設定します。
        /// </summary>
        public double LineHeight { set; get; }

        /// <summary>
        /// テキストを設定します。
        /// </summary>
        public string Text { set; get; }

        /// <summary>
        /// テキストの水平方向の配置を示す値を設定します。
        /// </summary>
        public TextAlignment TextAlignment { set; get; }

        /// <summary>
        /// テキストに適用する効果を設定します。
        /// </summary>
        public TextDecoration TextDecorations { set; get; }

        /// <summary>
        /// テキストがコンテンツエリアいっぱいになったときに使用するトリミング方法を設定します。
        /// </summary>
        public TextTrimming TextTrimming { set; get; }

        /// <summary>
        /// テキストの折り返し方法を設定します。
        /// </summary>
        public TextWrapping TextWrapping { set; get; }

        public TextBlockControl()
            : base(GuiComponents.TextBlock)
        { }
    }
}
