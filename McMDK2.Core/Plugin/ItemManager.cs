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
    public class ItemManager
    {
        // exts(Extension(Including dot '.'.), Identifier);
        private static Dictionary<string, string> exts = new Dictionary<string, string>();

        // icons(Identifier, Icon Path(Abs));
        private static Dictionary<string, string> icons = new Dictionary<string, string>();

        // viewers(Extension(Including dot '.'), View)
        private static Dictionary<string, UserControl> viewers = new Dictionary<string, UserControl>();

        public static string GetIdentifierFromExtension(string extension)
        {
            if (!exts.ContainsKey(extension))
                return null;
            return exts[extension];
        }

        public static UserControl GetItemViewFromExtension(string extension)
        {
            if (!viewers.ContainsKey(extension))
                return null;
            return viewers[extension];
        }

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
