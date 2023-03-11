using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Serializer;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AvailableTours.xaml
    /// </summary>
    public partial class AvailableTours : Window
    {
        public ObservableCollection<Tour> Tours
        {
            get { return tours; }
            set
            {
                if (value != tours)
                    tours = value;
                OnPropertyChanged("Tours");
            }
        }
        private ObservableCollection<TourInstance> TourInstances;
        private ObservableCollection<Tour> tours { get; set; }
        public Tour _tour;
        public TourInstance _tourInstance;
        private TourRepository _tourRepository;
        private TourInstanceRepository _tourInstanceRepository;
        public AvailableTours(TourInstance tourInstance,Tour tour)
        {
            InitializeComponent();
            DataContext = this;
            _tourInstance = tourInstance;
            _tour = tour;
            _tourRepository = new TourRepository();
            _tourInstanceRepository = new TourInstanceRepository();
            TourInstances = new ObservableCollection<TourInstance>(_tourInstanceRepository.GetAll());
            Tours = new ObservableCollection<Tour>();
            GetAllTours();
            SetLocations();
        }
        private ObservableCollection<Tour> GetAllTours()
        {
            foreach (TourInstance tourInstance in TourInstances)
            {
                if (tourInstance.Id != _tourInstance.Id && tourInstance.Tour.Id==_tourInstance.Tour.Id)
                {
                    Tours.Add(tourInstance.Tour);
                }
                
            }
            return Tours;
        }
        private ObservableCollection<Tour> GetAllToursInstances()
        {
            foreach (TourInstance tourInstance in TourInstances)
            {
                Tours.Add(tourInstance.Tour);
            }
            return Tours;
        }
        public void SetLocations()
        {
            Serializer<Location> _serializerLocation = new Serializer<Location>();
            List<Location> locations = _serializerLocation.FromCSV("../../../Resources/Data/locations.csv");
            foreach (Tour tour in Tours)
            {
                if (locations.Find(n => n.Id == tour.Location.Id) != null)
                {
                    tour.Location = locations.Find(n => n.Id == tour.Location.Id);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Reserve(object sender, RoutedEventArgs e)
        {
            /*Tour currentTour = (Tour)TourListDataGrid.CurrentItem;
            TourInstance currentTourInstance = new TourInstance();
            foreach(TourInstance tourInstance in TourInstances)
            {
                if(tourInstance.Tour.Id==currentTour.Id && tourInstance.Id == 5)
                {
                    currentTourInstance=tourInstance;
                }
            }
            TourReservationForm tourReservationForm = new TourReservationForm(currentTour, currentTourInstance);
            tourReservationForm.Show();*/
        }
    }
}
