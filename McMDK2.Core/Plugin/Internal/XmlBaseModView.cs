using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;
using McMDK2.UI.Controls;

namespace McMDK2.Core.Plugin.Internal
{
    public class XmlBaseModView : ModView
    {
        private List<UIControl> controls;

        public XmlBaseModView(List<UIControl> controls)
        {
            this.controls = controls;
        }

        // Add controls to System.Windows.Controls.ContentControl.
        public void Rendering()
        {

        }
    }
}
