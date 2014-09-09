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
    public abstract class ModView : UserControl
    {
        /// <summary>
        /// Modのオプション項目を取得します。<para />
        /// Keyには *.java で定義した名前を設定し、Valueにはその値を設定します。
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetProperties();
    }
}
