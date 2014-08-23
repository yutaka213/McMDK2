using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Utils.Plugin.Internal.UI
{
    /// <summary>
    /// McMDK2 XML Base Pluginで使用可能なコントロールの定義です。<para />
    /// ここに定義されているコントロールのみがXML Base Pluginで利用できます。
    /// </summary>
    public enum GuiComponents
    {
        /// <summary>
        /// 以下で定義されている全てに当てはまらない場合、Null(設置しない)となります。
        /// </summary>
        Null,

        /// <summary>
        /// System.Windows.Controls.TextBlock
        /// </summary>
        TextBlock,

        /// <summary>
        /// System.Windows.Controls.Label
        /// </summary>
        Label,

        /// <summary>
        /// System.Windows.Controls.TextBox
        /// </summary>
        TextBox,

        /// <summary>
        /// System.Windows.Controls.CheckBox
        /// </summary>
        CheckBox,

        /// <summary>
        /// System.Windows.Controls.ComboBox
        /// </summary>
        ComboBox,

        /// <summary>
        /// System.Windows.Controls.Image
        /// </summary>
        Image,

        /// <summary>
        /// System.Windows.Controls.Grid
        /// </summary>
        Grid,

        /// <summary>
        /// System.Windows.Controls.GroupBox
        /// </summary>
        GroupBox,

        /// <summary>
        /// System.Windows.Controls.WrapPanel
        /// </summary>
        WrapPanel,

        /// <summary>
        /// System.Windows.Controls.StackPanel
        /// </summary>
        StackPanel,

        /// <summary>
        /// System.Windows.Controls.DockPanel
        /// </summary>
        DockPanel,

        /// <summary>
        /// System.Windows.Controls.Canvas
        /// </summary>
        Canvas,

        /// <summary>
        /// System.Windows.Controls.UniformGrid
        /// </summary>
        UniformGrid,

        /// <summary>
        /// System.Windows.Controls.ScrollViewer
        /// </summary>
        ScrollViewer,

        /// <summary>
        /// System.Windows.Controls.Separator
        /// </summary>
        Separator,
    }
}
