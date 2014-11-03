using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Core.Extensions;

namespace McMDK2.Core.Data
{
    public class ProjectItem : ICloneable
    {
        public string Name { set; get; }

        public string FileType { set; get; }

        public string FilePath { set; get; }

        public string Id { set; get; }

        /* cutted this item on project explorer. */
        public bool IsCut { set; get; }

        /* should be used ReadOnlyObservableCollection<ProjectItem> ? */
        public ObservableCollection<ProjectItem> Children { set; get; }

        public ProjectItem()
        {
            this.Children = new ObservableCollection<ProjectItem>();
            this.IsCut = false;
        }

        public object Clone()
        {
            var item = new ProjectItem();
            item.Name = this.Name;
            item.FileType = this.FileType;
            item.FilePath = this.FilePath;
            item.Id = this.Id;
            item.IsCut = this.IsCut;
            item.Children = this.Children.Clone();
            return item;
        }
    }
}
