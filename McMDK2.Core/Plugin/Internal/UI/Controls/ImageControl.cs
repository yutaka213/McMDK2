using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace McMDK2.Core.Plugin.Internal.UI.Controls
{
    /// <summary>
    /// ImageSourceをもつUIControlのクラスです。
    /// </summary>
    public class ImageControl : UIControl
    {
        public ImageSource Source { set; get; }
    }
}
