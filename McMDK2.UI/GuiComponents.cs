using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.UI
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
        /// System.Windows.Controls.TextBlock <para />
        /// Ref -> McMDK2.UI.Controls.TextBlockControl
        /// </summary>
        TextBlock,

        /// <summary>
        /// System.Windows.Controls.Label <para />
        /// Ref -> McMDK2.UI.Controls.ContentControl
        /// </summary>
        Label,

        /// <summary>
        /// System.Windows.Controls.TextBox <para />
        /// Ref -> McMDK2.UI.Controls.EnterableControl
        /// </summary>
        TextBox,

        /// <summary>
        /// System.Windows.Controls.CheckBox <para />
        /// Ref -> McMDK2.UI.Controls.EnterableControl
        /// </summary>
        CheckBox,

        /// <summary>
        /// System.Windows.Controls.ComboBox <para />
        /// Ref -> McMDK2.UI.Contnrols.SelectableControl
        /// </summary>
        ComboBox,

        /// <summary>
        /// System.Windows.Controls.Image <para />
        /// Ref -> McMDK2.UI.Controls.ContentControl
        /// </summary>
        Image,

        /// <summary>
        /// System.Windows.Controls.Grid <para />
        /// Ref -> McMDK2.UI.Controls.PanelControl
        /// </summary>
        Grid,

        /// <summary>
        /// System.Windows.Controls.GroupBox <para />
        /// Ref -> McMDK2.UI.Controls.GroupBoxControl
        /// </summary>
        GroupBox,

        /// <summary>
        /// System.Windows.Controls.WrapPanel <para />
        /// Ref -> McMDK2.UI.Controls.PanelControl
        /// </summary>
        WrapPanel,

        /// <summary>
        /// System.Windows.Controls.StackPanel <para />
        /// Ref -> McMDK2.UI.Controls.PanelControl
        /// </summary>
        StackPanel,

        /// <summary>
        /// System.Windows.Controls.DockPanel <para />
        /// Ref -> McMDK2.UI.Controls.PanelControl
        /// </summary>
        DockPanel,

        /// <summary>
        /// System.Windows.Controls.Canvas <para />
        /// Ref -> McMDK2.UI.Controls.PanelControl
        /// </summary>
        Canvas,

        /// <summary>
        /// System.Windows.Controls.Primitives.UniformGrid <para />
        /// Ref -> McMDK2.UI.Controls.PanelControl
        /// </summary>
        UniformGrid,

        /// <summary>
        /// System.Windows.Controls.ScrollViewer <para />
        /// Ref -> McMDK2.UI.Controls.PanelControl
        /// </summary>
        ScrollViewer,

        /// <summary>
        /// System.Windows.Controls.Separator <para />
        /// Ref -> McMDK2.UI.Controls.UIControlEx
        /// </summary>
        Separator,
    }
}
