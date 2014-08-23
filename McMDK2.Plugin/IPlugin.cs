using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Plugin
{
    /// <summary>
    /// 一般的なプラグインのエントリーポイントを定義します。
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// プラグインの名前を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// プラグインのバージョンを取得します。<para />
        /// バージョンが上がった際に、IPlugin.Update()が呼ばれます。
        /// </summary>
        string Version { get; }

        /// <summary>
        /// プラグインの作者を取得します。
        /// </summary>
        string Author { get; }

        /// <summary>
        /// プラグインのユニークIDを取得します。<para />
        /// 他のプラグインを被るIDは使用できません。
        /// </summary>
        string Id { get; }

        /// <summary>
        /// プラグインの依存関係を取得します。<para />
        /// "ID1;ID2..."という風に、";"区切りで記述してください。
        /// </summary>
        string Dependents { get; }

        /// <summary>
        /// プラグインのアイコンのパスを取得します。
        /// </summary>
        string IconPath { get; }

        /// <summary>
        /// プラグインの説明文を取得します。
        /// </summary>
        string Description { get; }

        /// <summary>
        /// プラグインの読み込みが完了した際に呼ばれます。<para />
        /// Modカテゴリーの追加やBuildイベントの登録を行うことができます。
        /// </summary>
        void Loaded();

        /// <summary>
        /// プラグインが更新される際に呼ばれます。
        /// </summary>
        void Updated();
    }
}
