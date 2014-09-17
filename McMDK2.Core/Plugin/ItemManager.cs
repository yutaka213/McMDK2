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

        // viewers(Extension(Including dot '.'), View( is McMDK2.Plugin.ItemView))
        private static Dictionary<string, ItemView> viewers = new Dictionary<string, ItemView>();

        public static string GetIdentifierFromExtension(string extension)
        {
            if (!exts.ContainsKey(extension))
                return null;
            return exts[extension];
        }

        public static ItemView GetItemViewFromExtension(string extension)
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
