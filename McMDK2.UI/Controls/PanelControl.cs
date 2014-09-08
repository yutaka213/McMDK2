using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace McMDK2.UI.Controls
{
    /// <summary>
    /// System.Windows.Controls.Panel<para />
    /// 以下のコントロールで使用されます。<para />
    /// <list type="bullet">
    ///     <item>
    ///         <description>Grid</description>
    ///     </item>
    ///     <item>
    ///         <description>WrapPanel</description>
    ///     </item>
    ///     <item>
    ///         <description>StackPanel</description>
    ///     </item>
    ///     <item>
    ///         <description>Canvas</description>
    ///     </item>
    ///     <item>
    ///         <description>UniformGrid</description>
    ///     </item>
    ///     <item>
    ///         <description>ScrollViewer</description>
    ///     </item>
    /// </list>
    /// </summary>
    public class PanelControl : UIControl
    {
        /// <summary>
        /// パネルの境界線間の領域の色を指定します。
        /// </summary>
        public /*Brush*/object Backgound { set; get; }

        /// <summary>
        /// このパネルの子要素を設定します。
        /// </summary>
        public List<UIControl> Children { set; get; }

        public PanelControl(GuiComponents component) : base(component) { }
    }
}
