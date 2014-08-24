using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Utils.Data
{
    /// <summary>
    /// McMDK2のアイテムのうち、音声ファイルを管理します。
    /// </summary>
    public class SoundItem : Item
    {
        public SoundItem()
            : base(ItemCategory.Sound, false)
        {
        }

        public override string ToString()
        {
            return System.IO.Path.GetFileName(this.Path);
        }
    }
}
