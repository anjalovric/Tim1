using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace InitialProject.WPF.Converters
{
    public class ActivePageTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Page activePage)
            {
                return activePage.Title;
            }

            return "My Travel";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
