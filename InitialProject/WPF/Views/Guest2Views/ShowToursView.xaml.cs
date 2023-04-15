using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for ShowTours.xaml
    /// </summary>
    public partial class ShowToursView : UserControl
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
        private Guest2 guest2;
        public ShowToursView(Guest2 guest2)
        {
            InitializeComponent();
            DataContext = this;
            tourRepository = new TourRepository();
            this.guest2 = guest2;
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
            ShowAlertGuestForm();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            cityInput.IsEnabled = false;
        }
        private void SetTourInstances(ObservableCollection<TourInstance> TourInstances)
        {
            List<TourInstance> tourInstances;
            tourInstances = tourInstanceRepository.GetAll();
            foreach (TourInstance tourInstance in tourInstances)
            {
                if (!tourInstance.Finished && !tourInstance.Canceled)
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
                    AlertGuestFormView alertGuestForm = new AlertGuestFormView(alert.Id);

                    if (alert.Guest2Id == guest2.Id && alert.Informed == false)
                    {
                        alertGuestForm.Show();
                    }

                }
            }

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
        private bool IsDurationValid()
        {
            Match matchForPositive = CreateValidationDurationRegexForPositiveNumber();
            Match matchForNegative= CreateValidationDurationRegexForNegativeNumber();
            if (durationInput.Text == "")
            {
                return true;
            }
            if (!matchForPositive.Success || Convert.ToDouble(matchForPositive.ToString()) == 0.0 || Convert.ToString(matchForPositive).Equals('0') || matchForNegative.Success)
            {
                ShowDurationValidationMessages();
                return false;
            }
            else if (matchForPositive.Success)
            {
                ShowSuccessMessage();
                return true;
            }
            return false;
        }
        private Match CreateValidationDurationRegexForPositiveNumber()
        {
            var regex = "(([1-9][0-9]*)(\\.[0-9]+)?)|(0\\.[0-9]+)$";
            Match match = Regex.Match(durationInput.Text, regex, RegexOptions.IgnoreCase);
            return match;
        }
        private Match CreateValidationDurationRegexForNegativeNumber()
        {
            var regex = "-";
            Match match = Regex.Match(durationInput.Text, regex, RegexOptions.IgnoreCase);
            return match;
        }
        private void ShowDurationValidationMessages()
        {
            durationInput.BorderBrush = Brushes.Red;
            DurationLabel.Content = "Invalid number";
            durationInput.BorderThickness = new Thickness(1);
        }
        private void ShowSuccessMessage()
        {
            durationInput.BorderBrush = Brushes.Green;
            DurationLabel.Content = string.Empty;
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
                SetTours(storedTourInstances);
                FillWithStoredTourInstances(storedTourInstances);
                foreach (TourInstance tourInstance in storedTourInstances)
                {
                    SearchByInputParameters(tourInstance);
                }
            }
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
                if(!tourInstance.Canceled && !tourInstance.Finished)
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
            foreach (TourInstance tourInstance in TourInstances)
            {
                if (tourInstance.Id == currentTourInstance.Id)
                {
                    currentTourInstance = tourInstance;
                }
            }
            TourReservationFormView tourReservationForm = new TourReservationFormView(currentTourInstance, guest2, TourInstances, tourInstanceRepository, Label);
            tourReservationForm.Show();
        }
        private void ViewDetails(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            try
            {
                List<string> imagesUrls = new List<string>();
                GetImagesUrls(imagesUrls, currentTourInstance);
                TourDetailsView detailsView = new TourDetailsView(imagesUrls, currentTourInstance);
                detailsView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
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
                if (!tourInstance.Finished && !tourInstance.Canceled)
                {
                    TourInstances.Add(tourInstance);
                    SetTours(TourInstances);
                }
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
        
    }
}
