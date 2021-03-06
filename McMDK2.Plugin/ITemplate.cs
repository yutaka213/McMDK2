﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin.Process;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 「新規作成ウィザード」で作成可能なテンプレートを定義します。
    /// </summary>
    public interface ITemplate : IItem
    {
        /// <summary>
        /// テンプレートの名前を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// プラグインの依存関係を取得します。<para />
        /// "ID1;ID2..."という風に、";"区切りで記述してください。
        /// </summary>
        string Dependents { get; }

        /// <summary>
        /// テンプレートの説明文を取得します。
        /// </summary>
        string Description { get; }

        /// <summary>
        /// テンプレートのファイル群がまとめられたZIPファイルのパスを取得します。 <para />
        /// アセンブリ内に埋め込まれたファイルの場合は"assembly;filepath"、<para />
        /// 埋め込まれていないファイルの場合は"file;filepath" もしくは "filepath" と指定します。
        /// </summary>
        string TemplateFile { get; }

        /// <summary>
        /// プロジェクトタイプを取得します。<para />
        /// "Gradle" (1.7以降) もしくは "Mcp" (1.3.2～1.7), "None" (使用しない) のどれかを指定します。<para />
        /// デフォルトは"Gradle"です。
        /// </summary>
        string ProjectType { get; }

        /// <summary>
        /// 新規プロジェクト作成プロセスの開始直後に呼び出されます。<para />
        /// </summary>
        void PreInitialization(PreInitializationArgs args);

        /// <summary>
        /// TemplateFile で指定したファイルの展開直後に呼び出されます。 <para />
        /// </summary>
        void Initialization(InitializationArgs argsBase);

        /// <summary>
        /// 新規プロジェクト作成プロセスの終了直前に呼び出されます。<para />
        /// </summary>
        void PostInitialization(PostInitializationArgs argsBase);
    }
}
