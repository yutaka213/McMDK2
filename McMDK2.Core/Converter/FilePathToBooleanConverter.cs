using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

#pragma warning disable 1591

namespace McMDK2.Core.Converter
{
    /// <summary>
    ///  ファイルパスを検査し、ファイルが存在する場合はtrueを返します。
    /// </summary>
    public class FilePathToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            var path = (string)value;
            if (FileController.Exists(path))
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
