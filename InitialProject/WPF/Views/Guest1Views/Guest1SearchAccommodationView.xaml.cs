using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
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
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.Service;


namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1Overview.xaml
    /// </summary>
    public partial class Guest1SearchAccommodationView : Page
    {
        private Guest1 guest1;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        private LocationService locationService;
        private AccommodationService accommodationService;
        private ObservableCollection<Accommodation> accommodations;
        public ObservableCollection<Accommodation> Accommodations
        {
            get { return accommodations; }
            set
            {
                if (value != accommodations)
                    accommodations = value;
                OnPropertyChanged("Accommodations");
            }

        }
        private Location location;
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
        
        public Guest1SearchAccommodationView(Guest1 guest1)
        {
            InitializeComponent();
            DataContext = this;
            this.guest1 = guest1;
            accommodationService = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetAll());
            SetAccommodationTypes();
            GetLocations();
            SetAccommodationLocations();
            SetAccommodationCoverImages();
        }
        private void SetAccommodationCoverImages()
        {
            AccommodationImageService accommodationImageService = new AccommodationImageService();
            foreach (Accommodation accommodation in Accommodations)
            {
                    accommodation.CoverImage = accommodationImageService.GetCoverImage(accommodation);
            }
        }
        private void GetLocations()
        {
            locationService = new LocationService();
            location = new Location();
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            cityInput.IsEnabled = false;
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
        }
        private void CountryInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (countryInput.SelectedItem != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationService.GetCitiesByCountry((string)countryInput.SelectedItem))
                {
                    CitiesByCountry.Add(city);
                }
                cityInput.IsEnabled = true;
            }
        }
        private void SetAccommodationLocations()
        {
            List<Location> locations = locationService.GetAll();
            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = locations.Find(n => n.Id == accommodation.Location.Id);
            }
        }
        private void SetAccommodationTypes()
        {
            AccommodationTypeService accommodationTypeService = new AccommodationTypeService();
            List<AccommodationType> types = accommodationTypeService.GetAll();
            foreach (Accommodation accommodation in Accommodations)
            {
                if (types.Find(n => n.Id == accommodation.Type.Id) != null)
                {
                    accommodation.Type = types.Find(n => n.Id == accommodation.Type.Id);
                }
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsNumberOfDaysValid() && IsNumberOfGuestsValid())
            {
                List<Accommodation> storedAccommodation = accommodationService.GetAll();
                FillWithStoredAccommodation(storedAccommodation);
                foreach (Accommodation accommodation in storedAccommodation)
                {
                    SearchByInputParameters(accommodation);
                }
            }
        }
        private void SearchByInputParameters(Accommodation accommodation)
        {
            SearchName(accommodation);
            SearchCity(accommodation);
            SearchCountry(accommodation);
            SearchType(accommodation);
            SearchNumberOfGuests(accommodation);
            SearchNumberOfDays(accommodation);
        }
        private void FillWithStoredAccommodation(List<Accommodation> storedAccommodation)
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in storedAccommodation)
            {
                Accommodations.Add(accommodation);
            }
        }
        private void SearchName(Accommodation accommodation)
        {
            if (!accommodation.Name.ToLower().Contains(nameInput.Text.ToLower()))
            {
                Accommodations.Remove(accommodation);
            }
        }
        private void SearchCity(Accommodation accommodation)
        {
            if (Location.City != null && !accommodation.Location.City.ToLower().Equals(Location.City.ToLower()))
            {
                Accommodations.Remove(accommodation);
            }
        }
        private void SearchCountry(Accommodation accommodation)
        {
            if (Location.Country != null && !accommodation.Location.Country.ToLower().Equals(Location.Country.ToLower()))
            {
                Accommodations.Remove(accommodation);
            }
        }
        private void SearchType(Accommodation accommodation)
        {
            if (apartment.IsChecked == true || house.IsChecked == true || cottage.IsChecked == true)
            {
                if (apartment.IsChecked == false)
                    RemoveIfApartment(accommodation);

                if (house.IsChecked == false)
                    RemoveIfHouse(accommodation);

                if (cottage.IsChecked == false)
                    RemoveIfCottage(accommodation);
            }
        }
        private void RemoveIfApartment(Accommodation accommodation)
        {
            if (accommodation.Type.Name.ToLower() == "apartment")
                Accommodations.Remove(accommodation);
        }
        private void RemoveIfHouse(Accommodation accommodation)
        {
            if (accommodation.Type.Name.ToLower() == "house")
                Accommodations.Remove(accommodation);
        }
        private void RemoveIfCottage(Accommodation accommodation)
        {
            if (accommodation.Type.Name.ToLower() == "cottage")
                Accommodations.Remove(accommodation);
        }
        private void SearchNumberOfGuests(Accommodation accommodation)
        {
            if (!(numberOfGuests.Text == "") && Convert.ToInt32(numberOfGuests.Text) > accommodation.Capacity)
            {
                Accommodations.Remove(accommodation);
            }
        }
        private void SearchNumberOfDays(Accommodation accommodation)
        {
            if (!(numberOfDays.Text == "") && Convert.ToInt32(numberOfDays.Text) < accommodation.MinDaysForReservation)
            {
                Accommodations.Remove(accommodation);
            }
        }
        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in accommodationService.GetAll())
                Accommodations.Add(accommodation);

            ResetAllSearchingFields();
        }
        private void ResetAllSearchingFields()
        {
            nameInput.Text = "";
            countryInput.SelectedItem = null;
            cityInput.SelectedItem = null;
            cityInput.IsEnabled = false;
            apartment.IsChecked = false;
            house.IsChecked = false;
            cottage.IsChecked = false;
            numberOfDays.Text = "";
            numberOfGuests.Text = "";
        }
        private void DecrementGuestsNumberButton_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if (numberOfGuests.Text != "" && Convert.ToInt32(numberOfGuests.Text) > 1)
            {
                changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) - 1;
                numberOfGuests.Text = changedGuestsNumber.ToString();
            }
        }
        private void IncrementGuestsNumberButton_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if (numberOfGuests.Text == "")
                numberOfGuests.Text = "1";
            else
            {
                changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) + 1;
                numberOfGuests.Text = changedGuestsNumber.ToString();
            }
        }
        private void DecrementDaysNumberButton_Click(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            if (numberOfDays.Text != "" && Convert.ToInt32(numberOfDays.Text) > 1)
            {
                changedDaysNumber = Convert.ToInt32(numberOfDays.Text) - 1;
                numberOfDays.Text = changedDaysNumber.ToString();
            }
        }
        private void IncrementDaysNumberButton_Click(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            if (numberOfDays.Text == "")
                numberOfDays.Text = "1";
            else
            {
                changedDaysNumber = Convert.ToInt32(numberOfDays.Text) + 1;
                numberOfDays.Text = changedDaysNumber.ToString();
            }
        }
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            Accommodation currentAccommodation = ((Button)sender).DataContext as Accommodation;
            AccommodationDetailsView details = new AccommodationDetailsView(currentAccommodation, guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = details;
        }
        
        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            Accommodation currentAccommodation = ((Button)sender).DataContext as Accommodation;
            AccommodationReservationFormView accommodationReservationForm = new AccommodationReservationFormView(currentAccommodation, guest1);
            accommodationReservationForm.Show();
        }
        private bool IsNumberOfDaysValid()
        {
            var content = numberOfDays.Text; 
            Match match = CreateValidationNumberRegex(content);
            bool isValid = false;
            if (!match.Success && numberOfDays.Text != "")
                ShowNumberOfDaysValidationMessages();
            else
            {
                isValid = true;
                numberOfDays.BorderBrush = Brushes.Green;
                numberOfDaysLabel.Content = string.Empty;
            }
            return isValid;
        }
        private void ShowNumberOfDaysValidationMessages()
        {
            numberOfDays.BorderBrush = Brushes.Red;
            numberOfDaysLabel.Content = "This field should be positive integer number";
            numberOfDays.BorderThickness = new Thickness(1);
        }
        private void ShowNumberOfGuestsValidationMessages()
        {
            numberOfGuests.BorderBrush = Brushes.Red;
            numberOfGuestsLabel.Content = "This field should be positive integer number";
            numberOfGuests.BorderThickness = new Thickness(1);
        }
        private bool IsNumberOfGuestsValid()
        {
            var content = numberOfGuests.Text;
            Match match = CreateValidationNumberRegex(content);
            bool isValid = false;
            if (!match.Success && numberOfGuests.Text != "")
                ShowNumberOfGuestsValidationMessages();
            else
            {
                isValid = true;
                numberOfGuests.BorderBrush = Brushes.Green;
                numberOfGuestsLabel.Content = string.Empty;
            }
            return isValid;
        }
        private Match CreateValidationNumberRegex(string content)
        {
            var regex = "^([1-9][0-9]*)$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            return match;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
