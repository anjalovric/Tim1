using InitialProject.Domain.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class TourRequestStatisticsViewModel
    {
        public double acceptedRequest { get; set; }
        public double invalidRequest { get; set; }
        public double averageNumberOfPeople { get; set; }
        private RequestStatisticsService requestStatisticsService;
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        private List<OrdinaryTourRequests> OrdinaryTourRequests;
        private Model.Guest2 Guest2;
        public RelayCommand SearchCommand { get; set; }
        private TextBox Year;
        public TourRequestStatisticsViewModel(Model.Guest2 guest2,TextBox year)
        {
            Guest2 = guest2;
            Year = year;
            SearchCommand = new RelayCommand(Search_Executed,CanExecute);
            requestStatisticsService = new RequestStatisticsService();
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetByGuestId(guest2.Id));
            acceptedRequest = requestStatisticsService.ProcentOfAcceptedRequest(OrdinaryTourRequests, Guest2);
            invalidRequest = requestStatisticsService.ProcentOfInvalidRequest(OrdinaryTourRequests, Guest2);
            averageNumberOfPeople = requestStatisticsService.AverageNumberOfPeopleInAcceptedRequests(OrdinaryTourRequests, Guest2);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Search_Executed(object sender)
        {
            var regex = "[1-9][0-9]{3}"; //ovo izmijeniti
            Match match = Regex.Match(Year.Text, regex, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                StatisticForChosenYearFormView statisticForChoosenYearFormView = new StatisticForChosenYearFormView(Guest2, Year.Text);
                statisticForChoosenYearFormView.Show();
            }
            else
            {
                MessageBox.Show("Niste dobro unijeli godinu.");
            }
        }
    }
}
