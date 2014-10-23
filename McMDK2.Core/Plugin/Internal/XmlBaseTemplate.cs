using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;
using McMDK2.Plugin.Process;

namespace McMDK2.Core.Plugin.Internal
{
    /// <summary>
    /// McMDK2 のXMLベースプラグインでのテンプレート情報を保持します。<para />
    /// template.xml から読み込まれます。
    /// </summary>
    internal class XmlBaseTemplate : ITemplate
    {
        public string Name { set; get; }

        public string Id { set; get; }

        public string Dependents { set; get; }

        public string IconPath { set; get; }

        public string Description { set; get; }

        public string TemplateFile { set; get; }

        /// <summary>
        /// template.xml Version
        /// </summary>
        public string XmlVersion { set; get; }

        public void Initialization(InitializationArgs args)
        {
        }

        public void PostInitialization(PostInitializationArgs args)
        {
        }

        public void PreInitialization(PreInitializationArgs args)
        {
        }
    }
}
