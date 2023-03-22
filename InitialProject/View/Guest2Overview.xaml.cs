using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
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
    /// Interaction logic for Guest2Overview.xaml
    /// </summary>
    public partial class Guest2Overview : Window
    {
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
        public Guest2Overview()
        {
            InitializeComponent();
            DataContext = this;
            tourRepository = new TourRepository();
            tourInstanceRepository = new TourInstanceRepository();
            tourReservationRepository = new TourReservationRepository();
            alertGuest2Repository = new AlertGuest2Repository();
            TourInstances = new ObservableCollection<TourInstance>();
            SetTourInstances(TourInstances);
            TourReservations = new ObservableCollection<TourReservation>(tourReservationRepository.GetAll());
            tourImageRepository = new TourImageRepository();
            TourImages=new ObservableCollection<TourImage>(tourImageRepository.GetAll());
            locationRepository = new LocationRepository();
            Location = new Location();
            SetLocations();
            SetTours(TourInstances);
            ShowAlertGuestForm();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            cityInput.IsEnabled = false;
        }
        private void SetTourInstances(ObservableCollection<TourInstance> TourInstances)
        {
            List<TourInstance> tourInstances;
            tourInstances = tourInstanceRepository.GetAll();
            foreach(TourInstance tourInstance in tourInstances)
            {
                if (tourInstance.Finished == false)
                    TourInstances.Add(tourInstance);
            }
        }
        private void ShowAlertGuestForm()
        {
            Alerts = alertGuest2Repository.GetAll();
            if (Alerts.Count() != 0)
            {
                foreach (AlertGuest2 alert in Alerts)
                {
                    AlertGuestForm alertGuestForm = new AlertGuestForm(alert.Id);
                    if (3 == alert.Guest2Id && alert.Informed == false)
                        alertGuestForm.Show();
                }
            }
        }
        public void SetLocations()
        {
            List<Location> locations = locationRepository.GetAll();
            List<Tour> tours = tourRepository.GetAll();

            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }
        public void SetTours(ObservableCollection<TourInstance> TourInstances)
        {
            List<Tour> tours = tourRepository.GetAll();
            foreach (TourInstance tourInstance in TourInstances)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Id == tourInstance.Tour.Id)
                    {
                        tourInstance.Tour = tour;
                    }
                }
            }
        }
        private bool IsDurationValid()
        {
            var content = durationInput.Text;
            var regex = "(([1-9][0-9]*)(\\.[0-9]+)?)|(0\\.[0-9]+)$";
            var regexMinus = "-";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            Match matchMinus = Regex.Match(content, regexMinus, RegexOptions.IgnoreCase);
            if (!match.Success || Convert.ToDouble(match.ToString()) == 0.0 || Convert.ToString(match).Equals('0') || matchMinus.Success)
            {
                durationInput.BorderBrush = Brushes.Red;
                DurationLabel.Content = "Invalid number";
                durationInput.BorderThickness = new Thickness(1);
                return false;
            }
            else if (match.Success)
            {
                durationInput.BorderBrush = Brushes.Green;
                DurationLabel.Content = string.Empty;
                return true;
            }
            if (durationInput.Text == "")
            {
                return true;
            }
            return false;
        }
        private void SearchCity(TourInstance tourInstance)
        {
            if (Location.City != null && !tourInstance.Tour.Location.City.ToLower().Equals(Location.City.ToLower()))
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void SearchCountry(TourInstance tourInstance)
        {
            if (Location.Country != null && !tourInstance.Tour.Location.Country.ToLower().Equals(Location.Country.ToLower()))
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void SearchDuration(TourInstance tourInstance)
        {
            if (tourInstance.Tour.Duration != null && durationInput.Text != "" && tourInstance.Tour.Duration < Convert.ToDouble(durationInput.Text))
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void SearchLanguage(TourInstance tourInstance)
        {
            if (tourInstance.Tour.Language != null && !tourInstance.Tour.Language.ToLower().Contains(languageInput.Text.ToLower()))
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void SearchNumberOfGuest(TourInstance tourInstance)
        {
            if (tourInstance.Tour.MaxGuests != null && Convert.ToInt32(capacityNumber.Text) > tourInstance.Tour.MaxGuests)
            {
                TourInstances.Remove(tourInstance);
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (IsDurationValid())
            {
                ObservableCollection<TourInstance> storedTourInstances = new ObservableCollection<TourInstance>(tourInstanceRepository.GetAll()); 
                SetLocations();
                SetTours(storedTourInstances);
                TourInstances.Clear();
                foreach (TourInstance tourInstance in storedTourInstances)
                {
                    if(!tourInstance.Finished)
                        TourInstances.Add(tourInstance);
                }
                foreach (TourInstance tourInstance in storedTourInstances)
                {
                    SearchCity(tourInstance);
                    SearchCountry(tourInstance);
                    SearchDuration(tourInstance);
                    SearchLanguage(tourInstance);
                    SearchNumberOfGuest(tourInstance);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void IncrementCapacityNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedCapacityNumber;
            changedCapacityNumber = Convert.ToInt32(capacityNumber.Text) + 1;
            capacityNumber.Text = changedCapacityNumber.ToString();
        }

        private void DecrementCapacityNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedCapacityNumber;
            if (Convert.ToInt32(capacityNumber.Text) > 1)
            {
                changedCapacityNumber = Convert.ToInt32(capacityNumber.Text) - 1;
                capacityNumber.Text = changedCapacityNumber.ToString();
            }
        }
        private void Reserve(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            foreach(TourInstance tourInstance in TourInstances)
            {
                if (tourInstance.Id==currentTourInstance.Id)
                {
                    currentTourInstance = tourInstance;
                }
            }
            TourReservationForm tourReservationForm = new TourReservationForm(currentTourInstance,3,TourInstances,tourInstanceRepository,Label);
            tourReservationForm.Show();
        }
        private void ViewDetails(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            try
            {
                List<string> imagesUrls = new List<string>();
                GetImagesUrls(imagesUrls,currentTourInstance);
                TourDetails detailsView = new TourDetails(imagesUrls,currentTourInstance);
                detailsView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void GetImagesUrls(List<string> imagesUrls,TourInstance currentTourInstance)
        {
            foreach (TourImage image in TourImages)
            {
                if (image.TourId == currentTourInstance.Tour.Id)
                {
                    imagesUrls.Add(image.Url);
                }
            }
        }

        private void CountryInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (countryInput.SelectedItem != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationRepository.GetCitiesByCountry((string)countryInput.SelectedItem))
                {
                    CitiesByCountry.Add(city);
                }
                cityInput.IsEnabled = true;
            }
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            List<TourInstance> tourInstances = tourInstanceRepository.GetAll();
            TourInstances.Clear();
            foreach (TourInstance tourInstance in tourInstances)
            {
                if(!tourInstance.Finished)
                    TourInstances.Add(tourInstance);
            }
            Label.Content = "Showing all tours:";
            ResetAllFields();
        }
        private void ResetAllFields()
        {
            countryInput.SelectedValue = null;
            cityInput.SelectedValue = null;
            cityInput.IsEnabled = false;
            durationInput.Text = "";
            languageInput.Text = "";
            capacityNumber.Text = "1";
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }
    }
}
