using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Fireworks.Templates.ViewModels;
using Fireworks.Templates.Views;

using McMDK2.Core;
using McMDK2.Core.Utils;
using McMDK2.Plugin;
using McMDK2.Plugin.Process;
using McMDK2.Plugin.Process.Internal;

using Microsoft.WindowsAPICodePack.Dialogs;

using Newtonsoft.Json.Linq;

// ReSharper disable AssignNullToNotNullAttribute
namespace Fireworks.Templates
{
    /// <summary>
    /// McMDK2 の標準的なプロジェクトのテンプレート
    /// </summary>
    public class LegacyStandardTemplate : ITemplate
    {
        public string Name
        {
            get { return "スタンダードプロジェクト(Minecraft Forge ユニバーサル, Mcp)"; }
        }

        public string Id
        {
            get { return "A520FE62-237C-47A8-BECF-7AF4A561EF1F"; }
        }

        public string Dependents
        {
            get { return null; }
        }

        public string IconPath
        {
            get { return Id + ";Fireworks.Resources.Contract_32xLG.png"; }
        }

        public string Description
        {
            get { return "Minecraft Forge Universalを前提Modとした、Modの作成を行います。" + Environment.NewLine + "Minecraft 1.3.2～1.7を対象としたModを作成することができます。"; }
        }

        public string TemplateFile
        {
            get { return "assembly;Fireworks.Templates.StandardTemplate.zip"; }
        }

        public string ProjectType
        {
            get { return Define.ProjectTypeMcp; }
        }

        #region PreInitialization
        public void PreInitialization(PreInitializationArgs args)
        {
            //
        }
        #endregion

        public void Initialization(InitializationArgs args)
        {
            //
        }

        public void PostInitialization(PostInitializationArgs args)
        {
            //
        }
    }
}
