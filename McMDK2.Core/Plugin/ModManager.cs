using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace McMDK2.Core.Plugin
{
    /// <summary>
    /// Mod を管理するクラスです。 <para />
    /// IMod もしくは XML BASE PLUGIN の PluginType = Mod(Default) の場合、このクラスで管理されます。
    /// </summary>
    public class ModManager
    {
        private static List<IMod> mods = new List<IMod>();

        public static IEnumerable<IMod> Mods
        {
            get
            {
                return mods.AsReadOnly();
            }
        }

        public static IMod GetModFromName(string name)
        {
            return mods.Single(w => w.Name == name);
        }

        public static void Register(IMod mod)
        {
            try
            {
                if (mods.Where(w => w.Id == mod.Id).ToArray().Length != 0)
                {
                    throw new Exception("既に同じIDをもつModが登録されています。 : " + mod.Id);
                }
                mods.Add(mod);
                Define.GetLogger().Info(String.Format("Register Mod : {0}({1}).", mod.Name, mod.Id));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
