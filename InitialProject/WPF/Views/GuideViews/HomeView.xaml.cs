using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {
        public ObservableCollection<TourInstance> Tours { get; set; }
        private TourRepository tourRepository;
        private TourInstanceRepository tourInstanceRepository;
        private LocationRepository locationRepository;
        private User loggedInUser;
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
        public HomeView(User user)
        {
            InitializeComponent();
            DataContext = this;
            tourRepository = new TourRepository();
            tourInstanceRepository = new TourInstanceRepository();
            locationRepository = new LocationRepository();

            Tours = new ObservableCollection<TourInstance>(tourInstanceRepository.GetByStart());
            SetLocationToTour();
            SetTourToTourInstance();
            loggedInUser = user;

            
        }
        private void SetLocationToTour()
        {
            List<Location> locations = locationRepository.GetAll();
            List<Tour> tours = tourRepository.GetAll();
            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }

        private void SetTourToTourInstance()
        {
            List<TourInstance> tourInstances = tourInstanceRepository.GetAll();
            List<Tour> tours = tourRepository.GetAll();
            foreach (TourInstance tourInstance in tourInstances)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Id == tourInstance.Tour.Id)
                    {
                        tourInstance.Tour = tour;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartTour_Click(object sender, RoutedEventArgs e)
        {
            if (Selected != null)
            {
                TourCheckPoints checkPoints = new TourCheckPoints(Selected, Tours);
                checkPoints.Show();
            }

        }



        private void History_Click(object sender, RoutedEventArgs e)
        {
            HistoryView historyView = new HistoryView();
            historyView.Show();
        }


    }
}
