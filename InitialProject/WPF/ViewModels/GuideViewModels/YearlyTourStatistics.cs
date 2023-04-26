using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.WPF.ViewModels.GuideViewModels
{
    public class YearlyTourStatistics:INotifyPropertyChanged
    {
        public ObservableCollection<GuideOneYearRequestStatisticViewModel> Statistics { get; set; }
        public ObservableCollection<GuideOneYearRequestStatisticViewModel> StatisticsLocation { get; set; }
        public ObservableCollection<string> Languages { get; set; }

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                if (value != selectedLanguage)
                    selectedLanguage = value;
                Toast = "Hidden";
                OnPropertyChanged();
            }
        }
        private GuideOneYearRequestStatisticViewModel selectedYear;
        public GuideOneYearRequestStatisticViewModel SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (value != selectedYear)
                    selectedYear = value;
                Toast = "Hidden";
                OnPropertyChanged();
            }

        }
        private bool isComboBoxCityEnabled;
        public bool IsComboBoxCityEnabled
        {
            get => isComboBoxCityEnabled;
            set
            {
                if (value != isComboBoxCityEnabled)
                {
                    isComboBoxCityEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private string city;
        public string City
        {
            get => city;
            set
            {
                if (value != city)
                {
                    city = value;
                    ToastLocation = "Hidden";
                        ToastCity = "Hidden";
;                    OnPropertyChanged();
                }
            }
        }
        private string country;
        public string Country
        {
            get => country;
            set
            {
                if (value != country)
                {
                    country = value;
                    ToastLocation = "Hidden";
                    City = null;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }

        public RelayCommand SearchLanguageCommand { get; set; }
        public RelayCommand ResetLanguageCommand { get; set; }
        public RelayCommand SearchLocationCommand { get; set; }
        public RelayCommand ResetLocationCommand { get; set; }
        public RelayCommand MonthlyStatisticCommand { get;set; }
        public RelayCommand MonthlyStatisticLocationCommand { get; set; }


        private string toastLocation;
        public string ToastLocation
        {
            get { return toastLocation; }
            set
            {
                if (value != toastLocation)
                    toastLocation = value;
                OnPropertyChanged();
            }

        }
        private string toast;
        public string Toast
        {
            get { return toast; }
            set
            {
                if (value != toast)
                    toast = value;
                OnPropertyChanged();
            }
        }
        private string toastCity;
        public string ToastCity
        {
            get { return toastCity; }
            set
            {
                if (value != toastCity)
                    toastCity = value;
                OnPropertyChanged();
            }
        }
        public YearlyTourStatistics() 
        {
            Statistics=new ObservableCollection<GuideOneYearRequestStatisticViewModel>();
            StatisticsLocation = new ObservableCollection<GuideOneYearRequestStatisticViewModel>();
            AddLanguages();
            MakeCommands();
            SelectedLanguage = null;
            Toast = "Hidden";
            ToastLocation = "Hidden";
            ToastCity = "Hidden";
            MakeListOfLocations();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MakeCommands()
        {
            SearchLanguageCommand = new RelayCommand(SearchLanguage_Executed, CanExecute);
            ResetLanguageCommand = new RelayCommand(ResetLanguage_Executed, CanExecute);
            SearchLocationCommand= new RelayCommand(SearchLocation_Executed, CanExecute);
            ResetLocationCommand=new RelayCommand(ResetLocation_Executed,CanExecute);
            MonthlyStatisticCommand = new RelayCommand(MonthlyStatisticLanguage_Executed, CanExecute);
            MonthlyStatisticLocationCommand = new RelayCommand(MonthlyStatisticLocation_Executed, CanExecute);
        }
        private void ResetLanguage_Executed(object sender)
        {
            SelectedLanguage = null;
            Statistics.Clear();
            Toast = "Hidden";
        }
        private void MonthlyStatisticLanguage_Executed(object sender)
        {
            RequestStatisticsMonthly requestStatisticsMonthly = new RequestStatisticsMonthly(SelectedLanguage,SelectedYear);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = requestStatisticsMonthly;
        }
        private void MonthlyStatisticLocation_Executed(object sender)
        {
            LocationService locationService = new LocationService();
            Location location = locationService.GetByCityAndCountry(Country, City);
            RequestStatisticMonthlyLocatiovView requestStatisticsMonthly = new RequestStatisticMonthlyLocatiovView(location, SelectedYear);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = requestStatisticsMonthly;
        }
        private void ResetLocation_Executed(object sender)
        {
            Country = null;
            City = null;
            StatisticsLocation.Clear();
            ToastLocation = "Hidden";
            IsComboBoxCityEnabled = false;
            ToastCity = "Hidden";
        }
        private void SearchLanguage_Executed(object sender)
        {
            TourRequestStatisticYearlyService tourRequestStatisticYearlyService = new TourRequestStatisticYearlyService();
            Toast = "Hidden";
            Statistics.Clear();
            if (SelectedLanguage != null)
                if (tourRequestStatisticYearlyService.GetYearStatistic(SelectedLanguage).Count > 0)
                    foreach (GuideOneYearRequestStatisticViewModel tourRequest in tourRequestStatisticYearlyService.GetYearStatistic(SelectedLanguage))
                        Statistics.Add(tourRequest);
                else
                    Toast = "Visible";
        }
        private void SearchLocation_Executed(object sender)
        {
            TourRequestStatisticYearlyLocationService tourRequestStatisticYearlyService = new TourRequestStatisticYearlyLocationService();
            LocationService locationService = new LocationService();
            StatisticsLocation.Clear();
            if (Country != null && City != null)
            {
                Location searchedLocation = locationService.GetByCityAndCountry(Country, City);

                if (tourRequestStatisticYearlyService.GetYearStatistic(searchedLocation).Count > 0)
                    foreach (GuideOneYearRequestStatisticViewModel tourRequest in tourRequestStatisticYearlyService.GetYearStatistic(searchedLocation))
                        StatisticsLocation.Add(tourRequest);
                else
                    ToastLocation = "Visible";

            }
            else if (Country != null && City == null)
                ToastCity = "Visible";
        }
        private void AddLanguages()
        {
            Languages = new ObservableCollection<string>();
            Languages.Add("english");
            Languages.Add("spanish");
            Languages.Add("russian");
            Languages.Add("arabic");
            Languages.Add("serbian");
            Languages.Add("italian");
        }
        public void ComboBoxCountry_SelectionChanged()
        {
            LocationService locationService = new LocationService();
            if (Country != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationService.GetCitiesByCountry((string)Country))
                {
                    CitiesByCountry.Add(city);
                }
                IsComboBoxCityEnabled = true;
            }
        }
        private void MakeListOfLocations()
        {
            LocationService locationService = new LocationService();
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            IsComboBoxCityEnabled = false;
        }
    }
}
