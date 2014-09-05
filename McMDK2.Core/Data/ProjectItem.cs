using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Data
{
    public class ProjectItem
    {
        public string Name { set; get; }

        public ItemType ItemType { set; get; }

        public List<ProjectItem> Children { set; get; }

        public ProjectItem()
        {
            this.Children = new List<ProjectItem>();
        }
    }

    public enum ItemType
    {
        Directory,

        Mod,

        Text,

        Image,

        Sound
    }
}
