using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Data
{
    public class ProjectItem
    {
        public string Name { set; get; }

        public string FileType { set; get; }

        public string FilePath { set; get; }

        /* should be used ReadOnlyObservableCollection<ProjectItem> ? */
        public ObservableCollection<ProjectItem> Children { set; get; }

        public ProjectItem()
        {
            this.Children = new ObservableCollection<ProjectItem>();
        }
    }
}
