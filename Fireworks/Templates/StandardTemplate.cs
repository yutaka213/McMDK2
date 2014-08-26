using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Core.Data.Project;
using McMDK2.Core.Data.Project.Internal;

namespace Fireworks.Templates
{
    /// <summary>
    /// McMDK2 の標準的なプロジェクトのテンプレート
    /// </summary>
    public class StandardTemplate : IProjectTemplate
    {
        public string Name
        {
            get { return "スタンダードプロジェクト"; }
        }

        public string IconPath
        {
            get { return null; }
        }

        public string Description
        {
            get { return "標準的なModのプロジェクトです。通常はこのプロジェクトを使用してください。"; }
        }

        public void Initialize(List<Item> template)
        {
            template.Add(new FolderItem { UniqueId = "Mods" });
            template.Add(new FolderItem { UniqueId = "Textures" });
            template.Add(new FolderItem
            {
                UniqueId = "Texts",
                Children = new System.Collections.ObjectModel.ObservableCollection<Item>
                {
                    new TextItem { UniqueId = "en_US.lang", Path = "%PROJECTROOT%\\langs\\en_US.lang" },
                    new TextItem { UniqueId = "ja_JP.lang", Path = "%PROJECTROOT%\\langs\\ja_JP.lang" }
                }
            });
        }
    }
}
