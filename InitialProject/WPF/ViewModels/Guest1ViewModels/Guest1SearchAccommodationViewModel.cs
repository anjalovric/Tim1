using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Interactivity;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1SearchAccommodationViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }

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
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (!value.Equals(name))
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        private string numberOfDays;
        public string NumberOfDays
        {
            get { return numberOfDays; }
            set
            {
                if (!value.Equals(numberOfDays))
                {
                    numberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }
        private string numberOfGuests;
        public string NumberOfGuests
        {
            get { return numberOfGuests; }
            set
            {
                if (!value.Equals(numberOfGuests))
                {
                    numberOfGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool apartmentChecked;
        public bool ApartmentChecked 
        {
            get { return apartmentChecked; }
            set
            {
                if (!value.Equals(apartmentChecked))
                {
                    apartmentChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool houseChecked;
        public bool HouseChecked 
        {
            get { return houseChecked; }
            set
            {
                if (!value.Equals(houseChecked))
                {
                    houseChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool cottageChecked;
        public bool CottageChecked 
        {
            get { return cottageChecked; }
            set
            {
                if (!value.Equals(cottageChecked))
                {
                    cottageChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isCityComboBoxEnabled;
        public bool IsCityComboBoxEnabled
        {
            get { return isCityComboBoxEnabled; }
            set
            {
                if (value != isCityComboBoxEnabled)
                {
                    isCityComboBoxEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private string locationCountry;
        public string LocationCountry
        {
            get { return locationCountry; }
            set
            {
                if (value != locationCountry)
                {
                    locationCountry = value;
                    OnPropertyChanged("LocationCountry");
                }
            }
        }
        private string locationCity;
        public string LocationCity
        {
            get { return locationCity; }
            set
            {
                if (value != locationCity)
                {
                    locationCity = value;
                    OnPropertyChanged("LocationCity");
                }
            }
        }


        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ShowAllCommand { get; set; }
        public RelayCommand ReserveCommand { get; set; }
        public RelayCommand ViewDetailsCommand { get; set; }
        public RelayCommand IncrementGuestsNumberCommand { get; set; }

        public RelayCommand IncrementDaysNumberCommand { get; set; }
        public RelayCommand DecrementGuestsNumberCommand { get; set; }
        public RelayCommand DecrementDaysNumberCommand { get; set; }
        public RelayCommand CountryInputSelectionChangedCommand { get; set; }
        public Guest1SearchAccommodationViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            accommodationService = new AccommodationService();
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetAll());
            SortAccommodationBySuperOwners();
            NumberOfDays = "";
            NumberOfGuests = "";
            Name = "";
            GetLocations();
            MakeCommands();
        }
        private void SortAccommodationBySuperOwners()
        {
            Accommodations = new ObservableCollection<Accommodation>(Accommodations.OrderByDescending(x => x.Owner.IsSuperOwner).ToList());
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void MakeCommands()
        {
            SearchCommand = new RelayCommand(Search_Executed, CanExecute);
            ShowAllCommand = new RelayCommand(ShowAll_Executed, CanExecute);
            ReserveCommand = new RelayCommand(Reserve_Executed, CanExecute);
            ViewDetailsCommand = new RelayCommand(ViewDetails_Executed, CanExecute);
            IncrementDaysNumberCommand = new RelayCommand(IncrementDaysNumber_Executed, CanExecute);
            IncrementGuestsNumberCommand = new RelayCommand(IncrementGuestsNumber_Executed, CanExecute);
            DecrementDaysNumberCommand = new RelayCommand(DecrementDaysNumber_Executed, CanExecute);
            DecrementGuestsNumberCommand = new RelayCommand(DecrementGuestsNumber_Executed, CanExecute);
            CountryInputSelectionChangedCommand = new RelayCommand(CountryInputSelectionChanged_Executed, CanExecute);
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
            IsCityComboBoxEnabled = false;
            LocationService locationService = new LocationService();
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
        }
        private void CountryInputSelectionChanged_Executed(object sender)
        {
            LocationService locationService = new LocationService();
            if (LocationCountry != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationService.GetCitiesByCountry(LocationCountry))
                {
                    CitiesByCountry.Add(city);
                }
                IsCityComboBoxEnabled = true;
            }
        }
        private void Search_Executed(object sender)
        {
            if (IsNumberOfDaysValid() && IsNumberOfGuestsValid())
            {
                accommodationService = new AccommodationService();
                List<Accommodation> storedAccommodation = accommodationService.GetAll();
                Accommodations.Clear();
                Accommodations = new ObservableCollection<Accommodation>(storedAccommodation);
                foreach (Accommodation accommodation in storedAccommodation)
                {
                    SearchByInputParameters(accommodation);
                }
            }
            SortAccommodationBySuperOwners();
        }
        private void SearchByInputParameters(Accommodation accommodation)
        {
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.SearchName(accommodation, Name));
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.SearchCity(accommodation, LocationCity));
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.SearchCountry(accommodation, LocationCountry));
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.SearchType(accommodation, ApartmentChecked, HouseChecked, CottageChecked));
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.SearchNumberOfGuests(accommodation, NumberOfGuests));
            Accommodations = new ObservableCollection<Accommodation>(accommodationService.SearchNumberOfDays(accommodation, NumberOfDays));
        }
        private void ShowAll_Executed(object sender)
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in accommodationService.GetAll())
                Accommodations.Add(accommodation);

            ResetAllSearchingFields();
            SortAccommodationBySuperOwners();
        }
        private void ResetAllSearchingFields()
        {
            Name = "";
            LocationCountry = null;
            LocationCity = null;
            IsCityComboBoxEnabled = false;
            ApartmentChecked = false;
            HouseChecked = false;
            CottageChecked = false;
            NumberOfDays = "";
            NumberOfGuests = "";
        }
        private void DecrementGuestsNumber_Executed(object sender)
        {
            int changedGuestsNumber;
            if (NumberOfGuests != "" && Convert.ToInt32(NumberOfGuests) > 1)
            {
                changedGuestsNumber = Convert.ToInt32(NumberOfGuests) - 1;
                NumberOfGuests = changedGuestsNumber.ToString();
            }
        }
        private void IncrementGuestsNumber_Executed(object sender)
        {
            int changedGuestsNumber;
            if (NumberOfGuests == "")
                NumberOfGuests = "1";
            else
            {
                changedGuestsNumber = Convert.ToInt32(NumberOfGuests) + 1;
                NumberOfGuests = changedGuestsNumber.ToString();
            }
        }
        private void DecrementDaysNumber_Executed(object sender)
        {
            int changedDaysNumber;
            if (NumberOfDays != "" && Convert.ToInt32(NumberOfDays) > 1)
            {
                changedDaysNumber = Convert.ToInt32(NumberOfDays) - 1;
                NumberOfDays = changedDaysNumber.ToString();
            }
        }
        private void IncrementDaysNumber_Executed(object sender)
        {
            int changedDaysNumber;
            if (NumberOfDays == "")
                NumberOfDays = "1";
            else
            {
                changedDaysNumber = Convert.ToInt32(NumberOfDays) + 1;
                NumberOfDays = changedDaysNumber.ToString();
            }
        }
        private void ViewDetails_Executed(object sender)
        {
            Accommodation currentAccommodation = ((Button)sender).DataContext as Accommodation;
            AccommodationDetailsView details = new AccommodationDetailsView(currentAccommodation, guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = details;
        }

        private void Reserve_Executed(object sender)
        {
            Accommodation currentAccommodation = ((Button)sender).DataContext as Accommodation;
            AccommodationReservationFormView accommodationReservationForm = new AccommodationReservationFormView(currentAccommodation, guest1);
            accommodationReservationForm.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            accommodationReservationForm.ShowDialog();
        }
        private bool IsNumberOfDaysValid()  //dodati normalnu validaciju
        {
            var content = NumberOfDays;
            Match match = CreateValidationNumberRegex(content);
            bool isValid = false;
            if (!match.Success && NumberOfDays != "")
                ShowNumberOfDaysValidationMessages();
            else
            {
                isValid = true;
                //numberOfDays.BorderBrush = Brushes.Green;
               // numberOfDaysLabel.Content = string.Empty;
            }
            return isValid;
        }
        private void ShowNumberOfDaysValidationMessages()
        {
            //numberOfDays.BorderBrush = Brushes.Red;
            //numberOfDaysLabel.Content = "This field should be positive integer number";
            //numberOfDays.BorderThickness = new Thickness(1);
        }
        private void ShowNumberOfGuestsValidationMessages()
        {
            //numberOfGuests.BorderBrush = Brushes.Red;
            //numberOfGuestsLabel.Content = "This field should be positive integer number";
            //numberOfGuests.BorderThickness = new Thickness(1);
        }
        private bool IsNumberOfGuestsValid()
        {
            var content = NumberOfGuests;
            Match match = CreateValidationNumberRegex(content);
            bool isValid = false;
            if (!match.Success && NumberOfGuests != "")
                ShowNumberOfGuestsValidationMessages();
            else
            {
                isValid = true;
                //numberOfGuests.BorderBrush = Brushes.Green;   //dodati validaciju1!!!!!!!
                //numberOfGuestsLabel.Content = string.Empty;
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
