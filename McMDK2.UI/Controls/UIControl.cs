using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace McMDK2.UI.Controls
{
    /// <summary>
    /// McMDK 2 XML Base Plugin の UI 情報の基本クラスです。 <para />
    /// このクラスは System.Windows.UIElement/System.Windows.FrameworkElement に該当する部分のクラスとなっています。
    /// </summary>
    public class UIControl
    {
        // System.Windows.UIElement

        /// <summary>
        /// コントロールの有効状態を示します。
        /// </summary>
        public bool IsEnabled { set; get; }

        /// <summary>
        /// コントロールが表示されるかどうかを示します。
        /// </summary>
        public bool IsVisible { set; get; }

        /// <summary>
        /// コントロールの不透明率を示します。
        /// </summary>
        public double Opacity { set; get; }

        /// <summary>
        /// コントロールの可視状態を示します。
        /// </summary>
        public Visibility Visibility { set; get; }


        // System.Windows.FrameworkElement

        /// <summary>
        /// コントロールの高さを示します。
        /// </summary>
        public double Height { set; get; }

        /// <summary>
        /// コントロールの水平方向の配置を示します。
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { set; get; }

        /// <summary>
        /// コントロールの外側の余白を設定します。
        /// </summary>
        public Thickness Margin { set; get; }

        /// <summary>
        /// コントロールの名前を設定します。<para />
        /// 入力可能なコントロールで、入力された値を使用する場合は設定する必要があります。
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// ツールヒント・テキストを示します。
        /// </summary>
        public string ToolTip { set; get; }

        /// <summary>
        /// コントロールの垂直方向の配置を示します。
        /// </summary>
        public VerticalAlignment VerticalAlignment { set; get; }

        /// <summary>
        /// コントロールの幅を設定します。
        /// </summary>
        public double Width { set; get; }


        // McMDK2.UI.Controls.UIControl

        /// <summary>
        /// コントロールを示す値を設定します。
        /// </summary>
        public GuiComponents Component { private set; get; }

        public UIControl(GuiComponents component)
        {
            this.Component = component;
        }
    }
}
