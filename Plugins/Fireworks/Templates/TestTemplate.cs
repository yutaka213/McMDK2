using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Core;
using McMDK2.Plugin;
using McMDK2.Plugin.Process;

namespace Fireworks.Templates
{
    /// <summary>
    /// Fireworks.dll で登録される全てのアイテムを含んだテスト用のテンプレートです。<para />
    /// ソリューション構成 Debug の時のみ登録されます。
    /// </summary>
    public class TestTemplate : ITemplate
    {
        public string Name
        {
            get { return "テストプロジェクト(DEBUG)"; }
        }

        public string Id
        {
            get { return "AD9DD43C-B890-4788-950C-20819E6A45EB"; }
        }

        public string Dependents
        {
            get { return null; }
        }

        public string IconPath
        {
            get { return Id + ";Fireworks.Resources.application_32xLG.png"; }
        }

        public string Description
        {
            get { return "テスト用のプロジェクトです。Fireworks.dll及びApp.xaml.csで登録された全てのアイテムが含まれます。"; }
        }

        public string TemplateFile
        {
            get { return "assembly;Fireworks.Templates.TestTemplate.zip"; }
        }

        public string ProjectType
        {
            get { return Define.ProjectTypeNone; }
        }

        public void PreInitialization(PreInitializationArgs args)
        {
        }

        public void Initialization(InitializationArgs args)
        {
        }

        public void PostInitialization(PostInitializationArgs args)
        {
        }
    }
}
