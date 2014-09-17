using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Core;
using McMDK2.Core.Data;
using McMDK2.Core.Plugin;
using McMDK2.Plugin;

using Fireworks.Templates;

namespace Fireworks
{

    [Export(typeof(IPlugin))]
    public class EntryPoint : IPlugin
    {
        public string Name
        {
            get { return "Fireworks"; }
        }

        public string Version
        {
            get { return Define.Version; }
        }

        public string Author
        {
            get { return "tuyapin"; }
        }

        public string Id
        {
            get { return "McMDK.Fireworks"; }
        }

        public string Dependents
        {
            get { return null; }
        }

        public string IconPath
        {
            get { return null; }
        }

        public string Description
        {
            get { return "Fireworksプラグインは、McMDKの基本的なプロジェクトテンプレート、リソース、ユーザーインターフェースを提供します。"; }
        }

        public void Loaded()
        {
            //テンプレートの登録
            TemplateManager.Register(new StandardTemplate());
            TemplateManager.Register(new BukkitTemplate());
            TemplateManager.Register(new ServerTemplate());

            //拡張子の登録
            ItemManager.RegisterExtension("png", "IMAGE", null);
            ItemManager.RegisterExtension("ogg", "SOUND", null);
            ItemManager.RegisterExtension("txt", "PLAIN TEXT", null);
            ItemManager.RegisterExtension("lang", "PROPERTIES", null);
            ItemManager.RegisterExtension("properties", "PROPERTIES", null);
            ItemManager.RegisterExtension("json", "JAVASCRIPT OBJECT NOTATION", null);
            ItemManager.RegisterExtension("java", "JAVA SOURCE FILE", null);
            ItemManager.RegisterExtension("", "DIRECTORY", null);

            ItemManager.RegisterIcon("IMAGE", "pack://application:,,,/Resources/Image_24x.png");
            ItemManager.RegisterIcon("SOUND", "pack://application:,,,/Resources/Soundfile_461.png");
            ItemManager.RegisterIcon("PLAIN TEXT", "pack://application:,,,/Resources/Textfile_818_16x.png");
            ItemManager.RegisterIcon("DIRECTORY", "pack://application:,,,/Resources/Folder_6222.png");
        }

        public void Updated() { }
    }
}
