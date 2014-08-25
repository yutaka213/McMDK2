using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Utils.Data.Project.Internal
{
    /// <summary>
    /// McMDK2のアイテムのうち、テキストファイル(*.lang, *.cfg)などを管理します。
    /// </summary>
    public class TextItem : Item
    {
        public TextItem()
            : base(ItemCategory.Text)
        {
        }

        public override string ToString()
        {
            return System.IO.Path.GetFileName(this.Path);
        }
    }
}
