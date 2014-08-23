using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace Recipe
{
    [Export(typeof(IPlugin))]
    public class EntryPoint : IPlugin
    {
        public string Name
        {
            get { return "Crafting Recipe"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
        }

        public string Author
        {
            get { return "tuyapin"; }
        }

        public string Id
        {
            get { return "McMDK.CraftingRecipe"; }
        }

        public string Dependents
        {
            get { return ""; }
        }

        public string IconPath
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Crafting Recipeプラグインは、McMDKにおけるCrafting Tableでのレシピ追加の機能を提供します。"; }
        }

        public void Loaded()
        {

        }

        public void Updated()
        {

        }
    }
}
