using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.UI.Controls
{
    /// <summary>
    /// 入力可能なコントロールの基本となるクラスです。 <para />
    /// 以下のコントロールで使用されます。<para />
    /// <list type="bullet">
    ///     <item>
    ///         <description>CheckBox</description>
    ///     </item>
    /// </list>
    /// </summary>
    public class EnterableControl : ContentControl
    {
        // McMDK2.UI.Controls.EnterableControl

        /// <summary>
        /// 入力が必須かどうかを示します。
        /// </summary>
        public bool IsRequired { set; get; }

        /// <summary>
        /// デフォルトのコンテンツの値を設定します。
        /// </summary>
        public object Default { set; get; }

        public EnterableControl(GuiComponents component)
            : base(component)
        {
        }
    }
}
