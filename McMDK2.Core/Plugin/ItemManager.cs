using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using McMDK2.Plugin;

namespace McMDK2.Core.Plugin
{
    /// <summary>
    /// Text File, Sound File, Java Source Fileなど、Projectに含まれるアイテムの管理を行うクラスです。<para />
    /// 拡張子ごとに判定を行います。
    /// </summary>
    public static class ItemManager
    {
        // exts(Extension(Including dot '.'.), Identifier);
        private static readonly Dictionary<string, string> exts = new Dictionary<string, string>();

        // icons(Identifier, Icon Path(Abs));
        private static readonly Dictionary<string, string> icons = new Dictionary<string, string>();

        // viewers(Extension(Including dot '.'), View)
        private static readonly Dictionary<string, UserControl> viewers = new Dictionary<string, UserControl>();

        /// <summary>
        /// 拡張子から関連付けされたIdentifierを取得します。
        /// </summary>
        public static string GetIdentifierFromExtension(string extension)
        {
            if (!exts.ContainsKey(extension))
                return null;
            return exts[extension];
        }

        /// <summary>
        /// 拡張子から関連付けされたコントロールを取得します。
        /// </summary>
        public static UserControl GetItemViewFromExtension(string extension)
        {
            if (!viewers.ContainsKey(extension))
                return null;
            return viewers[extension];
        }

        /// <summary>
        /// Identifierからアイコンを取得します。
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static string GetIconFromIdentifier(string identifier)
        {
            if (!icons.ContainsKey(identifier))
                return null;
            return icons[identifier];
        }

        /// <summary>
        /// McMDKで扱える拡張子を追加します。<para />
        /// アイコンを表示したい時や、プレビューを使用したいときに登録する必要があります。<para />
        /// ItemView に null を指定すると、NullPage(プレビューが利用できない)が使用されます。
        /// </summary>
        public static void RegisterExtension(string extension, string identifier, ItemView viewer)
        {
            if (exts.ContainsKey(extension))
            {
                Define.GetLogger().Info(String.Format("\"{0}\" is already registered.", extension));
                return;
            }
            extension = "." + extension;
            exts.Add(extension, identifier);
            viewers.Add(extension, viewer);
        }

        /// <summary>
        /// McMDKで扱える拡張子を追加します。<para />
        /// アイコンを表示したい時や、プレビューを使用したいときに登録する必要があります。<para />
        /// ItemView に null を指定すると、NullPage(プレビューが利用できない)が使用されます。
        /// </summary>
        [Obsolete]
        public static void RegisterExtension(string extension, string identifier, UserControl viewer, ItemViewEx viewmodel)
        {
            if (exts.ContainsKey(extension))
            {
                Define.GetLogger().Info(String.Format("\"{0}\" is already registered.", extension));
                return;
            }
            extension = "." + extension;
            exts.Add(extension, identifier);
            viewer.DataContext = viewmodel;
            viewers.Add(extension, viewer);
        }

        /// <summary>
        /// アイコンを登録します。
        /// </summary>
        public static void RegisterIcon(string identifier, string iconpath)
        {
            if (icons.ContainsKey(identifier))
            {
                Define.GetLogger().Info(String.Format("\"{0}\" is already registered.", identifier));
            }
            icons.Add(identifier, iconpath);
        }
    }
}
