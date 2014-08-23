using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Utils.Plugin.Internal.UI.Controls
{
    /// <summary>
    /// ComboBoxなどの選択式のUIControlのクラスです。
    /// </summary>
    public class SelectionControl : TextControl
    {
        public string ItemsSource { set; get; }
    }
}
