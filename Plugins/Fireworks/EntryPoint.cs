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

using Fireworks.ItemViewers.Views;
using Fireworks.ItemViewers.ViewModels;
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
#if DEBUG
            TemplateManager.Register(new TestTemplate());
#endif
            //TemplateManager.Register(new BukkitTemplate());
            //TemplateManager.Register(new ServerTemplate());
            //拡張子の登録
            ItemManager.RegisterExtension("png", "IMAGE", new ImagePage(), new ImagePageViewModel());
            ItemManager.RegisterExtension("ogg", "SOUND", null);               // Preview unavailable
            ItemManager.RegisterExtension("txt", "PLAIN TEXT", null);
            ItemManager.RegisterExtension("lang", "PROPERTIES", null);
            ItemManager.RegisterExtension("properties", "PROPERTIES", null);
            ItemManager.RegisterExtension("json", "SCRIPT FILE", null);
            ItemManager.RegisterExtension("java", "SCRIPT FILE", null);
            ItemManager.RegisterExtension("jar", "JAR FILE", null);            // Preview unavailable
            ItemManager.RegisterExtension("xml", "XML FILE", null);
            ItemManager.RegisterExtension("page", "HTML PAGE", null);
            ItemManager.RegisterExtension("html", "HTML PAGE", null);
            ItemManager.RegisterExtension("js", "JAVASCRIPT", null);
            ItemManager.RegisterExtension("", "DIRECTORY", null);              // Preview unavailable

            ItemManager.RegisterIcon("IMAGE", this.Id + ";Fireworks.Resources.Image_24x.png");
            ItemManager.RegisterIcon("SOUND", this.Id + ";Fireworks.Resources.Soundfile_461.png");
            ItemManager.RegisterIcon("PLAIN TEXT", this.Id + ";Fireworks.Resources.Textfile_818_16x.png");
            ItemManager.RegisterIcon("PROPERTIES", this.Id + ";Fireworks.Resources.document_16xLG.png");
            ItemManager.RegisterIcon("SCRIPT FILE", this.Id + ";Fireworks.Resources.ScriptFile_452.png");
            ItemManager.RegisterIcon("JAR FILE", this.Id + ";Fireworks.Resources.JARFile_464.png");
            ItemManager.RegisterIcon("XML FILE", this.Id + ";Fireworks.Resources.XMLFile_828_16x.png");
            ItemManager.RegisterIcon("HTML PAGE", this.Id + ";Fireworks.Resources.HTMLPage(HTM)_825_16x_color.png");
            ItemManager.RegisterIcon("JAVASCRIPT", this.Id + ";Fireworks.Resources.LanguageOverlay_JS_32xSM.png");
            ItemManager.RegisterIcon("DIRECTORY", "pack://application:,,,/Resources/Folder_6222.png");
        }

        public void Updated() { }
    }
}
