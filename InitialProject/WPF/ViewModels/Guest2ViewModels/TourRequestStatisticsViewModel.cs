using InitialProject.Domain.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class TourRequestStatisticsViewModel
    {
        public double acceptedRequest { get; set; }
        private int counter;
        public double invalidRequest { get; set; }
        public double averageNumberOfPeople { get; set; }
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        private List<OrdinaryTourRequests> OrdinaryTourRequests;
        private Model.Guest2 Guest2;
        public RelayCommand SearchCommand { get; set; }
        private TextBox Year;
        public TourRequestStatisticsViewModel(Model.Guest2 guest2,TextBox year)
        {
            counter = 0;
            Guest2 = guest2;
            Year = year;
            SearchCommand = new RelayCommand(Search_Executed,CanExecute);
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetByGuestId(guest2.Id));
            acceptedRequest = ordinaryTourRequestsService.ProcentOfAcceptedRequest(OrdinaryTourRequests, Guest2);
            invalidRequest = ordinaryTourRequestsService.ProcentOfInvalidRequest(OrdinaryTourRequests, Guest2);
            averageNumberOfPeople = ordinaryTourRequestsService.AverageNumberOfPeopleInAcceptedRequests(OrdinaryTourRequests, Guest2);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Search_Executed(object sender)
        {
            StatisticForChosenYearFormView statisticForChoosenYearFormView = new StatisticForChosenYearFormView(Guest2, Year.Text);
            statisticForChoosenYearFormView.Show();
        }
    }
}
