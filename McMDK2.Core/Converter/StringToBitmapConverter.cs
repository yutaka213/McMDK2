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

namespace McMDK2.Core.Converter
{
    public class StringToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            string imagepath = (string)value;
            var image = new BitmapImage();
            image.BeginInit();

            if (imagepath.Split(';').Length == 2)
            {
                string[] path = imagepath.Split(';');
                var template = TemplateManager.GetTemplateFromId(path[0]);
                image.StreamSource = Assembly.GetAssembly(template.GetType()).GetManifestResourceStream(path[1]);
            }
            else
            {
                image.UriSource = new Uri(imagepath);
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
