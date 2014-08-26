using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Core.Data.Project.Internal;

namespace McMDK2.Core.Data.Project
{
    /// <summary>
    /// プロジェクト作成ウィザードで作成可能なプロジェクトのテンプレートの基本クラスとなります。
    /// </summary>
    public interface IProjectTemplate
    {
        /// <summary>
        /// テンプレートの名前を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 新規作成ウィザードで表示されるアイコンを取得します。
        /// </summary>
        string IconPath { get; }

        /// <summary>
        /// テンプレートの説明文を取得します。
        /// </summary>
        string Description { get; }

        /// <summary>
        /// プロジェクトが作成される際に呼び出されます。
        /// </summary>
        void Initialize(List<Item> template);
    }
}
