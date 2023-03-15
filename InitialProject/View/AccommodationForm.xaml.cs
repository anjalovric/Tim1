using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
using InitialProject.Repository;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationForm.xaml
    /// </summary>
    public partial class AccommodationForm : Window, INotifyPropertyChanged
    {
        private AccommodationRepository accommodationRepository;
        private Accommodation accommodation;
        private AccommodationTypeRepository accommodationTypeRepository;
        public List<AccommodationType> accommodationTypes { get; set; }
        private LocationRepository locationRepository;
        private Location location;
        public ObservableCollection<AccommodationImage> Images { get; set; }
        public string Url { get; set; }
        private AccommodationImageRepository accommodationImageRepository;
        private ObservableCollection<Accommodation> accommodations;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }  
        public Model.Owner Owner { get; set; }
        

        public AccommodationForm(ObservableCollection<Accommodation> oldAccommodations, Model.Owner owner)
        {
            InitializeComponent();
            this.DataContext = this;
            accommodationRepository = new AccommodationRepository();
            accommodation = new Accommodation();
            accommodations = oldAccommodations;
            accommodationTypeRepository = new AccommodationTypeRepository();
            accommodationTypes = accommodationTypeRepository.GetAll();
            locationRepository = new LocationRepository();
            location = new Location();
            Owner = owner;
            Images = new ObservableCollection<AccommodationImage>();
            accommodationImageRepository = new AccommodationImageRepository();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            ComboBoxCity.IsEnabled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public Accommodation Accommodation
        {
            get { return accommodation; }
            set
            {
                if (value != accommodation)
                {
                    accommodation = value;
                    OnPropertyChanged();
                }
            }
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                accommodation.Id = accommodationRepository.NextId();
                accommodation.Location = locationRepository.GetLocation(Location.Country, Location.City);
                accommodation.Owner = Owner;
                accommodations.Add(accommodation);
                accommodationRepository.Add(accommodation);
                AddImages();
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must be valid", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddImages()
        {
            foreach (AccommodationImage image in Images)
            {
                image.Accommodation = accommodation;
                image.Id = accommodationImageRepository.Add(image.Url, image.Accommodation);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewImage_Click(object sender, RoutedEventArgs e)
        {
            AccommodationImage image = new AccommodationImage();
            image.Url = Url;
            image.Id = -1;
            Images.Add(image);
            TextBoxUrl.Clear();
        }

        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBoxCountry.SelectedItem != null)
            {
                CitiesByCountry.Clear();
                foreach(string city in locationRepository.GetCitiesByCountry((string)ComboBoxCountry.SelectedItem))
                {
                    CitiesByCountry.Add(city);
                }
                ComboBoxCity.IsEnabled = true;
            }
        }

        private bool IsValid()
        {
            bool isValid = true;
            if (NameTextBox.Text.Equals(""))
            {
                isValid = false;
                NameValidation.Content = "This field is required";
                NameTextBox.BorderBrush = Brushes.Red;
            }
            if(ComboBoxCountry.SelectedItem == null)
            {
                isValid = false;
                CountryValidation.Content = "This field is required";
                ComboBoxCountry.BorderBrush = Brushes.Red;
            }
            if (ComboBoxCity.SelectedItem == null)
            {
                isValid = false;
                CityValidation.Content = "This field is required";
                ComboBoxCity.BorderBrush = Brushes.Red;
            }
            if (ComboBoxType.SelectedItem == null)
            {
                isValid = false;
                TypeValidation.Content = "This field is required";
                ComboBoxType.BorderBrush = Brushes.Red;
            }
            if(CapacityTextBox.Text.Equals(""))
            {
                isValid = false;
                CapacityValidation.Content = "This field is required";
                CapacityTextBox.BorderBrush = Brushes.Red;
            }
            else if(Convert.ToInt32(CapacityTextBox.Text) <= 0)
            {
                isValid = false;
                CapacityValidation.Content = "At least one guest is required";
                CapacityTextBox.BorderBrush = Brushes.Red;
            }
            if (MinDaysForReservationTextBox.Text.Equals(""))
            {
                isValid = false;
                MinDaysForReservationValidation.Content = "This field is required";
                MinDaysForReservationTextBox.BorderBrush = Brushes.Red;
            }
            else if (Convert.ToInt32(MinDaysForReservationTextBox.Text) <= 0)
            {
                isValid = false;
                MinDaysForReservationValidation.Content = "At least one day is required";
                MinDaysForReservationTextBox.BorderBrush = Brushes.Red;
            }
            if (DaysBeforeToCancelTextBox.Text.Equals(""))
            {
                isValid = false;
                DaysBeforeToCancelValidation.Content = "This field is required";
                DaysBeforeToCancelTextBox.BorderBrush = Brushes.Red;
            }
            else if (Convert.ToInt32(DaysBeforeToCancelTextBox.Text) < 0)
            {
                isValid = false;
                DaysBeforeToCancelValidation.Content = "Please enter valid number";
                DaysBeforeToCancelTextBox.BorderBrush = Brushes.Red;
            }
            if (Images.Count == 0)
            {
                isValid = false;
                ImageValidation.Content = "At least one picture is required";
                TextBoxUrl.BorderBrush = Brushes.Red;
            }

            return isValid;
        }
    }
}
