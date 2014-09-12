using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 「新規作成ウィザード」で作成可能なテンプレートを定義します。
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// テンプレートの名前を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// テンプレートのユニークIDを取得します。 <para />
        /// 他のテンプレートと被るIDは使用できません。
        /// </summary>
        string Id { get; }

        /// <summary>
        /// プラグインの依存関係を取得します。<para />
        /// "ID1;ID2..."という風に、";"区切りで記述してください。
        /// </summary>
        string Dependents { get; }

        /// <summary>
        /// テンプレートのアイコンのパスを取得します。
        /// </summary>
        string IconPath { get; }

        /// <summary>
        /// テンプレートの説明文を取得します。
        /// </summary>
        string Description { get; }

        /// <summary>
        /// テンプレートのファイル群がまとめられたZIPファイルのパスを取得します。 <para />
        /// アセンブリ内に埋め込む場合は"ID;FilePath",埋め込まない場合は"FilePath"とします。
        /// </summary>
        string TemplateFile { get; }
    }
}
