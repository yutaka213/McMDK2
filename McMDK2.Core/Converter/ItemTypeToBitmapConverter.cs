﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using McMDK2.Core.Data;
using McMDK2.Core.Plugin;

namespace McMDK2.Core.Converter
{
    public class ItemTypeToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            string identifier = (string)value;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();

            string iconPath = ItemManager.GetIconFromIdentifier(identifier);
            if (String.IsNullOrEmpty(iconPath))
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/Content_6017.png");
            else
                bitmap.UriSource = new Uri(iconPath);

            bitmap.EndInit();
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
