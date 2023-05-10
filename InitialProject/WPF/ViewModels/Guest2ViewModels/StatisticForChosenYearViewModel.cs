using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class StatisticForChosenYearViewModel
    {
        private YearlyRequestStatisticsService requestStatisticsService;
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        public List<OrdinaryTourRequests> ordinaryTours;
        private int Year;
        private Model.Guest2 Guest2;
        public double acceptedRequest { get; set; }
        public double invalidRequest { get; set; }
        public double averageNumberOfPeople { get; set; }
        public int chosenYear { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public StatisticForChosenYearViewModel(Model.Guest2 guest2, string year)
        {
            Year = Convert.ToInt32(year);
            chosenYear = Convert.ToInt32(year);    
            Guest2 = guest2;
            CloseCommand = new RelayCommand(Close_Executed,CanExecute);
            acceptedRequest = 0;
            invalidRequest = 0;
            averageNumberOfPeople = 0;
            requestStatisticsService = new YearlyRequestStatisticsService();
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            ordinaryTours = new List<OrdinaryTourRequests>();
            StatisticsForChoosenYear();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Close_Executed(object sender)
        {
            Application.Current.Windows.OfType<StatisticForChosenYearFormView>().FirstOrDefault().Close();
        }
        private void StatisticsForChoosenYear()
        {
            acceptedRequest = requestStatisticsService.ProcentOfAcceptedRequest(chosenYear, Guest2).Round(2);
            invalidRequest = requestStatisticsService.ProcentOfInvalidRequest(chosenYear, Guest2).Round(2);
            averageNumberOfPeople = requestStatisticsService.AverageNumberOfPeopleInAcceptedRequests(chosenYear, Guest2).Round(2);
        }
    }
}
