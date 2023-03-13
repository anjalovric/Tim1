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
        public Accommodation accommodation { get; set; }
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
        

        public AccommodationForm(ObservableCollection<Accommodation> oldAccommodations)
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
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            accommodation.Id = accommodationRepository.NextId();
            accommodation.Location = locationRepository.GetLocation(Location.Country, Location.City);
            accommodations.Add(accommodation); 
            accommodationRepository.Add(accommodation);
            AddImages();
            
            this.Close();
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
    }
}
