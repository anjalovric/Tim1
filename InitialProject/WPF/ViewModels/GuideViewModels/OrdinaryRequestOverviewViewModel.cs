using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.WPF.ViewModels.GuideViewModels
{
    public class OrdinaryRequestOverviewViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<OrdinaryTourRequests> Requests { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<TourInstance> Tours { get; set; }
        public ObservableCollection<TourInstance> Future { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        public ObservableCollection <string> Languages { get; set; }

        private List <OrdinaryTourRequests> appropriateRequests;

        private SearchRequestsService searchRequestsService;
        public OrdinaryTourRequests Selected { get; set; }
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        private RequestNotificationService requestNotificationService;
        private LocationService locationService;

        private string country;
        public string Country
        {
            get { return country; }
            set
            {
                if (value != country)
                    country = value;
                 OnPropertyChanged();
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                    description = value;
                OnPropertyChanged();
            }
        }
        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if (value != city)
                    city = value;
                OnPropertyChanged();
            }
        }
        private string language;
        public string Language
        {
            get { return language; }
            set
            {
                if (value != language)
                    language = value;
                OnPropertyChanged();
            }
        }
        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                    capacity = value;
                OnPropertyChanged();
            }
        }
        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set
            {
                if (value != start)
                    start = value;
                OnPropertyChanged();
            }
        }
        private DateTime stardate;
        public DateTime Stardate
        {
            get { return stardate; }
            set
            {
                if (value != stardate)
                    stardate = value;
                OnPropertyChanged();
            }
        }
        private DateTime end;
        public DateTime End
        {
            get { return end; }
            set
            {
                if (value != end)
                    end = value;
                OnPropertyChanged();
            }
        }
        private bool isComboBoxCityEnabled;
        public bool IsComboBoxCityEnabled
        {
            get { return isComboBoxCityEnabled; }
            set
            {
                if (value != isComboBoxCityEnabled)
                    isComboBoxCityEnabled = value;
                OnPropertyChanged();
            }
        }
        private User loggedUser;
        private bool isEntered = false;
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand ViewDescriptionCommand { get; set; }
        public RelayCommand EnableCityCommand { get; set; } 

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public OrdinaryRequestOverviewViewModel(User user,ObservableCollection<TourInstance> tours,ObservableCollection<TourInstance> futures)
        {
            Capacity = 0;
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            AddLanguages();
            MakeRequestsList();
            loggedUser = user;
            appropriateRequests = new List<OrdinaryTourRequests>();
            searchRequestsService= new SearchRequestsService();
            MakeCommands();
            Tours = tours;
            Future= futures;
            requestNotificationService = new RequestNotificationService();
            locationService = new LocationService();
            MakeListOfLocations();
            requestNotificationService.UpCount();
            SetNews();
            Description = "";
            Stardate= DateTime.Now;
        }
        private void SetNews()
        {
            foreach (OrdinaryTourRequests request in Requests)
            {
                foreach (OrdinaryRequestNotification requestNotification in requestNotificationService.GetAll())
                    if (requestNotification.Count == 1 && request.Id == requestNotification.RequestId)
                        request.IsNew = true;
            }
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
        private void MakeRequestsList()
        {

            Requests= new ObservableCollection<OrdinaryTourRequests>(ordinaryTourRequestsService.GetOnWaitingRequests());
        }
        private void MakeCommands()
        {
            SearchCommand = new RelayCommand(Search_Executed, CanExecute);
            ResetCommand= new RelayCommand(Restart, CanExecute);
            CreateCommand = new RelayCommand(CreateTour_Executed, CanExecute);
            ViewDescriptionCommand=new RelayCommand(ViewDescription_Executed, CanExecute);
            EnableCityCommand = new RelayCommand(EnableCityComboBox_Executed, CanExecute);
        }
        private void MakeListOfLocations()
        {
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            IsComboBoxCityEnabled = false;
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
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Search_Country()
        {
            if (Country != null && !Country.Equals(""))
            { 
                appropriateRequests = searchRequestsService.GetRequestsByCountry(appropriateRequests, Country, isEntered);
                isEntered = true;
            }
        }
        private void Search_City()
        {
            if (City != null && !City.Equals(""))
            {
                appropriateRequests = searchRequestsService.GetRequestsByCity(appropriateRequests, City, isEntered);
                isEntered = true;
            }
        }
        private void Search_Language()
        {
            if (Language != null && !Language.Equals(""))
            {
                appropriateRequests = searchRequestsService.GetRequestsByLanguage(appropriateRequests, Language, isEntered);
                isEntered = true;
            }
        }
        private void Search_Capacity()
        {
            if (Capacity != null && Capacity != 0 )
            {
                appropriateRequests = searchRequestsService.GetRequestsByCapacity(appropriateRequests, Capacity, isEntered);
                isEntered = true;
            }

        }
        private void RefreshResquests()
        {
            Requests.Clear();
            foreach(OrdinaryTourRequests ordinaryTourRequests in appropriateRequests)
                Requests.Add(ordinaryTourRequests); 
        }
        private void Search_StartDate()
        {
            if (Start != null && !Start.ToString().Equals("1/1/0001 12:00:00 AM"))
            {
                appropriateRequests = searchRequestsService.GetRequestsByStart(appropriateRequests, Start, isEntered);
                isEntered = true;
            }
        }
        private void Search_EndDate()
        {
            if (End != null && !End.ToString().Equals("1/1/0001 12:00:00 AM"))
            {
                appropriateRequests = searchRequestsService.GetRequestsByEnd(appropriateRequests, End, isEntered);
                isEntered = true;
            }
        }
        private void Search_Executed(object sender)
        {
           isEntered = false;
            appropriateRequests.Clear();
            Search_Country();
            Search_City();
            Search_Capacity();
            Search_StartDate();
            Search_EndDate();
            Search_Language();
            if (Country == null && City == null && Language == null && Capacity == 0 && Start == Convert.ToDateTime("1/1/0001 12:00:00 AM") && End == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                appropriateRequests = Requests.ToList();
            RefreshResquests();
        }
        private void Restart(object sender)
        {
            Country = null;
            City = null;
            Capacity =0;
            Language = null;
            String date = "1/1/0001 12:00:00 AM";
            Start = Convert.ToDateTime(date);
            End = Convert.ToDateTime(date);
            Requests.Clear();
            foreach(OrdinaryTourRequests request in ordinaryTourRequestsService.GetOnWaitingRequests())
                Requests.Add(request);
        }
        private void CreateTour_Executed(object sender)
        {
            CreateTourFromRequestView createTourFromRequestView = new CreateTourFromRequestView(Tours,loggedUser,Future,Selected, Requests);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = createTourFromRequestView;
        }
        private void ViewDescription_Executed(object sender)
        {
            Description = Selected.Description;
        }
    }
}
