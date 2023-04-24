using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InitialProject.WPF.Converters
{
    public class IntToImageConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = new List<string>();
            if (value is int j)
            {
                for(int i=0; i<j; i++)
                {
                    list.Add("/Resources/Images/goldStar1.png");
                }
            }
            return list;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
