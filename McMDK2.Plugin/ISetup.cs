using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Plugin
{
    /// <summary>
    /// Template選択後に行われるセットアップ動作のインターフェイスです。
    /// </summary>
    public interface ISetup
    {
        /// <summary>
        /// ほかのセットアップ動作と被らないユニークなID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// セットアップ時に呼び出されるメソッドです。
        /// </summary>
        /// <param name="path">プロジェクトのホームディレクトリのパス</param>
        /// <param name="version">選択されたバージョン</param>
        void Setup(string path, string version);
    }
}
