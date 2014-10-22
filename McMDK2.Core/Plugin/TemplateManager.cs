using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Core.Plugin.Internal;
using McMDK2.Plugin;

namespace McMDK2.Core.Plugin
{
    /// <summary>
    /// テンプレートを管理するクラスです。<para />
    /// ITemplate もしくは XML BASE PLUGIN の PluginType = Template の場合、このクラスで管理されます。
    /// </summary>
    public class TemplateManager
    {
        private static List<ITemplate> templates = new List<ITemplate>();

        public static IEnumerable<ITemplate> Templates
        {
            get
            {
                return templates.AsReadOnly();
            }
        }

        public static ITemplate GetTemplateFromId(string id)
        {
            return templates.Single(w => w.Id == id);
        }

        public static void Register(ITemplate template)
        {
            if (templates.Where(w => w.Id == template.Id).ToArray().Length != 0)
            {
                throw new Exception("既に同じIDをもつテンプレートが登録されています。 : " + template.Id);
            }
            templates.Add(template);
            IdStore.RegisterId(template.Id, template.GetType());
            Define.GetLogger().Info(String.Format("Register Template : {0}({1}).", template.Name, template.Id));
        }
    }
}
