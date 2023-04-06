using InitialProject.Model;
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
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels
{

    public class TourStatisticsViewModel
    {
        public int Year { get; set; }
        public ObservableCollection<TourInstance> Instances { get; set; }

        private TourInstanceService instanceService;
        public TourInstance Selected { get; set; }
        public TourStatisticsViewModel()
        {
            Instances = new ObservableCollection<TourInstance>();
            instanceService = new TourInstanceService();
            instanceService.SetFinishedInstances(Instances);
        }
        public void ViewDetails(DataGrid TourListDataGrid)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            FinishedTourDetails finishedTourDetails = new FinishedTourDetails(currentTourInstance);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;

        }

        public void MostVisited()
        {
            FinishedTourDetails finishedTourDetails = new FinishedTourDetails(instanceService.FindMostVisited());
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;

        }


        public void MostVisitedForYear()
        {
            FinishedTourDetails finishedTourDetails = new FinishedTourDetails(instanceService.FindMostVisitedForChosenYear(Year));
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = finishedTourDetails;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
