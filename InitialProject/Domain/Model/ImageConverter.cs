using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace InitialProject.Model
{
    public sealed class ImageConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /*if (value == null) return null;

            var image = (System.Drawing.Image)value;
            var bitmap = new System.Windows.Media.Imaging.BitmapImage();

            bitmap.BeginInit();
            MemoryStream memoryStream = new MemoryStream();
            image.Add(memoryStream, ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();*/
            return new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute));
            //return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
