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
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ActiveInstanceView.xaml
    /// </summary>
    public partial class ActiveInstanceView : Page,INotifyPropertyChanged
    {
        public ObservableCollection<CheckPoint> AllPoints { get; set; }
        public ObservableCollection<CheckPoint> CurrentPoint { get; set; }
        private CheckPointRepository pointsRepository;
        private int orderCounter = 0;
        private TourInstanceRepository tourInstanceRepository;
        private TourReservationRepository tourReservationRepository;
        private AlertGuest2Repository alertGuest2Repository;
        private TourInstance selected;
        private TourDetailsService tourDetailsService;
        private TourInstanceService tourInstanceService=new TourInstanceService();
        private ObservableCollection<TourInstance> tour;
        public ObservableCollection<TourInstance> Tours
        {
            get => tour;
            set
            {
                if (value != tour)
                {
                    tour = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<TourInstance> finishedInstances;
        public ObservableCollection<TourInstance> FinishedInstances
        {
            get => finishedInstances;
            set
            {
                if (value != finishedInstances)
                {
                    finishedInstances = value;
                    OnPropertyChanged();
                }
            }
        }

        public ActiveInstanceView(TourInstance active, ObservableCollection<TourInstance> tours, ObservableCollection<TourInstance> finishedInstances)
        {
            InitializeComponent();
            DataContext = this;
            selected=active;
            FinishedInstances = finishedInstances;
            Tours= tours;
            pointsRepository = new CheckPointRepository();
            AllPoints = new ObservableCollection<CheckPoint>();
            CurrentPoint = new ObservableCollection<CheckPoint>();
            tourDetailsService = new TourDetailsService();
            tourInstanceRepository = new TourInstanceRepository();
            tourReservationRepository = new TourReservationRepository();
            alertGuest2Repository = new AlertGuest2Repository();
            FindPointsForSelectedInstance(active);
            FindLastCheckedPoint();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void FindLastCheckedPoint()
        {
            foreach (CheckPoint point in AllPoints)
            {
                if(point.Checked==false)
                {
                    orderCounter = point.Order - 1;
                    CurrentPoint.Add(AllPoints[orderCounter - 1]);
                }
            }
        }

        private void FinishTour_Click(object sender, RoutedEventArgs e)
        {
            FinishInstance();

        }


        private void FinishInstance()
        {
            selected.Finished = true;
            selected.Active = false;
            selected.Attendance = tourDetailsService.MakeAttendancePrecentage(selected.Id);
            tourInstanceRepository.Update(selected);

            FinishedInstances.Add(selected);
            CurrentPoint[0].Checked = true;


            tourInstanceService.FillTour(selected);
            FindActive(selected);
            Finish.IsEnabled = false;
            Next.IsEnabled = false;
            FinishMessage.Content = "This tour is finished";
        }

        private void FindActive(TourInstance selected)
        {
            foreach(TourInstance instance in Tours)
            {
                if(instance.Id == selected.Id)
                {
                    Tours.Remove(instance);
                    break;
                }
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

            AddAlerts(CurrentPoint[0].Id, selected.Id);

        }

        private void UpdateAllPointsListToNextPoint()
        {
            int pointSize = AllPoints.Count;
            List<CheckPoint> points = AllPoints.ToList();
            AllPoints.Clear();
            for (int i = 0; i < orderCounter; i++)
            {
                points[i].Checked = true;
                pointsRepository.Update(points[i]);
                AllPoints.Add(points[i]);
            }
            for (int i = orderCounter; i < pointSize; i++)
            {
                points[i].Checked = false;
                pointsRepository.Update(points[i]);
                AllPoints.Add(points[i]);
            }
        }


        private void AddAlerts(int currentPointId, int _callId)
        {
            List<TourReservation> availableReservations = GetReservationsForTour();

            foreach (TourReservation tour in availableReservations)
            {
                AlertGuest2 alertGuest2 = new AlertGuest2();
                alertGuest2.Availability = false;
                alertGuest2.ReservationId = tour.Id;
                alertGuest2.Guest2Id = tour.GuestId;
                alertGuest2.CheckPointId = currentPointId;
                alertGuest2.InstanceId = selected.Id;
                alertGuest2.Informed = false;
                AlertGuest2 savedAlert = alertGuest2Repository.Save(alertGuest2);


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
