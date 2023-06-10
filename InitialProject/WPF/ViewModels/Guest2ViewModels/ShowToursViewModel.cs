using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using InitialProject.Domain.Model;
using InitialProject.Service;
using InitialProject.Help;
using System.Windows.Input;
using InitialProject.APPLICATION.UseCases;
using System.Security.Cryptography.Xml;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class ShowToursViewModel:INotifyPropertyChanged
    {
        public RelayCommand CountryInputSelectionChangedCommand { get; set; }
        private ObservableCollection<TourInstance> tourInstances;
        public ObservableCollection<TourInstance> TourInstances
        {
            get { return tourInstances; }
            set
            {
                if (value != tourInstances)
                    tourInstances = value;
                OnPropertyChanged("TourInstances");
            }

        }
        public ObservableCollection<string> Languages { get; set; }
        private string language;
        public string SelectedLanguage
        {
            get => language;
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<TourImage> TourImages;
        private ObservableCollection<TourReservation> TourReservations;
        private TourRepository tourRepository;
        private TourInstanceRepository tourInstanceRepository;
        private TourImageRepository tourImageRepository;
        private TourReservationRepository tourReservationRepository;
        private AlertGuest2Repository alertGuest2Repository;
        private List<AlertGuest2> Alerts;
        private LocationRepository locationRepository;
        private Location location;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        private string city;
        public string City
        {
            get => city;
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged();
                }
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
        private string country;
        public string Country
        {
            get => country;
            set
            {
                if (value != country)
                {
                    country = value;
                    OnPropertyChanged();
                }
            }
        }
        public Location Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged();
                }
            }
        }
        private Guest2 guest2;
        private string maxGuests;

        public string MaxGuests
        {
            get => maxGuests;
            set
            {
                if (value != maxGuests)
                {
                    maxGuests = value;
                    OnPropertyChanged("MaxGuests");
                }
            }
        }
        private string duration;

        public string Duration
        {
            get => duration;
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }
        private TourInstance selected;
        public TourInstance Selected
        {
            get { return selected; }
            set
            {
                if (value != selected)
                    selected = value;
                OnPropertyChanged("Selected");
            }

        }
        private string label;
        public string Label
        {
            get => label;
            set
            {
                if (value != label)
                    label = value;
                OnPropertyChanged("Label");
            }
        }
        public RelayCommand ReserveCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand RestartCommand { get; set; }
        public RelayCommand IncrementCommand { get; set; }
        public RelayCommand DecrementCommand { get; set; }
        public RelayCommand ViewDetailsCommand { get; set; }
        public ICommand HelpCommandInViewModel { get;}
        private ShowToursView org;
        private GuideService guideService;
        public ShowToursViewModel(Guest2 guest2,ShowToursView org)
        {
            AddLanguages();
            Duration = "";
            MaxGuests = "";
            tourRepository = new TourRepository();
            this.org = org;
            this.guest2 = guest2;
            Label = "SHOWING ALL TOURS: ";
            MakeCommands();
            guideService = new GuideService();
            tourInstanceRepository = new TourInstanceRepository();
            tourReservationRepository = new TourReservationRepository();
            alertGuest2Repository = new AlertGuest2Repository();
            Alerts = new List<AlertGuest2>(alertGuest2Repository.GetAll());
            TourInstances = new ObservableCollection<TourInstance>();
            SetTourInstances(TourInstances);
            TourReservations = new ObservableCollection<TourReservation>(tourReservationRepository.GetAll());
            tourImageRepository = new TourImageRepository();
            TourImages = new ObservableCollection<TourImage>(tourImageRepository.GetAll());
            locationRepository = new LocationRepository();
            Location = new Location();
            SetTours(TourInstances);
            SetNotifications();
            //ShowAlertGuestForm();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            IsComboBoxCityEnabled = false;
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
            SortToursBySuperGuides();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void MakeCommands()
        {
            ReserveCommand = new RelayCommand(Reserve_Executed, CanExecute);
            SearchCommand = new RelayCommand(Search_Executed, CanExecute);
            RestartCommand = new RelayCommand(Restart_Executed, CanExecute);
            IncrementCommand=new RelayCommand(IncrementCapacityNumber_Executed,CanExecute);
            DecrementCommand=new RelayCommand(DecrementCapacityNumber_Executed, CanExecute);
            ViewDetailsCommand=new RelayCommand(ViewDetails_Executed,CanExecute);
            CountryInputSelectionChangedCommand = new RelayCommand(CountryInputSelectionChanged_Executed, CanExecute);  
        }
        private void SetTourInstances(ObservableCollection<TourInstance> TourInstances)
        {
            List<TourInstance> tourInstances;
            tourInstances = tourInstanceRepository.GetAll();
            foreach (TourInstance tourInstance in tourInstances)
            {
                if (!tourInstance.Finished && !tourInstance.Canceled)
                {
                    guideService.SetGuide(tourInstance);
                    TourInstances.Add(tourInstance);
                }
                    
            }
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
        private void ShowAlertGuestForm()
        {
            Alerts = alertGuest2Repository.GetAll();
            if (Alerts.Count() != 0)
            {
                foreach (AlertGuest2 alert in Alerts)
                {
                    AlertGuestFormView alertGuestForm = new AlertGuestFormView(alert.Id);

                    if (alert.Guest2Id == guest2.Id && alert.Informed == false)
                    {
                        alertGuestForm.Show();
                    }

                }
            }

        }
        private void SortToursBySuperGuides()
        {
            TourInstances = new ObservableCollection<TourInstance>(TourInstances.OrderByDescending(x => x.Guide.IsSuperGuide).ToList());
        }
       
        public void SetTours(ObservableCollection<TourInstance> TourInstances)
        {
            List<Tour> tours = tourRepository.GetAll();
            List<Location> locations = locationRepository.GetAll();
            foreach (TourInstance tourInstance in TourInstances)
            {
                foreach (Tour tour in tours)
                {
                    foreach (Location location in locations)
                    {
                        if (location.Id == tour.Location.Id && tour.Id == tourInstance.Tour.Id)
                        {
                            tour.Location = location;
                            tourInstance.Tour = tour;
                        }
                    }
                }
            }
        }
        private void SearchCity(TourInstance tourInstance)
        {
            if (City != null && !tourInstance.Tour.Location.City.ToLower().Equals(City.ToLower()))
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void SearchCountry(TourInstance tourInstance)
        {
            if (Country != null && !tourInstance.Tour.Location.Country.ToLower().Equals(Country.ToLower()))
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void SearchDuration(TourInstance tourInstance)
        {
            if (tourInstance.Tour.Duration != null && !(Duration == "") && tourInstance.Tour.Duration < Convert.ToDouble(Duration))
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void SearchLanguage(TourInstance tourInstance)
        {
            if (SelectedLanguage != null && !tourInstance.Tour.Language.Equals(SelectedLanguage))
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void SearchNumberOfGuest(TourInstance tourInstance)
        {
            if (tourInstance.Tour.MaxGuests != null && !(MaxGuests == "") && Convert.ToInt32(MaxGuests) > tourInstance.Tour.MaxGuests)
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void Search_Executed(object sender)
        {
            ObservableCollection<TourInstance> storedTourInstances = new ObservableCollection<TourInstance>(tourInstanceRepository.GetAll());
            SetTours(storedTourInstances);
            FillWithStoredTourInstances(storedTourInstances);
            foreach (TourInstance tourInstance in storedTourInstances)
            {
                SearchByInputParameters(tourInstance);
            }
            Label = "SHOWING SEARCHING TOURS: ";
        }
        private void SearchByInputParameters(TourInstance tourInstance)
        {
            SearchCity(tourInstance);
            SearchCountry(tourInstance);
            SearchDuration(tourInstance);
            SearchLanguage(tourInstance);
            SearchNumberOfGuest(tourInstance);
        }
        private void FillWithStoredTourInstances(ObservableCollection<TourInstance> storedTourInstances)
        {
            SetTours(storedTourInstances);
            TourInstances.Clear();
            foreach (TourInstance tourInstance in storedTourInstances)
            {
                if (!tourInstance.Canceled && !tourInstance.Finished)
                {
                    TourInstances.Add(tourInstance);
                    SetTours(TourInstances);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void IncrementCapacityNumber_Executed(object sender)
        {
            int changedDaysNumber;
            if (MaxGuests == "")
                MaxGuests = "1";
            else
            {
                changedDaysNumber = Convert.ToInt32(MaxGuests) + 1;
                MaxGuests = changedDaysNumber.ToString();
            }
        }

        private void DecrementCapacityNumber_Executed(object sender)
        {
            int changedDaysNumber;
            if (MaxGuests != "" && Convert.ToInt32(MaxGuests) > 1)
            {
                changedDaysNumber = Convert.ToInt32(MaxGuests) - 1;
                MaxGuests = changedDaysNumber.ToString();
            }
        }
        private void Reserve_Executed(object sender)
        {
            foreach (TourInstance tourInstance in TourInstances)
            {
                if (tourInstance.Id == Selected.Id)
                {
                    Selected = tourInstance;
                }
            }
            TourReservationFormView tourReservationForm = new TourReservationFormView(Selected, guest2, TourInstances, tourInstanceRepository, label);
            tourReservationForm.Show();
        }
        private void ViewDetails_Executed(object sender)
        {
            TourDetailsView details = new TourDetailsView(Selected, guest2);
            Keyboard.ClearFocus();
            details.Show();
            //details.Activate();
        }
        private void GetImagesUrls(List<string> imagesUrls, TourInstance currentTourInstance)
        {
            foreach (TourImage image in TourImages)
            {
                if (image.TourId == currentTourInstance.Tour.Id)
                {
                    imagesUrls.Add(image.Url);
                }
            }
        }
        private void Restart_Executed(object sender)
        {
            List<TourInstance> tourInstances = tourInstanceRepository.GetAll();
            TourInstances.Clear();
            foreach (TourInstance tourInstance in tourInstances)
            {
                if (!tourInstance.Finished && !tourInstance.Canceled)
                {
                    TourInstances.Add(tourInstance);
                    SetTours(TourInstances);
                }
            }
            Label = "SHOWING ALL TOURS: ";
            ResetAllFields();
        }
        private void ResetAllFields()
        {
            Country = null;
            City = null;
            IsComboBoxCityEnabled = false;
            Duration = "";
            SelectedLanguage = null;
            MaxGuests = "";
        }
        public void CountryInputSelectionChanged_Executed(object sender)
        {
            if (Country != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationRepository.GetCitiesByCountry((string)Country))
                {
                    CitiesByCountry.Add(city);
                }
                IsComboBoxCityEnabled = true;
            }
        }
        private Guest2 FindGuest2()
        {
            Guest2Service guest2Service = new Guest2Service();
            List<Guest2> Guests = new List<Guest2>(guest2Service.GetAll());
            if (Alerts.Count() != 0)
            {
                foreach (AlertGuest2 alert in Alerts)
                {
                    foreach(Guest2 guest in Guests)
                    {
                        if (alert.Guest2Id == guest2.Id)
                            return guest2;
                    }
                }
            }
            return null;
        }
        private void SetNotifications()
        {
           
            List<TourInstance> TourInstances = new List<TourInstance>();
            NewTourNotificationService newTourNotificationService = new NewTourNotificationService();
            TourInstances = tourInstanceRepository.GetAll();
            Alerts = alertGuest2Repository.GetAll();
            CheckPointService checkPointService = new CheckPointService();
            List<CheckPoint> checkPoints = new List<CheckPoint>(checkPointService.GetAll());
            if (Alerts.Count() != 0)
            {
                foreach (AlertGuest2 alert in Alerts)
                {
                    foreach(TourInstance TourInstance in TourInstances)
                    {
                        if (alert.Guest2Id == guest2.Id && alert.Informed == false && TourInstance.Id==alert.InstanceId)
                        {
                            NewTourNotification guest2Notification = new NewTourNotification(FindGuest2(), "You reserved this tour. Confirm your presence.", Guest2NotificationType.CONFIRM_PRESENCE,TourInstance, false,alert.Id);
                            newTourNotificationService.Save(guest2Notification);
                            alert.Informed = true;
                            alertGuest2Repository.Update(alert);
                        }
                    }

                }
            }

        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelp(str, org);
            }
        }
    }
}

