﻿using System;
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
    internal class XmlBasePlugin : IPlugin
    {
        public XmlBasePlugin()
        {
            this.Controls = new List<UIControl>();
        }

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

        /// <summary>
        /// "Template" or "Mod"
        /// </summary>
        public string Type { set; get; }

        public void Loaded() { }

        public void Updated() { }
    }
}
