using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for TourCheckPoints.xaml
    /// </summary>
    public partial class TourCheckPoints : Window, INotifyPropertyChanged
    {
        public ObservableCollection<CheckPoint> AllPoints { get; set; }
        public ObservableCollection<CheckPoint> CurrentPoint { get; set; }
        public ObservableCollection<TourInstance> Tours { get; set; }


        private CheckPointRepository pointsRepository;
        private TourRepository tourRepository;
        private TourInstanceRepository tourInstanceRepository;
        private TourReservationRepository tourReservationRepository;
        private AlertGuest2Repository alertGuest2Repository;
        private TourInstance selected;

        private int orderCounter = 1;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourCheckPoints(TourInstance selectedInstance,ObservableCollection<TourInstance>tours)
        {
            InitializeComponent();
            DataContext = this;
            pointsRepository = new CheckPointRepository();
            tourInstanceRepository= new TourInstanceRepository();
            tourReservationRepository = new TourReservationRepository();
            alertGuest2Repository = new AlertGuest2Repository();
            AllPoints = new ObservableCollection<CheckPoint>();
            CurrentPoint = new ObservableCollection<CheckPoint>();
            Tours = tours;
            selected = selectedInstance;

            FindPointsForSelectedInstance(selected);
            CheckFirstPoint();
            SetFirstCheckPoint();
           
        }

        private void SetFirstCheckPoint()
        {
            if (AllPoints.Count != 0)
            {
                CurrentPoint.Add(AllPoints.ToList().Find(n => n.Order == orderCounter));
                AddAlerts(CurrentPoint[0].Id, selected.Id);
            }
        }
        private void FindPointsForSelectedInstance(TourInstance selectedInstance)
        {
            List<CheckPoint> points = pointsRepository.GetAll();

            if (selectedInstance != null)
            {
                foreach (CheckPoint point in points)
                {
                    if (point.TourId == selectedInstance.Tour.Id)
                    {
                        AllPoints.Add(point);
                    }
                }
            }
        }

        private void CheckFirstPoint()
        {
            foreach (CheckPoint point in AllPoints)
            {
                if (point.Order == 1)
                    point.Checked = true;
            }
        }
        private void FinishTour_Click(object sender, RoutedEventArgs e)
        {
            FinishInstance();
            
        }

        private void FinishInstance()
        {
            selected.Finished = true;
            tourInstanceRepository.Update(selected);

            CheckAllPoints();
            CurrentPoint[0].Checked = true;

            Tours.Remove(selected);
            Finish.IsEnabled = false;
            FinishMessage.Content = "This tour is finished";
        }
        private void CheckAllPoints()
        {
            int pointSize=AllPoints.Count;
            List<CheckPoint> points = AllPoints.ToList();
            AllPoints.Clear();
            for (int i = 0; i < pointSize; i++)
            {
                points[i].Checked = true;
                AllPoints.Add(points[i]);

            }
        }

        private void ChangeCurrentPointToNextState()
        {
           List<CheckPoint> points = AllPoints.ToList();

            CurrentPoint.Remove(points.Find(n => n.Order == orderCounter));
            int nextOrder = orderCounter + 1;
            CurrentPoint.Add(points.Find(n => n.Order == nextOrder));
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            ChangeCurrentPointToNextState();
            
            orderCounter++;

            if (orderCounter == AllPoints.ToList().Count)
            {
                this.Next.IsEnabled = false;
                FinishInstance();
                
            }

           UpdateAllPointsListToNextPoint();

           AddAlerts(CurrentPoint[0].Id,selected.Id);
           
        }

        private void UpdateAllPointsListToNextPoint()
        {
            int pointSize=AllPoints.Count;
            List<CheckPoint> points= AllPoints.ToList();
            AllPoints.Clear();
            for(int i = 0; i < orderCounter; i++)
            {
                points[i].Checked= true;
                AllPoints.Add(points[i]);
            }
            for(int i = orderCounter; i < pointSize; i++)
            {
                points[i].Checked = false;
                AllPoints.Add(points[i]);
            }
        }


        private void AddAlerts(int currentPointId,int _callId)
        {
            List<TourReservation> availableReservations = GetReservationsForTour();

            foreach(TourReservation tour in availableReservations)
            {
                AlertGuest2 alertGuest2 = new AlertGuest2();
                alertGuest2.Availability = false;
                alertGuest2.ReservationId = tour.Id;
                alertGuest2.Guest2Id = tour.GuestId;
                alertGuest2.CheckPointId = currentPointId;
                alertGuest2.InstanceId = selected.Id;
                alertGuest2.Informed = false;
                AlertGuest2 savedAlert=alertGuest2Repository.Save(alertGuest2);
                

            }
        }
        private List<TourReservation> GetReservationsForTour()
        {
            List<TourReservation> tourReservations = tourReservationRepository.GetAll();
            List<TourReservation> availableReservations = new List<TourReservation>();
            foreach (TourReservation reservation in tourReservations)
            {
                if (reservation.TourInstanceId == selected.Id)
                {
                    availableReservations.Add(reservation);
                }
            }

            return availableReservations;
        }
    }
}
