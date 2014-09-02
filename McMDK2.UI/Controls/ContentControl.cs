using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.UI.Controls
{
    /// <summary>
    /// コンテンツをもつコントロールの基本となるクラスです。 <para />
    /// 以下のコントロールで使用されます。<para />
    /// <list type="bullet">
    ///     <item>
    ///         <description>Image</description>
    ///     </item>
    ///     <item>
    ///         <description>Label</description>
    ///     </item>
    /// </list>
    /// </summary>
    public class ContentControl : UIControlEx
    {
        // System.Windows.Controls.ContentControl

        /// <summary>
        /// コンテンツ
        /// </summary>
        public object Content { set; get; }

        public ContentControl(GuiComponents component)
            : base(component)
        {

        }
    }
}
