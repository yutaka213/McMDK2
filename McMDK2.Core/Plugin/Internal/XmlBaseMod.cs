using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace McMDK2.Core.Plugin.Internal
{
    /// <summary>
    /// McMDK2のXMLベースプラグインでのModアイテムのデータを保持します。<para />
    /// ModViewは、ui.xmlから読み込まれたものを、その他のものについてはplugin.xmlから読み込まれたものが適用されます。
    /// </summary>
    public class XmlBaseMod : IMod
    {
        public string Name { set; get; }

        public string Version { set; get; }

        public string Id { set; get; }

        public ModView View { set; get; }

        public string SourceFile { set; get; }

        /// <summary>
        /// mod.xml Version
        /// </summary>
        public string XmlVersion { set; get; }
    }
}
