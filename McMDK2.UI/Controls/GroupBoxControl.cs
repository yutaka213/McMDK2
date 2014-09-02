using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.UI.Controls
{
    /// <summary>
    /// GroupBox
    /// </summary>
    public class GroupBoxControl : ContentControl
    {
        /// <summary>
        /// ヘッダーテキストを設定します。
        /// </summary>
        public string Header { set; get; }

        public GroupBoxControl() : base(GuiComponents.GroupBox) { }
    }
}
