using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using McMDK2.Core.Data;

namespace McMDK2.Core.Converter
{
    public class ItemTypeToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            ItemType type = (ItemType)value;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();

            switch (type)
            {
                case ItemType.Directory:
                    bitmap.UriSource = new Uri("pack://application:,,,/Resources/Folder_6222.png");
                    break;

                case ItemType.Text:
                    bitmap.UriSource = new Uri("pack://application:,,,/Resources/Textfile_818_16x.png");
                    break;

                case ItemType.Image:
                    bitmap.UriSource = new Uri("pack://application:,,,/Resources/Image_24x.png");
                    break;

                case ItemType.Sound:
                    bitmap.UriSource = new Uri("pack://application:,,,/Resources/Soundfile_461.png");
                    break;

                default:
                    bitmap.UriSource = new Uri("pack://application:,,,/Resources/Content_6017.png");
                    break;
            }

            bitmap.EndInit();
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
