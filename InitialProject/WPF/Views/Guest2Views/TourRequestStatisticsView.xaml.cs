using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Domain.Model;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for TourRequestStatisticsView.xaml
    /// </summary>
    public partial class TourRequestStatisticsView : Window
    {
        public double acceptedRequest { get; set; }
        public double invalidRequest { get; set; }
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        private List<OrdinaryTourRequests> OrdinaryTourRequests;
        public TourRequestStatisticsView(Model.Guest2 guest2)
        {
            InitializeComponent();
            DataContext = this;
            acceptedRequest = 0;
            invalidRequest = 0;
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetAll());
            ProcentOfAcceptedRequest();
            ProcentOfInvalidRequest();
        }
        private void ProcentOfAcceptedRequest()
        {
            foreach(OrdinaryTourRequests request in OrdinaryTourRequests)
            {
                if (request.Status == "Valid")
                {
                    acceptedRequest++;
                }
            }
            acceptedRequest /= OrdinaryTourRequests.Count();
            acceptedRequest *= 100;
        }
        private void ProcentOfInvalidRequest()
        {
            foreach (OrdinaryTourRequests request in OrdinaryTourRequests)
            {
                if (request.Status == "Invalid")
                {
                    invalidRequest++;
                }
            }
            invalidRequest /= OrdinaryTourRequests.Count();
            invalidRequest *= 100;
        }
    }
}
