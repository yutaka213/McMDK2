using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Data
{
    public class Project
    {
        public string Name { set; get; }

        public string Id { set; get; }

        public string Path { set; get; }

        /// <summary>
        /// Version can use 1.0.0 only currently available.
        /// </summary>
        public string Version { set; get; }

        public Dictionary<string, object> UserProperties { set; get; }

        public ObservableCollection<ProjectItem> Items { set; get; }
    }
}
