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
            /*
            TemplateManager.RegisterExtension("png", ItemType.Image);
            TemplateManager.RegisterExtension("ogg", ItemType.Sound);
            TemplateManager.RegisterExtension("txt", ItemType.Text);
            TemplateManager.RegisterExtension("lang", ItemType.Text);
            TemplateManager.RegisterExtension("json", ItemType.Mod);
            TemplateManager.RegisterExtension("java", ItemType.Text);
            TemplateManager.RegisterExtension("definer", ItemType.Definer);
            TemplateManager.RegisterExtension("designer", ItemType.Designer);
             */
            // Moved to McMDK2.exe
        }

        public void Updated() { }
    }
}
