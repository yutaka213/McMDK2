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
    public class StandardTemplate : ITemplate
    {
        public string Name
        {
            get { return "スタンダードプロジェクト(Minecraft Forge ユニバーサル, Gradle)"; }
        }

        public string Id
        {
            get { return "43ECD3FD-7E29-4968-AF55-4C5ED437E7B3"; }
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
            get { return "Minecraft Forge Universalを前提Modとした、Modの作成を行います。" + Environment.NewLine + "Minecraft 1.7以降を対象としたModを作成することができます。"; }
        }

        public string TemplateFile
        {
            get { return "assembly;Fireworks.Templates.StandardTemplate.zip"; }
        }

        public string ProjectType
        {
            get { return Define.ProjectTypeGradle; }
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
