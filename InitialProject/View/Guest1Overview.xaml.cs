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
using InitialProject.Repository;
using InitialProject.Serializer;
using InitialProject.View.Owner;


namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest1Overview.xaml
    /// </summary>
    public partial class Guest1Overview : Window
    {
        private AccommodationImageRepository accommodationImageRepository;
        private List<AccommodationImage> accommodationImages;


        private AccommodationRepository accommodationRepository;
        
        private ObservableCollection<Accommodation> accommodations;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        private LocationRepository locationRepository;
        private Location location;
        private AccommodationType accommodationType;
        private AccommodationTypeRepository accommodationTypeRepository;
        public ObservableCollection<Accommodation> Accommodations 
        { 
            get { return accommodations; } 
            set
            {
                if(value != accommodations)
                    accommodations=value;
                OnPropertyChanged("Accommodations");
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

        public Guest1Overview()
        {
            InitializeComponent();
            DataContext = this;
            accommodationRepository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetAll());
            
            accommodationImageRepository = new AccommodationImageRepository();
            accommodationImages = new List<AccommodationImage>(accommodationImageRepository.GetAll());

            locationRepository = new LocationRepository();
            location = new Location();
            accommodationType = new AccommodationType();
            accommodationTypeRepository = new AccommodationTypeRepository();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            cityInput.IsEnabled = false;

            SetLocationsForAccommodationView(Accommodations);
            SetTypeForAccommodationView(Accommodations);
            
        }

        

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void searchNameButton(object sender, RoutedEventArgs e)
        {
            
            List<Accommodation> listAccommodation = accommodationRepository.GetAll();
            Accommodations.Clear();
            
                foreach(Accommodation accommodation in listAccommodation)
                {
                    Accommodations.Add(accommodation);
                }
           // SetLocations(listAccommodation);
            //SetType(listAccommodation);


            

            foreach (Accommodation accommodation in listAccommodation)
            {
                if(!accommodation.Name.ToLower().Contains(nameInput.Text.ToLower()))
                {
                    Accommodations.Remove(accommodation);
                }
                if (Location.City != null && !accommodation.Location.City.ToLower().Equals(Location.City.ToLower()))
                {
                    Accommodations.Remove(accommodation);
                }
                if (Location.Country != null && !accommodation.Location.Country.ToLower().Equals(Location.Country.ToLower()))
                {
                    Accommodations.Remove(accommodation);
                }
                if(apartment.IsChecked==true || house.IsChecked==true || cottage.IsChecked==true)
                {
                    if (apartment.IsChecked==false)
                    {
                        if(accommodation.Type.Name.ToLower() == "apartment")
                            Accommodations.Remove(accommodation);
                    }
                    if (house.IsChecked==false)
                    {
                        if (accommodation.Type.Name.ToLower() == "house")
                            Accommodations.Remove(accommodation);
                    }
                    if (cottage.IsChecked==false)
                    {
                        if (accommodation.Type.Name.ToLower() == "cottage")
                            Accommodations.Remove(accommodation);
                    }
                }
                for(int i = 0; i < numberOfGuests.Text.Length; i++)
                {
                    if(numberOfGuests.Text[i]<'0' || numberOfGuests.Text[i]>'9')
                    {
                        MessageBox.Show("Field 'Number of guests' must be numeric and bigger than 0.");
                        numberOfGuests.Text = "";
                        return;
                    }
                }
                for (int i = 0; i < numberOfDays.Text.Length; i++)
                {
                    if (numberOfDays.Text[i] < '0' || numberOfDays.Text[i] > '9')
                    {
                        MessageBox.Show("Field 'Number of days' must be numeric and bigger than 0.");
                        numberOfDays.Text = "";
                        return;
                    }
                }

                if (!(numberOfGuests.Text=="") && Convert.ToInt32(numberOfGuests.Text) > accommodation.Capacity)
                {
                    Accommodations.Remove(accommodation);
                }
                if (!(numberOfDays.Text == "") && Convert.ToInt32(numberOfDays.Text) < accommodation.MinDaysForReservation)
                {
                    Accommodations.Remove(accommodation);
                }




            }
            

        }
        private void SetLocations(List<Accommodation> accommodations)
        {
            List<Location> locations = locationRepository.GetAll();
            foreach(Accommodation accommodation in accommodations)
            {
                accommodation.Location = locations.Find(n => n.Id == accommodation.Location.Id);
            }
        }

        private void SetLocationsForAccommodationView(ObservableCollection<Accommodation> Accommodations)
        {
            List<Location> locations = locationRepository.GetAll();
            foreach (Accommodation accommodation in Accommodations)
            {
                accommodation.Location = locations.Find(n => n.Id == accommodation.Location.Id);
            }
        }

        public void SetType(List<Accommodation> accommodations)
        {
            List<AccommodationType> types = accommodationTypeRepository.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                if (types.Find(n => n.Id == accommodation.Type.Id) != null)
                {
                    accommodation.Type = types.Find(n => n.Id == accommodation.Type.Id);
                }
            }
        }

        public void SetTypeForAccommodationView(ObservableCollection<Accommodation> Accommodations)
        {
            List<AccommodationType> types = accommodationTypeRepository.GetAll();
            foreach (Accommodation accommodation in Accommodations)
            {
                if (types.Find(n => n.Id == accommodation.Type.Id) != null)
                {
                    accommodation.Type = types.Find(n => n.Id == accommodation.Type.Id);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DecrementGuestsNumber(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if(numberOfGuests.Text!="" && Convert.ToInt32(numberOfGuests.Text)>1)
            {
                changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) - 1;
                numberOfGuests.Text = changedGuestsNumber.ToString();
            }
            
        }

        private void IncrementGuestsNumber(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if(numberOfGuests.Text=="")
            {
                numberOfGuests.Text = "1";
            }
            else
            {
                changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) + 1;
                numberOfGuests.Text = changedGuestsNumber.ToString();
            }
            
            
        }

        private void DecrementDaysNumber(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            if (numberOfDays.Text != "" && Convert.ToInt32(numberOfDays.Text) > 1)
            {
                changedDaysNumber = Convert.ToInt32(numberOfDays.Text) - 1;
                numberOfDays.Text = changedDaysNumber.ToString();
            }
        }

        private void IncrementDaysNumber(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            if(numberOfDays.Text=="")
            {
                numberOfDays.Text = "1";
            }
            else
            {
                changedDaysNumber = Convert.ToInt32(numberOfDays.Text) + 1;
                numberOfDays.Text = changedDaysNumber.ToString();
            }
            
        }

        private void ViewPhotos(object sender, RoutedEventArgs e)
        {
            try
            {
                Accommodation currentAccommodation = (Accommodation)AccommodationListDataGrid.CurrentItem;


                List<string> imagesUrl = new List<string>();

                foreach (AccommodationImage image in accommodationImages)
                {
                    if (image.Accommodation.Id == currentAccommodation.Id)
                    {
                        imagesUrl.Add(image.Url);
                    }
                }

                if (imagesUrl.Count == 0)
                {
                    MessageBox.Show("There are currently no images for the selected accommodation.");
                }
                else
                {
                    AccommodationPhotosView photosView = new AccommodationPhotosView(imagesUrl);
                    photosView.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Reserve(object sender, RoutedEventArgs e)
        {
            Accommodation currentAccommodation = (Accommodation)AccommodationListDataGrid.CurrentItem;
            AccommodationReservationForm accommodationReservationForm = new AccommodationReservationForm(currentAccommodation, ref accommodationRepository);
            accommodationReservationForm.Show();
        }


        private void countryInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            Accommodations.Clear();
            foreach (Accommodation accommodation in accommodationRepository.GetAll())
                Accommodations.Add(accommodation);
        }
    }
}
