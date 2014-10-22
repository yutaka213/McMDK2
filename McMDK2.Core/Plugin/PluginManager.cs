﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Core.Plugin.Internal;
using McMDK2.Plugin;

namespace McMDK2.Core.Plugin
{
    /// <summary>
    /// プラグイン全般を管理するクラスです.
    /// IPlugin もしくは XML BASE PLUGIN の エントリーポイントクラス は、全てこのクラスで管理されます。
    /// </summary>
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

        public static IPlugin GetPluginFromId(string id)
        {
            return plugins.Single(w => w.Id == id);
        }

        public static void Register(IPlugin plugin)
        {
            try
            {
                if (plugins.Where(w => w.Id == plugin.Id).ToArray().Length != 0)
                {
                    throw new Exception("既に同じIDをもつプラグインが登録されています。 : " + plugin.Id);
                }

                plugin.Loaded();
                plugins.Add(plugin);
                IdStore.RegisterId(plugin.Id, plugin.GetType());
                Define.GetLogger().Info(String.Format("Loading Plugin : {0}({1}).", plugin.Name, plugin.Id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("プラグインの読み込みに失敗しました。 : " + plugin.Id);
            }
        }
    }
}
