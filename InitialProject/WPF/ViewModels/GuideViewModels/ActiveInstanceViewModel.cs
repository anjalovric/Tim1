using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class ActiveInstanceViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<CheckPoint> AllPoints { get; set; }
        public ObservableCollection<CheckPoint> CurrentPoint { get; set; }
        public ObservableCollection<TourInstance> Tours { get; set; }
        public ObservableCollection<TourInstance> FinishedInstances { get; set; }
        private int orderCounter = 0;
        private TourInstance selected;
        private TourInstanceService tourInstanceService = new TourInstanceService();
        private CheckPointService checkPointService = new CheckPointService();
        private AlertGuest2Service alertGuest2Service = new AlertGuest2Service();

        public RelayCommand NextPointCommand { get; set; }
        public RelayCommand FinishCommand { get; set; }

        public RelayCommand HomeCommand { get; set; }

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
        private string title;
        public string Title
        {
            get => title;
            set
            {
                if (value != title)
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }
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
        private HomeView homeView;
        public ActiveInstanceViewModel(TourInstance active, ObservableCollection<TourInstance> tours, ObservableCollection<TourInstance> finishedInstances,HomeView HomeView)
        {
            selected = active;
            FinishedInstances = finishedInstances;
            Tours = tours;
            homeView= HomeView;
            AllPoints = new ObservableCollection<CheckPoint>();
            CurrentPoint = new ObservableCollection<CheckPoint>();
            checkPointService.FindPointsForSelectedInstance(active, AllPoints);
            SetStartState();
        }
        private void SetStartState()
        {
            Toast = "Hidden";
            TourInstanceTourLocationService tourInstanceTourLocation = new TourInstanceTourLocationService();
            tourInstanceTourLocation.FillTour(selected);
            Title = selected.Tour.Name + ", " + selected.Date + ", " + selected.StartClock;
            NextEnabled = true;
            FinishEnabled = true;
            FindLastCheckedPoint();
            MakeComands();
        }
        private void MakeComands()
        {
            NextPointCommand = new RelayCommand(NextPointExecuted, CanExecute);
            FinishCommand=new RelayCommand(FinishTourExecuted, CanExecute);
            HomeCommand=new RelayCommand (HomeExecuted, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
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
        public void FinishTourExecuted(object sender)
        {
            FinishInstance();
        }
        public void HomeExecuted(object sender)
        {
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = homeView;
        }
        private void FinishInstance()
        {
            TourInstance finished = tourInstanceService.SetFinishStatus(selected);
            FinishedInstances.Add(finished);
            CurrentPoint[0].Checked = true;

            FindActive(finished);
            FinishEnabled = false;
            NextEnabled = false;
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
        private void ChangeCurrentPointToNextState()
        {
            List<CheckPoint> points = AllPoints.ToList();
            CurrentPoint.Remove(points.Find(n => n.Order == orderCounter));
            int nextOrder = orderCounter + 1;
            CurrentPoint.Add(points.Find(n => n.Order == nextOrder));
        }
        public void NextPointExecuted(object sender)
        {
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
