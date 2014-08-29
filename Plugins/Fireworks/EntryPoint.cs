using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Core;
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
            TemplateManager.Register(new SoundTemplate());
        }

        public void Updated() { }
    }
}
