using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;
using McMDK2.UI.Controls;

namespace McMDK2.Core.Plugin.Internal
{
    /// <summary>
    /// McMDK2 の XML ベースプラグインのデータを保持します。
    /// </summary>
    public class XmlBasePlugin : IPlugin
    {
        public string Name { set; get; }

        public string Version { set; get; }

        public string Author { set; get; }

        public string Id { set; get; }

        public string Dependents { set; get; }

        public string IconPath { set; get; }

        public string Description { set; get; }

        /// <summary>
        /// ui.xml
        /// </summary>
        public List<UIControl> Controls { set; get; }

        /// <summary>
        /// plugin.xml Version
        /// </summary>
        public string XmlVersion { set; get; }

        public void Loaded() { }

        public void Updated() { }
    }
}
