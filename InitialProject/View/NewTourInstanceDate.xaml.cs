using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for NewTourInstanceDate.xaml
    /// </summary>
    public partial class NewTourInstanceDate : Window, INotifyPropertyChanged
    {
        private readonly TourInstanceRepository _tourInstanceRepository;
        private string _startHour;
        public string InstanceStartHour 
        {
            get => _startHour;
            set
            {
                if (value != _startHour)
                {
                    _startHour = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _start;
        private TourInstance newInstance;
        private ObservableCollection<TourInstance> tourInstances;

        public DateTime InstanceStartDate {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        private Tour _currentTour;
        public NewTourInstanceDate(Tour tour, ObservableCollection<TourInstance> instances)
        {
            InitializeComponent();
            DataContext = this;
            _currentTour = tour;
            newInstance = new TourInstance();
            tourInstances = instances;
            HourLabel.Content = "Time format should be hh:mm:ss";
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (IsDateValid() && IsTimeValid())
            {
                newInstance.StartDate = InstanceStartDate;
                newInstance.StartClock = InstanceStartHour;
                tourInstances.Add(newInstance);
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsDateValid()
        {
            if(InstanceStartDate.Date<DateTime.Now.Date)
            {
                Picker.BorderBrush = Brushes.Red;
                PickerLabel.Content = "Can't choose date from past";
                return false;
            }
            else
            {
                Picker.BorderBrush = Brushes.Green;
                PickerLabel.Content=string.Empty;
                return true;
            }
        }

        private bool IsTimeValid()
        {
            var content = InstanceStartHourTB.Text;
            var regex = "(([0-1][0-9])|(2[0-3]))\\:[0-5][0-9]\\:[0-5][0-9]$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (!match.Success)
            {
                HourLabel.Content = "Invalid format!Time format should be hh:mm:ss";
            }
            else if(match.Success && InstanceStartDate.Date>DateTime.Now.Date)
            {
                isValid = true;
            }
            else if (match.Success && InstanceStartDate.Date==DateTime.Now.Date)
            {
                string times = match.ToString();
                
                int hour = Convert.ToInt32(times.Split(':')[0]);
                int minute = Convert.ToInt32(times.Split(':')[1]);
                int second = Convert.ToInt32(times.Split(':')[2]);

                if (hour < Convert.ToInt32(DateTime.Now.Hour.ToString()))
                {
                    HourLabel.Content = "Can't choose time for past";
                    InstanceStartHourTB.BorderBrush = Brushes.Red;
                    isValid = false;
                }
                else if(hour> Convert.ToInt32(DateTime.Now.Hour.ToString()))
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Green;
                    HourLabel.Content=string.Empty;
                    isValid = true;
                }
                else if (hour == Convert.ToInt32(DateTime.Now.Hour.ToString()) && minute < Convert.ToInt32(DateTime.Now.Minute.ToString()))
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Red;
                    HourLabel.Content = "Can't choose time for past";
                }
                else if (hour == Convert.ToInt32(DateTime.Now.Hour.ToString()) && minute > Convert.ToInt32(DateTime.Now.Minute.ToString()))
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Green;
                    HourLabel.Content =string.Empty;
                    isValid =true;
                }
                else if (hour == Convert.ToInt32(DateTime.Now.Hour.ToString()) && minute == Convert.ToInt32(DateTime.Now.Minute.ToString()) && second < Convert.ToInt32(DateTime.Now.Second.ToString()))
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Red;
                    HourLabel.Content = "Can't choose time for past";
                }
                else if (hour == Convert.ToInt32(DateTime.Now.Hour.ToString()) && minute == Convert.ToInt32(DateTime.Now.Minute.ToString()) && second > Convert.ToInt32(DateTime.Now.Second.ToString()))
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Green;
                    HourLabel.Content = string.Empty;
                    isValid = true;
                }
            }
            return isValid;
        }
    }
}
