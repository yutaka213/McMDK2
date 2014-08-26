using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace McMDK2.Core.Plugin
{
    public class PluginManager
    {
        private static List<IPlugin> plugins = new List<IPlugin>();

        public static IEnumerable<IPlugin> Plugins
        {
            get
            {
                return plugins.AsReadOnly();
            }
        }

        public static void Register(IPlugin plugin)
        {
            try
            {
                Define.GetLogger().Info("Loading plugin : " + plugin.Name + " " + plugin.Version);
                plugin.Loaded();
                plugins.Add(plugin);
            }
            catch (Exception)
            {
                throw new Exception("プラグインの読み込みに失敗しました。 : " + plugin.Id);
            }
        }
    }
}
