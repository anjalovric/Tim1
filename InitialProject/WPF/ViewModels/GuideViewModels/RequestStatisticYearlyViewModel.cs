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
    public class RequestStatisticYearlyViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<GuideOneYearRequestStatisticViewModel> Statistics { get; set; }
        public ObservableCollection<GuideOneYearRequestStatisticViewModel> StatisticsLocation { get; set; }
        public ObservableCollection<TourInstance> todayInstances { get; set; }
        public ObservableCollection<TourInstance> futureInstances { get; set; }
        public ObservableCollection<string> Languages { get; set; }

        private User loggedUser;
        
        private string yearLanguage;
        public string YearLanguage
        {
            get { return yearLanguage; }
            set
            {
                if (value != yearLanguage)
                    yearLanguage = value;
                OnPropertyChanged();
            }
        }
        private string yearLocation;
        public string YearLocation
        {
            get { return yearLocation; }
            set
            {
                if (value != yearLocation)
                    yearLocation = value;
                OnPropertyChanged();
            }
        }
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
        public RelayCommand CreateTourByLanguageCommand { get; set; }
        public RelayCommand CreateTourByLocationCommand { get; set; }

        public RelayCommand EnableCityCommand { get; set; }
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
        private LocationService locationService;
        private TourRequestStatisticYearlyLanguageService tourRequestStatisticYearlyService;
        private TourRequestStatisticYearlyLocationService tourRequestStatisticYearlyLocationervice;
        private SuggestedLocationService suggestedLocationService;
        public RequestStatisticYearlyViewModel(ObservableCollection<TourInstance> todayinstances, User user, ObservableCollection<TourInstance> futureinstances) 
        {
            locationService=new LocationService();
            tourRequestStatisticYearlyService = new TourRequestStatisticYearlyLanguageService();
            tourRequestStatisticYearlyLocationervice=new TourRequestStatisticYearlyLocationService();
            suggestedLocationService=new SuggestedLocationService();            
            Statistics =new ObservableCollection<GuideOneYearRequestStatisticViewModel>();
            StatisticsLocation = new ObservableCollection<GuideOneYearRequestStatisticViewModel>();
            AddLanguages();
            MakeCommands();
            SelectedLanguage = null;
            MakeListOfLocations();
            loggedUser = user;
            todayInstances = todayinstances;
            futureInstances = futureinstances;
            SetDatas();
        }
        private void SetDatas()
        {
            Toast = "Hidden";
            ToastLocation = "Hidden";
            ToastCity = "Hidden";
            SuggestedLanguageService languageSuggestService = new SuggestedLanguageService();
            YearLanguage = languageSuggestService.GetMostWantedLanguage();
            SuggestedLocationService suggestedLocationService = new SuggestedLocationService();
            if (suggestedLocationService.GetMostWantedLocation() != null)
                YearLocation = suggestedLocationService.GetMostWantedLocation().Country + ", " + suggestedLocationService.GetMostWantedLocation().City;
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
            CreateTourByLanguageCommand = new RelayCommand(CreateTourByLanguage_Executed, CanExecute);
            CreateTourByLocationCommand = new RelayCommand(CreateTourByLocation_Executed,CanExecute);
            EnableCityCommand = new RelayCommand(EnableCityComboBox_Executed, CanExecute);
        }
        private void ResetLanguage_Executed(object sender)
        {
            SelectedLanguage = null;
            Statistics.Clear();
            Toast = "Hidden";
        }
        private void MonthlyStatisticLanguage_Executed(object sender)
        {
            RequestStatisticsMonthlyLanguageView requestStatisticsMonthly = new RequestStatisticsMonthlyLanguageView(SelectedLanguage,SelectedYear);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = requestStatisticsMonthly;
        }
        private void MonthlyStatisticLocation_Executed(object sender)
        {
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
            Toast = "Hidden";
            Statistics.Clear();
            if (SelectedLanguage != null)
                if (tourRequestStatisticYearlyService.GetLanguageYearStatistic(SelectedLanguage).Count > 0)
                    foreach (GuideOneYearRequestStatisticViewModel tourRequest in tourRequestStatisticYearlyService.GetLanguageYearStatistic(SelectedLanguage))
                        Statistics.Add(tourRequest);
                else
                    Toast = "Visible";
        }
        private void SearchLocation_Executed(object sender)
        {
            StatisticsLocation.Clear();
            if (Country != null && City != null)
            {
                Location searchedLocation = locationService.GetByCityAndCountry(Country, City);

                if (tourRequestStatisticYearlyLocationervice.GetLocationYearStatistic(searchedLocation).Count > 0)
                    foreach (GuideOneYearRequestStatisticViewModel tourRequest in tourRequestStatisticYearlyLocationervice.GetLocationYearStatistic(searchedLocation))
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
        public void EnableCityComboBox_Executed(object sender)
        {
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
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            IsComboBoxCityEnabled = false;
        }
        private void CreateTourByLanguage_Executed(object sender)
        {
            AddTourByLanguageView addTourByLanguageView = new AddTourByLanguageView(todayInstances, loggedUser, futureInstances, YearLanguage);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = addTourByLanguageView;
        }
        private void CreateTourByLocation_Executed(object sender)
        {
            AddTourByLocationView addTourByLocationView = new AddTourByLocationView(todayInstances, loggedUser, futureInstances, suggestedLocationService.GetMostWantedLocation());
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = addTourByLocationView;
        }
    }
}
