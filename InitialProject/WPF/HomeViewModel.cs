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

        public ObservableCollection<TourInstance> Tours { get; set; }
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

        public HomeViewModel(ObservableCollection<TourInstance> Instances)
        {
            FinishedInstances = Instances;
            TourInstanceService tourInstanceService = new TourInstanceService();
            Tours = new ObservableCollection<TourInstance>(tourInstanceService.GetByStart());
        }


     

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartTour()
        {
            StartedTourInstanceView startedTourInstanceView = new StartedTourInstanceView(Selected, Tours, FinishedInstances);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = startedTourInstanceView;

        }
    }
}
