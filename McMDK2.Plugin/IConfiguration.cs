using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 設定を保存する必要がある祭、ViewModel(DataContext)で実装してください。
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 設定画面読み込み時に呼び出されます。
        /// </summary>
        void Load();

        /// <summary>
        /// 設定適用時(=設定画面の[適用]もしくは[OK]を押した時)に呼び出されます。
        /// </summary>
        void Apply();
    }
}
