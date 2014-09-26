using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin.Internal;

namespace McMDK2.Plugin
{
    /// <summary>
    /// Template選択後に行われるセットアップ動作のインターフェイスです。
    /// </summary>
    public abstract class Setup : Progress
    {
        /// <summary>
        /// ほかのセットアップ動作と被らないユニークなID
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// セットアップ時に呼び出されるメソッドです。
        /// </summary>
        /// <param name="path">プロジェクトのホームディレクトリのパス</param>
        /// <param name="version">選択されたバージョン</param>
        public abstract void Process(string path, string version);
    }
}
