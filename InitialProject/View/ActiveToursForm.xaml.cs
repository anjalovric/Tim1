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
        private List<CheckPoint> AllPoints;
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
        private CheckPointService checkPointService;
        private int orderCounter = 0;
        private TourInstance tourInstance;
        private CheckPoint CurrentPoint;

        public ActiveToursForm(Guest2 guest2)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Guest2 = guest2;
            CurrentPoint = new CheckPoint();
            AllPoints = new List<CheckPoint>();
            tourRepository = new TourRepository();
            tourInstanceRepository = new TourInstanceRepository();
            checkPointService = new CheckPointService();
            alertGuest2Repository = new AlertGuest2Repository();
            TourInstances = new ObservableCollection<TourInstance>();
            Alerts = new List<AlertGuest2>(alertGuest2Repository.GetAll());
            CheckPoint = new ObservableCollection<CheckPoint>();
            tourInstances = new ObservableCollection<TourInstance>(tourInstanceRepository.GetAll());
            tourInstance = FindActiveTour(tourInstances);
            FindPointsForActiveInstance();
            SetTourInstances(TourInstances);
            locationRepository = new LocationRepository();
            location = new Location();
            SetLocations();
            SetTours(tourInstance);
            SetCheckPoint();
        }
        private void SetTourInstances(ObservableCollection<TourInstance> TourInstances)
        {
            TourInstances.Clear();
            if (tourInstance!=null)
                TourInstances.Add(tourInstance);
        }
        private void SetCheckPoint()
        {
            foreach (AlertGuest2 alertGuest2 in FindAllAlertGuestsByGuestId())
            {
                foreach (CheckPoint checkPoint in AllPoints)
                {
                    if (alertGuest2.Availability == true && alertGuest2.CheckPointId == checkPoint.Id)
                    {
                        FindLastCheckedPoint();
                        return;
                    }
                }
            }
        }
        private void FindPointsForActiveInstance()
        {
            ObservableCollection<TourInstance> tourInstances=new ObservableCollection<TourInstance>(tourInstanceRepository.GetAll());
            TourInstance tourInstance = FindActiveTour(tourInstances);
            List<CheckPoint> points = checkPointService.GetAll();
            if (tourInstance != null)
            {
                foreach (CheckPoint point in points)
                {
                    if (point.TourId == tourInstance.Tour.Id)
                    {
                        AllPoints.Add(point);
                    }
                }
            }
        }
        private void FindLastCheckedPoint()
        {
            foreach (CheckPoint point in AllPoints)
            {
                if (point.Checked == false)
                {
                    orderCounter = point.Order - 1;
                    CurrentPoint=AllPoints[orderCounter - 1];
                    CheckPoint.Add(CurrentPoint);
                    break;
                }
            }
        }
        private List<AlertGuest2> FindAllAlertGuestsByGuestId()
        {
            List<AlertGuest2> AlertGuest2;
            AlertGuest2 = new List<AlertGuest2>();
            foreach (AlertGuest2 alertGuest2 in Alerts)
            {
                if (alertGuest2.Guest2Id == Guest2.Id)
                {
                    AlertGuest2.Add(alertGuest2);
                }
            }
            return AlertGuest2;
        }
        private TourInstance FindActiveTour(ObservableCollection<TourInstance> TourInstances)
        {
            foreach(TourInstance tourInstance in TourInstances)
            {
                if (tourInstance.Active == true)
                {
                    return tourInstance;
                }
            }
            return null;
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
        public void SetTours(TourInstance tourInstance)
        {
            List<Tour> tours = tourRepository.GetAll();
            if (tourInstance != null)
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
