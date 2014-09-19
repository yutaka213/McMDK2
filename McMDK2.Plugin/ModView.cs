using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace McMDK2.Plugin
{
    /// <summary>
    /// Mod作成時の編集画面の基本クラスです。
    /// </summary>
    public class ModView : ItemView
    {
        /// <summary>
        /// ＼( ・ٹ・)／
        /// </summary>
        public ModView()
        {
            this.ModProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Modのオプション項目を取得します。<para />
        /// Keyには *.java で定義した名前を設定し、Valueにはその値を設定します。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ModProperties { set; get; }

        public override void Initialize(string path)
        {

        }

        public override void Closing()
        {

        }
    }
}
