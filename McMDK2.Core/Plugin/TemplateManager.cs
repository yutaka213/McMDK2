using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;
using McMDK2.Core;
using McMDK2.Core.Data;

namespace McMDK2.Core.Plugin
{
    public class TemplateManager
    {
        private static List<ITemplate> templates = new List<ITemplate>();
        private static Dictionary<string, ItemType> exts = new Dictionary<string, ItemType>();

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

        public static ItemType GetItemTypeFromExtension(string ext)
        {
            return exts[ext];
        }

        public static void Register(ITemplate template)
        {
            if (templates.Where(w => w.Id == template.Id).ToArray().Length != 0)
            {
                throw new Exception("既に同じIDをもつテンプレートが登録されています。 : " + template.Id);
            }
            templates.Add(template);
            Define.GetLogger().Info(String.Format("Register Template : {0}({1}).", template.Name, template.Id));
        }

        public static void RegisterExtension(string ext, ItemType type)
        {
            if (exts.ContainsKey(ext))
                return;

            exts.Add("." + ext, type);
            Define.GetLogger().Info(String.Format("Register Extension : {0}({1}).", ext, type));
        }
    }
}
