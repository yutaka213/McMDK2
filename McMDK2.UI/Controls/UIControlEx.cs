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
    /// McMDK 2 XML Base Plugin の UI 情報の基本クラスです。 <para />
    /// このクラスは System.Windows.Controls.Control に該当する部分のクラスとなっています。
    /// </summary>
    public class UIControlEx : UIControl
    {
        /// <summary>
        /// コントロールの背景色を設定します。
        /// </summary>
        public /*Brush*/object Background { set; get; }

        /// <summary>
        /// コントロールの境界線の背景色を設定します。
        /// </summary>
        public /*Brush*/object BorderBrush { set; get; }

        /// <summary>
        /// コントロールの境界線の太さを設定します。
        /// </summary>
        public /*Thickness*/object BorderThickess { set; get; }

        /// <summary>
        /// コントロールのフォントファミリーを設定します。
        /// </summary>
        public /*FontFamily*/object FontFamily { set; get; }

        /// <summary>
        /// コントロールのフォントサイズを設定します。
        /// </summary>
        public double FontSize { set; get; }

        /// <summary>
        /// コントロールのフォントの縮小/拡大する度合いを設定します。
        /// </summary>
        public /*FontStretch*/object FontStretch { set; get; }

        /// <summary>
        /// コントロールのフォントの太さを設定します。
        /// </summary>
        public /*FontWeight*/object FontWeight { set; get; }

        /// <summary>
        /// コントロールの前景色を設定します。
        /// </summary>
        public /*Brush*/object Foreground { set; get; }

        /// <summary>
        /// コントロール内側の空白を設定します。
        /// </summary>
        public /*Thickness*/object Padding { set; get; }

        public UIControlEx(GuiComponents component)
            : base(component)
        {
        }
    }
}
