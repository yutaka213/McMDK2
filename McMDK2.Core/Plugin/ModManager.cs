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
    /// Mod を管理するクラスです。 <para />
    /// IMod もしくは XML BASE PLUGIN の PluginType = Mod(Default) の場合、このクラスで管理されます。
    /// </summary>
    public static class ModManager
    {
        private static readonly List<IMod> mods = new List<IMod>();

        /// <summary>
        /// 読み込まれたModのReadonlyなリスト
        /// </summary>
        public static IEnumerable<IMod> Mods
        {
            get
            {
                return mods.AsReadOnly();
            }
        }

        /// <summary>
        /// 固有IDからModを取得します。
        /// </summary>
        public static IMod GetModFromId(string id)
        {
            return mods.Single(w => w.Id == id);
        }

        /// <summary>
        /// Modを登録します。
        /// </summary>
        public static void Register(IMod mod)
        {
            if (mods.Where(w => w.Id == mod.Id).ToArray().Length != 0)
            {
                throw new Exception("既に同じIDをもつModが登録されています。 : " + mod.Id);
            }
            mods.Add(mod);
            IdStore.RegisterId(mod.Id, mod.GetType());
            Define.GetLogger().Info(String.Format("Register Mod : {0}({1}).", mod.Name, mod.Id));
        }
    }
}
