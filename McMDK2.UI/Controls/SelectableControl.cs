using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.UI.Controls
{
    /// <summary>
    /// 選択可能なコントロールの基本となるクラスです。 <para />
    /// 以下のコントロールで使用されます。<para />
    /// <list type="bullet">
    ///     <item>
    ///         <description>ComboBox</description>
    ///     </item>
    /// </list>
    /// </summary>
    public class SelectableControl : EnterableControl
    {
        /// <summary>
        /// 選択可能なアイテムのコレクションを設定します。<para />
        /// </summary>
        public string ItemsSource { set; get; }


        public SelectableControl(GuiComponents components)
            : base(components)
        {
            this.Content = null;
        }
    }
}
