using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using McMDK2.Core.Plugin;
using McMDK2.Core.Plugin.Internal;

#pragma warning disable 1591

namespace McMDK2.Core.Converter
{
    /// <summary>
    /// 文字列の画像パスから画像を返します。
    /// </summary>
    public class StringToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            var imagepath = (string)value;
            var image = new BitmapImage();
            image.BeginInit();

            if (String.IsNullOrWhiteSpace(imagepath))
            {
                image.UriSource = new Uri("pack://application:,,,/Resources/minecraft.png");
            }
            else
            {
                if (imagepath.Split(';').Length == 2)
                {
                    string[] path = imagepath.Split(';');
                    var item = IdStore.GetTypeFromId(path[0]);
                    image.StreamSource = Assembly.GetAssembly(item).GetManifestResourceStream(path[1]);
                }
                else
                {
                    image.UriSource = new Uri(imagepath);
                }
            }
            image.EndInit();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
