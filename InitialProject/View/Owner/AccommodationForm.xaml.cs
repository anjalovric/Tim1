using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class AccommodationForm : Window
    {
        private AccommodationRepository accommodationRepository;
        public Accommodation accommodation { get; set; }
        private AccommodationTypeRepository accommodationTypeRepository;
        private List<AccommodationType> accommodationTypes;
        private LocationRepository locationRepository;
        public Location location { get; set; }

        public AccommodationForm()
        {
            InitializeComponent();
            this.DataContext = this;
            accommodationRepository = new AccommodationRepository();
            accommodation = new Accommodation();
            accommodationTypeRepository = new AccommodationTypeRepository();
            accommodationTypes = accommodationTypeRepository.GetAll();
            locationRepository = new LocationRepository();
            location = new Location();

        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            AccommodationType accommodationType = new AccommodationType();
            accommodationType.Id = -1;
            accommodation.Type = accommodationType;
            locationRepository.Add(location);
            accommodation.Location = location;
            accommodationRepository.Add(accommodation);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
