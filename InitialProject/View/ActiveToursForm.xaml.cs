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
        private AlertGuest2Service alertGuest2Service;
        private List<AlertGuest2> Alerts;
        private TourService tourService;
        private TourInstanceService tourInstanceService;
        private LocationService locationService;
        private CheckPointService checkPointService;
        private TourInstance tourInstance;

        public ActiveToursForm(Guest2 guest2)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Guest2 = guest2;
            AllPoints = new List<CheckPoint>();
            tourService = new TourService();
            tourInstanceService = new TourInstanceService();
            checkPointService = new CheckPointService();
            alertGuest2Service = new AlertGuest2Service();
            TourInstances = new ObservableCollection<TourInstance>();
            Alerts = new List<AlertGuest2>(alertGuest2Service.GetAll());
            CheckPoint = new ObservableCollection<CheckPoint>();
            tourInstances = new ObservableCollection<TourInstance>(tourInstanceService.GetAll());
            tourInstance = FindActiveTour(tourInstances);
            FindPointsForActiveInstance();
            SetTourInstances(TourInstances);
            locationService = new LocationService();
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
            ObservableCollection<TourInstance> tourInstances=new ObservableCollection<TourInstance>(tourInstanceService.GetAll());
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
            int orderCounter = 0;
            foreach (CheckPoint point in AllPoints)
            {
                if (point.Checked == false)
                {
                    orderCounter = point.Order - 1;
                    CheckPoint.Add(AllPoints[orderCounter - 1]);
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
            List<Location> locations = locationService.GetAll();
            List<Tour> tours = tourService.GetAll();

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
            List<Tour> tours = tourService.GetAll();
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
