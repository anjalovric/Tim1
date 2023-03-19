using InitialProject.Model;
using InitialProject.Repository;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : Window, INotifyPropertyChanged
    {
        public ObservableCollection<TourInstance> Instances { get; set; }
        private TourInstanceRepository tourInstanceRepository;
        private LocationRepository locationRepository;
        private TourRepository tourRepository;

        private TourInstance selected;
        public TourInstance Selected
        {
            get { return selected; }
            set
            {
                if (value != selected)
                    selected = value;
                
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public HistoryView()
        {
            InitializeComponent();
            DataContext = this;
            tourInstanceRepository = new TourInstanceRepository();
            locationRepository = new LocationRepository();
            tourRepository = new TourRepository();
            Instances = new ObservableCollection<TourInstance>();


            SetLocationToTour();
            SetTourToTourInstance();

            GetFinishedInsatnces();


        }
        private void GetFinishedInsatnces()
        {
            List<TourInstance> instances = tourInstanceRepository.GetAll();
            foreach (TourInstance instance in instances)
            {
                if (instance.Finished)
                {
                    Instances.Add(instance);
                }
            }
        }
        private void SetLocationToTour()
        {
            List<Location> locations = locationRepository.GetAll();
            List<Tour> tours =tourRepository.GetAll();

            foreach(Tour tour in tours)
            {
                foreach (Location location in locations)
                {
                    if(location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }

        private void SetTourToTourInstance()
        {
            List<TourInstance> instances = tourInstanceRepository.GetAll();
            List<Tour> tours = tourRepository.GetAll();

            foreach (TourInstance instance in instances)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Id == instance.Tour.Id)
                        instance.Tour = tour;
                }
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (Selected == null)
            {
                MessageBox.Show("Select tour insatnce first");
            }
            else
            {
                CheckPointDetails checkPointDetails = new CheckPointDetails(Selected);
                checkPointDetails.Show();
            }
        }

    }
}
