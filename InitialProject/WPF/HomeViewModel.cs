using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF
{
    public class HomeViewModel
    {
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
        private GuideService guideService=new GuideService();
        private TourInstanceService tourInstanceService;
        private Guide loggedGuide;
        public HomeViewModel(User user,ObservableCollection<TourInstance> Instances)
        {
            loggedGuide=guideService.GetByUsername(user.Username);
            FinishedInstances = Instances;
            tourInstanceService = new TourInstanceService();
            Tours = new ObservableCollection<TourInstance>(tourInstanceService.GetByStart(loggedGuide));
            SetTitle(user);
        }

        private void SetTitle(User user)
        {
            GuideService guideService = new GuideService();
            Guide guide=guideService.GetByUsername(user.Username);
            Home = guide.Name + " " + guide.Username + "'s home page";
        }
     

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartTour()
        {
            if (tourInstanceService.GetByActive(loggedGuide) == null)
            {
                StartedTourInstanceView startedTourInstanceView = new StartedTourInstanceView(Selected, Tours, FinishedInstances);
                Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = startedTourInstanceView;

            }
        }
    }
}
