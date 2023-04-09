using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ActiveToursForm.xaml
    /// </summary>
    public partial class ActiveToursForm : UserControl
    {
        private Model.Guest2 Guest2;
        private ObservableCollection<TourInstance> tourInstances;
        public ObservableCollection<TourInstance> TourInstances
        {
            get { return tourInstances; }
            set
            {
                if (value != tourInstances)
                    tourInstances = value;
                OnPropertyChanged("TourInstances");
            }

        }
        private ObservableCollection<CheckPoint> checkPoint;
        public ObservableCollection<CheckPoint> CheckPoint
        {
            get { return checkPoint; }
            set
            {
                if(value != checkPoint)
                    checkPoint = value;
                OnPropertyChanged("CheckPoint");
            }
        }
        private AlertGuest2Repository alertGuest2Repository;
        private List<AlertGuest2> Alerts;
        private TourRepository tourRepository;
        private TourInstanceRepository tourInstanceRepository;
        private LocationRepository locationRepository;
        private Location location;
        private TourReservationRepository tourReservationRepository;
        private ObservableCollection<CheckPoint> CheckPoints;
        private CheckPointService checkPointService;

        public ActiveToursForm(Guest2 guest2)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Guest2 = guest2;
            tourRepository = new TourRepository();
            tourInstanceRepository = new TourInstanceRepository();
            tourReservationRepository = new TourReservationRepository();
            checkPointService = new CheckPointService();
            CheckPoints = new ObservableCollection<CheckPoint>(checkPointService.GetAll());
            alertGuest2Repository = new AlertGuest2Repository();
            TourInstances = new ObservableCollection<TourInstance>();
            Alerts = new List<AlertGuest2>(alertGuest2Repository.GetAll());
            CheckPoint = new ObservableCollection<CheckPoint>();
            SetTourInstances(TourInstances);
            locationRepository = new LocationRepository();
            location = new Location();
            
            SetLocations();
            SetTours(TourInstances);
        }
        private void SetTourInstances(ObservableCollection<TourInstance> TourInstances)
        {
            List<TourInstance> tourInstances;
            tourInstances = tourInstanceRepository.GetAll();
            foreach (TourInstance tourInstance in tourInstances)
            {
                foreach(AlertGuest2 alertGuest2 in Alerts)
                {
                    foreach(CheckPoint checkPoint in CheckPoints)
                    {
                        if (alertGuest2.Guest2Id == Guest2.Id && alertGuest2.Availability == true && tourInstance.Active == true && alertGuest2.CheckPointId==checkPoint.Id &&checkPoint.Checked==true)
                        {
                            TourInstances.Add(tourInstance);
                            CheckPoint.Add(checkPoint);
                            break;
                        }
                    }
                }
            }
        }
        public void SetLocations()
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
        public void SetTours(ObservableCollection<TourInstance> TourInstances)
        {
            List<Tour> tours = tourRepository.GetAll();
            foreach (TourInstance tourInstance in TourInstances)
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

    }
}
