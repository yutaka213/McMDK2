using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Core.Extensions;
using McMDK2.Core.Migrations;

#pragma warning disable 1591

namespace McMDK2.Core.Data
{
    /// <summary>
    /// プロジェクトのアイテムのデータを保持します。
    /// </summary>
    public class ProjectItem : ICloneable
    {
        /// <summary>
        /// アイテム名を設定します。
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// アイテムのファイルタイプを設定します。
        /// </summary>
        public string FileType { set; get; }

        /// <summary>
        /// アイテムのファイルパスを設定します。
        /// </summary>
        public string FilePath { set; get; }

        /// <summary>
        /// アイテムごとの固有IDを設定します。
        /// </summary>
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
