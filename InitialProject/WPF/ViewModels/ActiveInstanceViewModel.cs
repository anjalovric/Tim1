using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace InitialProject.WPF.ViewModels
{
    public class ActiveInstanceViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<CheckPoint> AllPoints { get; set; }
        public ObservableCollection<CheckPoint> CurrentPoint { get; set; }

        private int orderCounter = 0;
        private TourInstance selected;
        private Button Next;
        private Button Finish;
        private StackPanel FinishMessage;

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
        private TourInstanceService tourInstanceService = new TourInstanceService();
        private CheckPointService checkPointService = new CheckPointService();
        private AlertGuest2Service alertGuest2Service = new AlertGuest2Service();
   
        public ActiveInstanceViewModel(TourInstance active, ObservableCollection<TourInstance> tours, ObservableCollection<TourInstance> finishedInstances,Button next,Button finish,StackPanel finishMessage)
        {
            selected = active;
            FinishedInstances = finishedInstances;
            Tours = tours;
            Next= next;
            Finish= finish;
            FinishMessage = finishMessage;
            AllPoints = new ObservableCollection<CheckPoint>();
            CurrentPoint = new ObservableCollection<CheckPoint>();
            checkPointService.FindPointsForSelectedInstance(active, AllPoints);
            FindLastCheckedPoint();
        }
        private void FindLastCheckedPoint()
        {
            foreach (CheckPoint point in AllPoints)
            {
                if (point.Checked == false)
                {
                    orderCounter = point.Order - 1;
                    CurrentPoint.Add(AllPoints[orderCounter - 1]);
                    break;
                }
            }
        }

        public void FinishTour()
        {
            FinishInstance();
        }

        private void FinishInstance()
        {
            TourInstance finished = tourInstanceService.SetFinishStatus(selected);
            FinishedInstances.Add(finished);
            CurrentPoint[0].Checked = true;

            FindActive(finished);
            Finish.IsEnabled = false;
            Next.IsEnabled = false;
            FinishMessage.Visibility = Visibility.Visible;
        }

        private void FindActive(TourInstance selected)
        {
            foreach (TourInstance instance in Tours)
            {
                if (instance.Id == selected.Id)
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
        public void NextPoint()
        {
            ChangeCurrentPointToNextState();

            orderCounter++;

            if (orderCounter == AllPoints.ToList().Count)
            {
                Next.IsEnabled = false;
                FinishInstance();
            }

            checkPointService.UpdateAllPointsListToNextPoint(AllPoints, orderCounter);
            alertGuest2Service.AddAlerts(CurrentPoint[0].Id, selected.Id, selected);

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
