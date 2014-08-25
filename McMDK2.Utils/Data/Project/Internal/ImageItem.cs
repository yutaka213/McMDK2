using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Utils.Data.Project.Internal
{
    /// <summary>
    /// McMDK2でのModのアイテムのうち、テクスチャを管理します。
    /// </summary>
    public class ImageItem : Item
    {
        public ImageItem()
            : base(ItemCategory.Image, false)
        {
        }

        public override string ToString()
        {
            return System.IO.Path.GetFileName(this.Path);
        }
    }
}
