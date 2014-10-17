using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;
using McMDK2.Plugin.Process;

namespace Fireworks.Templates
{
    public class BukkitTemplate : ITemplate
    {
        public string Name
        {
            get { return "スタンダードプロジェクト(Bukkit プラグイン)"; }
        }

        public string Id
        {
            get { return "A69FD32C-BD6B-4C8B-A1A7-D33944CA6984"; }
        }

        public string Dependents
        {
            get { return null; }
        }

        public string IconPath
        {
            get { return Id + ";Fireworks.Resources.Contract_32xLG.png"; }
        }

        public string Description
        {
            get { return "CraftBukkit プラグイン向けのプロジェクトです。"; }
        }

        public string TemplateFile
        {
            get { return Id + ";Fireworks.Templates.BukkitTemplate.zip"; }
        }

        public void PreInitialization(PreInitializationArgs args)
        {
        }

        public void Initialization(InitializationArgs args)
        {
        }

        public void PostInitialization(PostInitializationArgs args)
        {
        }

    }
}
