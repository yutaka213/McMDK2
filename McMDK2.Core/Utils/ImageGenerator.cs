using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace McMDK2.Core.Utils
{
    public static class ImageGenerator
    {
        public static object Generate(double height, double width, params string[] images)
        {
            BitmapImage bitmap;

            if (images.Length == 1)
            {
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(images[0]);
                bitmap.EndInit();

                return new Image { Source = bitmap, Height = height, Width = width, UseLayoutRounding = true };
            }

            var grid = new Grid();
            foreach (var image in images)
            {
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(image);
                bitmap.EndInit();

                grid.Children.Add(new Image { Source = bitmap, Height = height, Width = width, UseLayoutRounding = true });
            }
            return grid;
        }
    }
}
