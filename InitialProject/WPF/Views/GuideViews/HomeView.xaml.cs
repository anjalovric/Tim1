using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;
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
        private ObservableCollection<TourInstance> instances;
        public ObservableCollection<TourInstance> FinishedInstances
        {
            get { return instances; }
            set
            {
                if (value != instances)
                    instances = value;
                OnPropertyChanged("Completed");
            }

        }
        private TourRepository tourRepository;
        private TourInstanceRepository tourInstanceRepository;
        private LocationRepository locationRepository;
        private User loggedInUser;
        
        public TourInstance Selected { get;set; }
            
        public HomeView(User user,ObservableCollection<TourInstance> Instances)
        {
            InitializeComponent();
            DataContext = this;
            tourRepository = new TourRepository();
            tourInstanceRepository = new TourInstanceRepository();
            locationRepository = new LocationRepository();
            FinishedInstances = Instances;
            StartedTourInstanceView startedTourInstanceView = new StartedTourInstanceView(Selected, Tours, Instances);
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

                StartedTourInstanceView startedTourInstanceView = new StartedTourInstanceView(Selected, Tours,FinishedInstances);
                Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = startedTourInstanceView;

        }



        private void History_Click(object sender, RoutedEventArgs e)
        {
            HistoryView historyView = new HistoryView();
            historyView.Show();
        }


    }
}
