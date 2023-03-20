using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private LocationRepository locationRepository;
        private Location location;
        private AccommodationImageRepository accommodationImageRepository;
        private ObservableCollection<Accommodation> accommodations;
        public List<AccommodationType> AccommodationTypes { get; set; }
        public ObservableCollection<AccommodationImage> Images { get; set; }
        public string Url { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }  
        public Model.Owner Owner { get; set; }
        

        public AccommodationForm(ObservableCollection<Accommodation> oldAccommodations, Model.Owner owner)
        {
            InitializeComponent();
            this.DataContext = this;
            accommodationRepository = new AccommodationRepository();
            accommodationImageRepository = new AccommodationImageRepository();
            accommodationTypeRepository = new AccommodationTypeRepository();
            accommodation = new Accommodation();
            accommodations = oldAccommodations;                                  //for owner overview list
            AccommodationTypes = accommodationTypeRepository.GetAll();
            locationRepository = new LocationRepository();
            location = new Location();
            Owner = owner;
            Images = new ObservableCollection<AccommodationImage>();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            ComboBoxCity.IsEnabled = false;
            Accommodation.MinDaysToCancel = 1;
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

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                MakeAndSaveAccommodation();
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields must be valid", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MakeAndSaveAccommodation()
        {
            accommodation.Id = accommodationRepository.NextId();
            accommodation.Location = locationRepository.GetByCityAndCountry(Location.Country, Location.City);
            accommodation.Owner = Owner;
            accommodations.Add(accommodation);
            accommodationRepository.Add(accommodation);
            SaveImages();
        }

        private void SaveImages()
        {
            foreach (AccommodationImage image in Images)
            {
                image.Accommodation = accommodation;
                image.Id = accommodationImageRepository.Add(image);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewImageButton_Click(object sender, RoutedEventArgs e)
        {
            MakeAndAddImages();
            TextBoxUrl.Clear();
        }

        private void MakeAndAddImages()
        {
            AccommodationImage image = new AccommodationImage();
            image.Url = Url;
            image.Id = -1;
            Images.Add(image);
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
            isValid &= IsNameValid();
            isValid &= IsCountryValid();
            isValid &= IsCityValid();
            isValid &= IsTypeValid();
            isValid &= IsCapacityValid();
            isValid &= IsMinDaysValid();
            isValid &= IsDaysToCancelValid();
            isValid &= IsImageNumberValid();

            return isValid;
        }

        private bool IsImageNumberValid()
        {
            if (Images.Count == 0)
            {
                ImageValidation.Content = "At least one picture is required";
                TextBoxUrl.BorderBrush = Brushes.Red;
                return false;
            }
            ImageValidation.Content = "";
            TextBoxUrl.BorderBrush = Brushes.Gray;
            return true;
        }

        private bool IsDaysToCancelValid()
        {
            bool isValid = true;
            var regex = @"^[0-9]\d*$";
            Match match = Regex.Match(DaysBeforeToCancelTextBox.Text, regex, RegexOptions.IgnoreCase);
            if (DaysBeforeToCancelTextBox.Text.Equals(""))
            {
                isValid = false;
                DaysBeforeToCancelValidation.Content = "This field is required";
                DaysBeforeToCancelTextBox.BorderBrush = Brushes.Red;
            }
            else if (!match.Success)
            {
                isValid = false;
                DaysBeforeToCancelValidation.Content = "Please enter valid number";
                DaysBeforeToCancelTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                DaysBeforeToCancelValidation.Content = "";
                DaysBeforeToCancelTextBox.BorderBrush = Brushes.Gray;
            }
            return isValid;
        }

        private bool IsMinDaysValid()
        {
            bool isValid = true;
            var regex = @"^[1-9]\d*$";
            Match match = Regex.Match(MinDaysForReservationTextBox.Text, regex, RegexOptions.IgnoreCase);
            if (MinDaysForReservationTextBox.Text.Equals(""))
            {
                isValid = false;
                MinDaysForReservationValidation.Content = "This field is required";
                MinDaysForReservationTextBox.BorderBrush = Brushes.Red;
            }
            else if (!match.Success)
            {
                isValid = false;
                MinDaysForReservationValidation.Content = "At least one day is required";
                MinDaysForReservationTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                MinDaysForReservationValidation.Content = "";
                MinDaysForReservationTextBox.BorderBrush = Brushes.Gray;
            }
            return isValid;
        }

        private bool IsCapacityValid()
        {
            bool isValid = true;
            var regex = @"^[1-9]\d*$";
            Match match = Regex.Match(CapacityTextBox.Text, regex, RegexOptions.IgnoreCase);
            if (CapacityTextBox.Text.Equals(""))
            {
                isValid = false;
                CapacityValidation.Content = "This field is required";
                CapacityTextBox.BorderBrush = Brushes.Red;
            }
            else if (!match.Success)
            {
                isValid = false;
                CapacityValidation.Content = "At least one guest is required";
                CapacityTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                CapacityValidation.Content = "";
                CapacityTextBox.BorderBrush = Brushes.Gray;
            }
            return isValid;
        }

        private bool IsTypeValid()
        {
            if (ComboBoxType.SelectedItem == null)
            {
                TypeValidation.Content = "This field is required";
                ComboBoxType.BorderBrush = Brushes.Red;
                return false;
            }
            TypeValidation.Content = "";
            ComboBoxType.BorderBrush = Brushes.Gray;
            return true;
        }

        private bool IsCityValid()
        {
            if (ComboBoxCity.SelectedItem == null)
            {
                CityValidation.Content = "This field is required";
                ComboBoxCity.BorderBrush = Brushes.Red;
                return false;
            }
            CityValidation.Content = "";
            ComboBoxCity.BorderBrush = Brushes.Gray;
            return true;
        }

        private bool IsCountryValid()
        {
            if (ComboBoxCountry.SelectedItem == null)
            {
                CountryValidation.Content = "This field is required";
                ComboBoxCountry.BorderBrush = Brushes.Red;
                return false;
            }
            CountryValidation.Content = "";
            ComboBoxCountry.BorderBrush = Brushes.Gray;
            return true;
        }

        private bool IsNameValid()
        {
            if (NameTextBox.Text.Equals(""))
            {
                NameValidation.Content = "This field is required";
                NameTextBox.BorderBrush = Brushes.Red;
                return false;
            }
            NameValidation.Content = "";
            NameTextBox.BorderBrush = Brushes.Gray;
            return true;
        }
    }
}
