using InitialProject.Domain.Model;
using InitialProject.Help;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        public RelayCommand CloseCommand { get; set; }
        public string Year { get; set; }
        public ICommand HelpCommandInViewModel { get; }
        private TourRequestStatisticsView org;
        public TourRequestStatisticsViewModel(Model.Guest2 guest2, TourRequestStatisticsView org)
        {
            Guest2 = guest2;
            MakeCommand();
            this.org = org;
            requestStatisticsService = new RequestStatisticsService();
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetOnlyOrdinaryRequestsByGuestId(guest2.Id));
            acceptedRequest = requestStatisticsService.ProcentOfAcceptedRequest( Guest2);
            invalidRequest = requestStatisticsService.ProcentOfInvalidRequest( Guest2);
            averageNumberOfPeople = requestStatisticsService.AverageNumberOfPeopleInAcceptedRequests( Guest2);
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
        }
        private void MakeCommand()
        {
            SearchCommand = new RelayCommand(Search_Executed, CanExecute);
            CloseCommand = new RelayCommand(Close_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Search_Executed(object sender)
        {
            //Keyboard.ClearFocus();
            StatisticForChosenYearFormView statisticForChoosenYearFormView = new StatisticForChosenYearFormView(Guest2,Year);
            statisticForChoosenYearFormView.Show();
           
        }
        private void Close_Executed(object sender)
        {
            Application.Current.Windows.OfType<TourRequestStatisticsView>().FirstOrDefault().Close();
        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForStatistics(str, org);
            }
        }
    }
}
