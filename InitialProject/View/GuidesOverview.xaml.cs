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
    /// Interaction logic for GuidesOverview.xaml
    /// </summary>
    public partial class GuidesOverview : Window,INotifyPropertyChanged
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
                StartButton.IsEnabled= true;
                OnPropertyChanged();
            }
        }
        public GuidesOverview(User user)
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

            if (Selected ==null)
                StartButton.IsEnabled=false;
            
            
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
            List<TourInstance> tourInstances=tourInstanceRepository.GetAll();
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
        private void CreateNewTour_Click(object sender, RoutedEventArgs e)
        {
            TourForm tourForm = new TourForm(Tours,loggedInUser);
            tourForm.Show();

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

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            HistoryView historyView = new HistoryView();
            historyView.Show();
        }
    }
}
