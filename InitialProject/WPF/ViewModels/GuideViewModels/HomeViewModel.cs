using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace InitialProject.WPF.ViewModels
{
    public class HomeViewModel:INotifyPropertyChanged
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public string Home { get; set; }
        private ObservableCollection<TourInstance> tours;
        public ObservableCollection<TourInstance> Tours
        {
            get { return tours; }
            set
            {
                if (value != tours)
                    tours = value;
                OnPropertyChanged();
            }

        }
        private string clock;
        public string Clock
        {
            get { return clock; }
            set
            {
                if (value != clock)
                    clock = value;
                OnPropertyChanged();
            }

        }
        private ObservableCollection<TourInstance> instances;
        public ObservableCollection<TourInstance> FinishedInstances
        {
            get { return instances; }
            set
            {
                if (value != instances)
                    instances = value;
                OnPropertyChanged("Completed");
            }

        }
        public TourInstance Selected { get; set; }
        private GuideService guideService = new GuideService();
        private TourInstanceService tourInstanceService;
        private Guide loggedGuide;
        public RelayCommand StartCommand { get; set; }
        public HomeViewModel(User user, ObservableCollection<TourInstance> Instances)
        {
            loggedGuide = guideService.GetByUsername(user.Username);
            FinishedInstances = Instances;
            tourInstanceService = new TourInstanceService();
            Tours = new ObservableCollection<TourInstance>(tourInstanceService.GetByStart(loggedGuide));
            SetTitle(user);
            StartCommand = new RelayCommand(StartTour_Executed, CanExecute);
            SetClock();
        }
        private void SetClock()
        {
            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void SetTitle(User user)
        {
            GuideService guideService = new GuideService();
            Guide guide = guideService.GetByUsername(user.Username);
            Home = guide.Name + " " + guide.LastName + "'s home page";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void StartTour_Executed(object sender)
        {
            if (tourInstanceService.GetByActive(loggedGuide) == null)
            {
                StartedTourInstanceView startedTourInstanceView = new StartedTourInstanceView(Selected, Tours, FinishedInstances);
                Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = startedTourInstanceView;
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
             Clock = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
