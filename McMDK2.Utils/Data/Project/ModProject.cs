using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Utils.Data.Project.Internal;

namespace McMDK2.Utils.Data.Project
{
    public class ModProject : IProject
    {
        public string Name { set; get; }

        public string Path { set; get; }

        public List<Item> Items { set; get; }

        public List<string> ItemsPath { set; get; }

        public string Type
        {
            get { return "Mod"; }
        }

        public void Build()
        {

        }
    }
}
