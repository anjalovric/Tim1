using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{

    public class TourStatisticsViewModel:INotifyPropertyChanged
    {
        private string year;
        public string Year
        {
            get => year;
            set
            {
                if (!value.Equals(year))
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
            }
        }
        private string toastVisibility;
        public string ToastVisibility
        {
            get => toastVisibility;
            set
            {
                if (!value.Equals(toastVisibility))
                {
                    toastVisibility = value;
                    OnPropertyChanged("ToastVisibility");
                }
            }
        }
        private string toast;
        public string Toast
        {
            get => toast;
            set
            {
                if (!value.Equals(toast))
                {
                    toast = value;
                    OnPropertyChanged("Toast");
                }
            }
        }
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

        public RelayCommand MostVisitedCommand { get; set; }
        public RelayCommand MostVisitedForYearCommand { get;set; }
        public RelayCommand ViewDetailsCommand { get; set; }
        public RelayCommand OKCommand { get; set; }

        public TourStatisticsViewModel(User user)
        {
            loggedUser = user;  
            Instances = new ObservableCollection<TourInstance>();
            instanceService = new TourInstanceService();
            instanceService.SetFinishedInstances(Instances,guideService.GetByUsername(user.Username));
            ToastVisibility = "Hidden";
            Toast = "Hidden";
            MakeCommands();
        }
        private void MakeCommands()
        {
            MostVisitedCommand = new RelayCommand(MostVisitedExecuted, CanExecute);
            MostVisitedForYearCommand = new RelayCommand(MostVisitedForYearExecuted, CanExecute);
            ViewDetailsCommand=new RelayCommand(ViewDetailsExecuted, CanExecute);
            OKCommand=new RelayCommand(OKExecuted, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        public void ViewDetailsExecuted(object sender)
        {
            FinishedTourDetailsView finishedTourDetails = new FinishedTourDetailsView(Selected);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;

        }
        public void MostVisitedExecuted(object sender)
        {
            FinishedTourDetailsView finishedTourDetails = new FinishedTourDetailsView(instanceService.FindMostVisited());
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;

        }
        public void MostVisitedForYearExecuted(object sender)
        {
            var regex = @"^[0-9]+$";
            string year=Year.ToString();
            var match = Regex.Match(year, regex, RegexOptions.IgnoreCase);
            if (Year != null && Year != "" && match.Success)
            {
                if (instanceService.FindMostVisitedForChosenYear(Convert.ToInt32(Year), guideService.GetByUsername(loggedUser.Username)) != null)
                {
                    FinishedTourDetailsView finishedTourDetails = new FinishedTourDetailsView(instanceService.FindMostVisitedForChosenYear(Convert.ToInt32(Year), guideService.GetByUsername(loggedUser.Username)));
                    Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;              
                }
                else
                   ToastVisibility = "Visible";             
            }
            else
            {
                Toast = "Visible";
            }
            Year = "";
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OKExecuted(object sender)
        {
                ToastVisibility = "Hidden";
                Toast = "Hidden";
        }
    }
}
