using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class ActiveToursViewModel: INotifyPropertyChanged
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
                if (value != checkPoint)
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

        public ActiveToursViewModel(Guest2 guest2)
        {
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
            SetCheckPoint();
            SetTourInstances(TourInstances);
            locationService = new LocationService();
            SetTours(tourInstance);
        }
        private void SetTourInstances(ObservableCollection<TourInstance> TourInstances)
        {
            TourInstances.Clear();
            if (tourInstance != null && CheckPoint.Count()!=0)
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
            ObservableCollection<TourInstance> tourInstances = new ObservableCollection<TourInstance>(tourInstanceService.GetAll());
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
            foreach (TourInstance tourInstance in TourInstances)
            {
                foreach(AlertGuest2 alert in FindAllAlertGuestsByGuestId())
                {
                    if (tourInstance.Active == true && alert.Availability==true)
                    {
                        return tourInstance;
                    }
                }
            }
            return null;
        }
        public void SetTours(TourInstance tourInstance)
        {
            List<Tour> tours = tourService.GetAll();
            List<Location> locations = locationService.GetAll();
            if (tourInstance != null)
            {
                foreach (Location location in locations)
                {
                    foreach (Tour tour in tours)
                    {
                        if (location.Id == tour.Location.Id && tour.Id == tourInstance.Tour.Id)
                        {
                            tour.Location = location;
                            tourInstance.Tour = tour;
                        }
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
