using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using McMDK2.Utils.Plugin.Internal.UI;

namespace McMDK2.Utils.Converter
{
    public class StringToObjectConverter
    {

#pragma warning disable 693
        /// <summary>
        /// 文字列から該当するGuiComponentを取得します。
        /// </summary>
        public static GuiComponents StringToComponents(string obj)
        {
            GuiComponents component;
            try
            {
                component = (GuiComponents)Enum.Parse(typeof(GuiComponents), obj);
            }
            catch (Exception)
            {
                component = GuiComponents.Null;
            }
            return component;
        }

        /// <summary>
        /// stringをTypeに変換します。
        /// </summary>
        public static Type StringTo<Type>(string obj, object def = null) where Type : struct
        {
            if (!String.IsNullOrEmpty(obj))
            {
                var converter = TypeDescriptor.GetConverter(typeof(Type));
                var convertFromString = converter.ConvertFromString(obj);
                if (convertFromString != null)
                {
                    return (Type)convertFromString;
                }
            }
            if (def == null)
            {
                return default(Type);
            }
            else
            {
                return (Type)def;
            }
        }

        /// <summary>
        /// 文字列から該当する列挙型を取得します。
        /// </summary>
        public static object StringToEnum(string obj, Type type, object def = null)
        {
            try
            {
                if (!String.IsNullOrEmpty(obj))
                {
                    return Enum.Parse(type, obj);
                }
            }
            catch (Exception)
            {
                if (!String.IsNullOrEmpty(obj))
                {
                    Define.GetLogger().Error(obj + " cannot convert to " + type + ".");
                }
            }
            return def;
        }

        /// <summary>
        /// 文字列から該当するプロパティを取得します。
        /// </summary>
        public static object StringToProperty(string obj, Type type, object def = null)
        {
            try
            {
                if (!String.IsNullOrEmpty(obj))
                {
                    PropertyInfo info = type.GetProperty(obj);
                    if (info != null)
                    {
                        return info.GetValue(null);
                    }
                }
            }
            catch (Exception)
            {
                if (!String.IsNullOrEmpty(obj))
                {
                    Define.GetLogger().Error(obj + " cannot convert to " + type + ".");
                }
            }
            return def;
        }

        /// <summary>
        /// 文字列から該当するBrushを取得します。
        /// </summary>
        public static Brush StringToBrush(string obj, Brush def = null)
        {
            try
            {
                if (String.IsNullOrEmpty(obj))
                {
                    return def;
                }
                if (obj.StartsWith("#"))
                {
                    Color color = new Color();
                    if (obj.Length == 9)
                    {
                        color.A = (byte)Convert.ToInt32(obj.Substring(1, 2), 16);
                        color.R = (byte)Convert.ToInt32(obj.Substring(3, 2), 16);
                        color.G = (byte)Convert.ToInt32(obj.Substring(5, 2), 16);
                        color.B = (byte)Convert.ToInt32(obj.Substring(7, 2), 16);
                    }
                    else if (obj.Length == 7)
                    {
                        color.R = (byte)Convert.ToInt32(obj.Substring(1, 2), 16);
                        color.G = (byte)Convert.ToInt32(obj.Substring(3, 2), 16);
                        color.B = (byte)Convert.ToInt32(obj.Substring(5, 2), 16);
                    }
                    else
                    {
                        color.R = (byte)Convert.ToInt32("00", 16);
                        color.G = (byte)Convert.ToInt32("00", 16);
                        color.B = (byte)Convert.ToInt32("00", 16);
                    }
                    return new SolidColorBrush(color);
                }
                PropertyInfo info = typeof(Brushes).GetProperty(obj);
                return (Brush)info.GetValue(null);
            }
            catch (Exception)
            {
                Define.GetLogger().Error(obj + " cannot convert to Brush.");
            }
            return def;
        }

    }
}
