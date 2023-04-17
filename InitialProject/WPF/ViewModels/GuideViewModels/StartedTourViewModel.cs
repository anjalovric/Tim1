using InitialProject.Model;
using InitialProject.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InitialProject.WPF.ViewModels
{
    public class StartedTourViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CheckPoint> AllPoints { get; set; }
        public ObservableCollection<CheckPoint> CurrentPoint { get; set; }
        public ObservableCollection<TourInstance> Tours { get; set; }
        public ObservableCollection<TourInstance> FinishedInstances { get; set; }

        private int orderCounter = 1;
        private TourInstance selected;
        private bool nextEnabled;
        public bool NextEnabled
        {
            get => nextEnabled;
            set
            {
                if (value != nextEnabled)
                {
                    nextEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool finishEnabled;
        public bool FinishEnabled
        {
            get => finishEnabled;
            set
            {
                if (value != finishEnabled)
                {
                    finishEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Title { get; set; }
        private string toast;
        public string Toast
        {
            get => toast;
            set
            {
                if (value != toast)
                {
                    toast = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand NextCommand { get; set; }
        public RelayCommand FinishCommand { get; set; }

        public StartedTourViewModel(TourInstance active, ObservableCollection<TourInstance> tours, ObservableCollection<TourInstance> finishedInstances)
        {
            AllPoints = new ObservableCollection<CheckPoint>();
            CurrentPoint = new ObservableCollection<CheckPoint>();
            Tours = tours;
            selected = active;
            FinishedInstances = finishedInstances;
            SetStartState();
            MakeCommands();
        }
        private void SetStartState()
        {
            Toast = "Hidden";
            NextEnabled = true;
            FinishEnabled = true;
            Title = selected.Tour.Name + ", " + selected.Date + ", " + selected.StartClock;
            StartInstance();
        }
        private void MakeCommands()
        {
            NextCommand = new RelayCommand(NextExecuted, CanExecute);
            FinishCommand = new RelayCommand(FinishExecuted, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void StartInstance()
        {
            TourInstanceService tourInstanceService = new TourInstanceService();
            CheckPointService checkPointService = new CheckPointService();
            tourInstanceService.ActivateTour(selected);
            checkPointService.FindPointsForSelectedInstance(selected, AllPoints);
            checkPointService.CheckFirstPoint(AllPoints);
            SetFirstCheckPoint();
        }
        private void SetFirstCheckPoint()
        {
            AlertGuest2Service alertGuest2Service = new AlertGuest2Service();
            if (AllPoints.Count != 0)
            {
                CurrentPoint.Add(AllPoints.ToList().Find(n => n.Order == orderCounter));
                alertGuest2Service.AddAlerts(CurrentPoint[0].Id, selected.Id, selected);
            }
        }
        public void FinishExecuted(object sender)
        {
            FinishInstance();
        }
        private void FinishInstance()
        {
            TourInstanceService tourInstanceService = new TourInstanceService();
            TourInstance finished = tourInstanceService.SetFinishStatus(selected);
            FinishedInstances.Add(finished);
            CurrentPoint[0].Checked = true;

            FindActive(finished);
            NextEnabled = false;
            FinishEnabled = false;
            Toast = "Visible";
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
        public void NextExecuted(object sender)
        {
            CheckPointService checkPointService = new CheckPointService();
            AlertGuest2Service alertGuest2Service = new AlertGuest2Service();
            ChangeCurrentPointToNextState();
            orderCounter++;
            if (orderCounter == AllPoints.ToList().Count)
            {
                NextEnabled = false;
                FinishInstance();
            }
            checkPointService.UpdateAllPointsListToNextPoint(AllPoints, orderCounter);
            alertGuest2Service.AddAlerts(CurrentPoint[0].Id, selected.Id, selected);
        }
        private void ChangeCurrentPointToNextState()
        {
            List<CheckPoint> points = AllPoints.ToList();

            CurrentPoint.Remove(points.Find(n => n.Order == orderCounter));
            int nextOrder = orderCounter + 1;
            CurrentPoint.Add(points.Find(n => n.Order == nextOrder));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
