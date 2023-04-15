using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels
{

    public class TourStatisticsViewModel
    {
        public int Year { get; set; }
        private ObservableCollection<TourInstance> instances;
        public ObservableCollection<TourInstance> Instances
        {
            get { return instances; }
            set
            {
                if (value != instances)
                    instances = value;
                OnPropertyChanged("Completed");
            }

        }
        private TourInstanceService instanceService;
        public TourInstance Selected { get; set; }

        GuideService guideService = new GuideService();
        private User loggedUser;
        private TextBox ChosenYear;

        public RelayCommand MostVisitedCommand { get; set; }
        public RelayCommand MostVisitedForYearCommand { get;set; }
        public RelayCommand ViewDetailsCommand { get; set; }

        public TourStatisticsViewModel(User user,TextBox chosenYear)
        {
            loggedUser = user;  
            ChosenYear= chosenYear;
            Instances = new ObservableCollection<TourInstance>();
            instanceService = new TourInstanceService();
            instanceService.SetFinishedInstances(Instances,guideService.GetByUsername(user.Username));
            MakeCommands();
        }
        private void MakeCommands()
        {
            MostVisitedCommand = new RelayCommand(MostVisitedExecuted, CanExecute);
            MostVisitedForYearCommand = new RelayCommand(MostVisitedForYearExecuted, CanExecute);
            ViewDetailsCommand=new RelayCommand(ViewDetailsExecuted, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        public void ViewDetailsExecuted(object sender)
        {
            FinishedTourDetails finishedTourDetails = new FinishedTourDetails(Selected);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;

        }
        public void MostVisitedExecuted(object sender)
        {
            FinishedTourDetails finishedTourDetails = new FinishedTourDetails(instanceService.FindMostVisited());
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;

        }
        public void MostVisitedForYearExecuted(object sender)
        {
            if (Year>0)
            {
                FinishedTourDetails finishedTourDetails = new FinishedTourDetails(instanceService.FindMostVisitedForChosenYear(Year, guideService.GetByUsername(loggedUser.Username)));
                Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;
            }
            ChosenYear.Clear();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
