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
        
        private readonly GuideRepository guideRepository;
        private Guide currentGuide;
        private TourInstance newInstance;
        private ObservableCollection<TourInstance> tourInstances;


        private string startTime;
        public string InstanceStartHour 
        {
            get => startTime;
            set
            {
                if (value != startTime)
                {
                    startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime startDate;
        public DateTime InstanceStartDate {
            get => startDate;
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public NewTourInstanceDate( ObservableCollection<TourInstance> instances,User user)
        {
            InitializeComponent();
            DataContext = this;
            newInstance = new TourInstance();
            tourInstances = instances;
            guideRepository = new GuideRepository();
            
            currentGuide = guideRepository.GetByUsername(user.Username);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (IsDateValid() && IsTimeValid())
            {
                newInstance.StartDate = InstanceStartDate;
                newInstance.StartClock = InstanceStartHour;
                newInstance.Guide = currentGuide;
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

                if (hour < DateTime.Now.Hour)
                {
                    HourLabel.Content = "Can't choose time for past";
                    InstanceStartHourTB.BorderBrush = Brushes.Red;
                }
                else if(hour> DateTime.Now.Hour)
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Green;
                    HourLabel.Content=string.Empty;
                    isValid = true;
                }
                else if (hour == DateTime.Now.Hour && minute < DateTime.Now.Minute)
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Red;
                    HourLabel.Content = "Can't choose time for past";
                }
                else if (hour == DateTime.Now.Hour && minute > DateTime.Now.Minute)
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Green;
                    HourLabel.Content =string.Empty;
                    isValid =true;
                }
                else if (hour == DateTime.Now.Hour && minute == DateTime.Now.Minute && second < DateTime.Now.Second)
                {
                    InstanceStartHourTB.BorderBrush = Brushes.Red;
                    HourLabel.Content = "Can't choose time for past";
                }
                else if (hour == DateTime.Now.Hour && minute == DateTime.Now.Minute && second > DateTime.Now.Second)
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
