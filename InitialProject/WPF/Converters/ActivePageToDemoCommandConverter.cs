using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using InitialProject.WPF.Demo;

namespace InitialProject.WPF.Converters
{
    public class ActivePageToDemoCommandConverter : IValueConverter
    {
        public RelayCommand AccommodationInputCommand { get; set; }
        public RelayCommand ScheduleRenovationCommand { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MakeCommands();
            if(value is Page activePage)
            {
                if (activePage.Title.Equals("Enter Accommodation"))
                    return AccommodationInputCommand;
                if(activePage.Title.Equals("Schedule Renovation"))
                    return ScheduleRenovationCommand;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private void MakeCommands()
        {
            AccommodationInputCommand = new RelayCommand(AccommodationInput_Executed, CanExecute);
            ScheduleRenovationCommand = new RelayCommand(ScheduleRenovation_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void AccommodationInput_Executed(object sender)
        {
            AccommodationInputDemo accommodationInputDemo = new AccommodationInputDemo();
            Thread thread = new Thread(accommodationInputDemo.PlayDemo);
            thread.Start();
        }

        private void ScheduleRenovation_Executed(object sender)
        {
            ScheduleRenovationDemo scheduleRenovationDemo = new ScheduleRenovationDemo();
            //Thread thread = new Thread(scheduleRenovationDemo.PlayDemo);
            //thread.Start();
            scheduleRenovationDemo.PlayDemo();
        }
    }
}
