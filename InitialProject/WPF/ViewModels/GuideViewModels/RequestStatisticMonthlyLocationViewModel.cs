using InitialProject.Model;
using InitialProject.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.WPF.ViewModels.GuideViewModels
{
    public class RequestStatisticMonthlyLocationViewModel:INotifyPropertyChanged
    {
        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if (value != location)
                    location = value;
                OnPropertyChanged();
            }
        }
        private string year;
        public string Year
        {
            get { return year; }
            set
            {
                if (value != year)
                    year = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MonthRequestStatisticViewModel> Statistics { get; set; }
        public RequestStatisticMonthlyLocationViewModel(Location location, GuideOneYearRequestStatisticViewModel selectedYear)
        {
            Location = location.Country+", "+location.City;
            Year = selectedYear.Year.ToString();
            TourRequestStatisticMonthlyLocationService monthLocationStatisticService = new TourRequestStatisticMonthlyLocationService();
            Statistics = new ObservableCollection<MonthRequestStatisticViewModel>(monthLocationStatisticService.GetMonthStatistic(location, selectedYear.Year));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
        
}
