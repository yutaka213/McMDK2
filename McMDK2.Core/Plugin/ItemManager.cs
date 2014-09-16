using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace McMDK2.Core.Plugin
{
    /// <summary>
    /// Text File, Sound File, Java Source Fileなど、Projectに含まれるアイテムの管理を行うクラスです。<para />
    /// 拡張子ごとに判定を行います。
    /// </summary>
    public class ItemManager
    {
        // items(Extension(Including dot '.', Identifier, Object Viewer, Item Icon). 
        private static Dictionary<string, Tuple<string, object/* Interim */, string>> items = new Dictionary<string, Tuple<string, object, string>>();


        /// <summary>
        /// 拡張子から、その拡張子に対応したアイテムタイプ(識別子)を取得します。
        /// </summary>
        public static string GetItemTypeFromExtension(string extension)
        {
            if (!items.ContainsKey(extension))
                return null;
            var item = items[extension];
            return item.Item1;
        }

        /// <summary>
        /// 拡張子から、その拡張子に対応したビューワーを取得します。
        /// </summary>
        public static object/* Interim */ GetItemViewerFromExtension(string extension)
        {
            if (!items.ContainsKey(extension))
                return null;
            var item = items[extension];
            return item.Item2;
        }

        /// <summary>
        /// 拡張子から、その拡張子に対応したアイコンを取得します。
        /// </summary>
        public static string GetIconFromExtension(string extension)
        {
            if (!items.ContainsKey(extension))
                return null;
            var item = items[extension];
            return item.Item3;
        }

        /// <summary>
        /// 識別子から、その識別子に対応したアイコンを取得します。
        /// </summary>
        public static string GetIconFromIdentifier(string identifier)
        {
            var item = items.Where(w => w.Value.Item1 == identifier).ToArray();
            if (item.Length >= 1)
                return item[0].Value.Item3;
            return null;
        }

        /// <summary>
        /// 拡張子を登録します。
        /// </summary>
        public static void Register(string extension, string identifier, object viewer, string iconpath)
        {
            var tuple = new Tuple<string, object, string>(identifier, viewer, iconpath);
            items.Add("." + extension, tuple);
            Define.GetLogger().Info(String.Format("Register Extension : {0}(IDENTIFIER:{1}, VIEWER:{2}, ICONPATH:{3})", extension, identifier, viewer, iconpath));
        }
    }
}
