using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core.Data.Project.Internal
{
    /// <summary>
    /// McMDK2でのアイテムのうち、フォルダー階層を示します。
    /// </summary>
    public class FolderItem : Item
    {
        public FolderItem()
            : base(ItemCategory.Folder)
        {
        }

        public override string ToString()
        {
            return this.UniqueId;
        }
    }
}
