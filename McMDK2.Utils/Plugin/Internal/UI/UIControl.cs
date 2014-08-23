using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace McMDK2.Utils.Plugin.Internal.UI
{
    /// <summary>
    /// McMDK2 XML Base PluginのUI情報の基本クラスです。
    /// </summary>
    public class UIControl
    {
        /// <summary>
        /// コントロールの種類
        /// </summary>
        public GuiComponents Component { set; get; }

        /// <summary>
        /// コントロールの名前<para />
        /// コントロールに入力された値を使用する場合は、名前を設定する必要があります。
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// コントロールの高さ
        /// </summary>
        public double Height { set; get; }

        /// <summary>
        /// コントロールの幅
        /// </summary>
        public double Width { set; get; }

        /// <summary>
        /// コントロールが有効かどうか<para />
        /// 使用すべきではありません。
        /// </summary>
        public bool? IsEnabled { set; get; }

        /// <summary>
        /// コントロールの可視状態<para />
        /// 使用すべきではありません。
        /// </summary>
        public bool? IsVisible { set; get; }

        /// <summary>
        /// コントロールの可視状態
        /// </summary>
        public Visibility? Visibility { set; get; }

        /// <summary>
        /// コントロールのGridへの横方向の配置方法
        /// </summary>
        public HorizontalAlignment? HorizontalAlignment { set; get; }

        /// <summary>
        /// コントロールのGridへの縦方向の配置方法
        /// </summary>
        public VerticalAlignment? VerticalAlignment { set; get; }

        /// <summary>
        /// コントロールの余白
        /// </summary>
        public Thickness? Margin { set; get; }

        /// <summary>
        /// コントロールの背景色
        /// </summary>
        public Brush Background { set; get; }

        /// <summary>
        /// コントロールの前景色
        /// </summary>
        public Brush Foreground { set; get; }

        /// <summary>
        /// コントロールのツールチップテキスト
        /// </summary>
        public string ToolTip { set; get; }

        /// <summary>
        /// コントロールの透過度
        /// </summary>
        public double Opacity { set; get; }

        public List<UIControl> Children { set; get; }

        /// <summary>
        /// タグ
        /// </summary>
        public object Tag { set; get; }

        /// <summary>
        /// コントロールへの入力が必須
        /// </summary>
        public bool IsRequired { set; get; }

        public UIControl()
        {
            this.Children = new List<UIControl>();
        }
    }
}
