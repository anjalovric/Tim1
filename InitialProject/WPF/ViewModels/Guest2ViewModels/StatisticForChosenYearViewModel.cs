using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class StatisticForChosenYearViewModel
    {
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        public List<OrdinaryTourRequests> ordinaryTours;
        private int Year;
        private Model.Guest2 Guest2;
        public double acceptedRequest { get; set; }
        public double invalidRequest { get; set; }
        public double averageNumberOfPeople { get; set; }
        public int chosenYear { get; set; }
        public StatisticForChosenYearViewModel(Model.Guest2 guest2, string year)
        {
            Year = Convert.ToInt32(year);
            chosenYear = Convert.ToInt32(year);    
            Guest2 = guest2;
            acceptedRequest = 0;
            invalidRequest = 0;
            averageNumberOfPeople = 0;
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            ordinaryTours = new List<OrdinaryTourRequests>();
            StatisticsForChoosenYear();
        }
        private void StatisticsForChoosenYear()
        {
            GetRequestsForChosenYear(Year, Guest2);
            acceptedRequest = ordinaryTourRequestsService.ProcentOfAcceptedRequest(ordinaryTours, Guest2);
            invalidRequest = ordinaryTourRequestsService.ProcentOfInvalidRequest(ordinaryTours, Guest2);
            averageNumberOfPeople = ordinaryTourRequestsService.AverageNumberOfPeopleInAcceptedRequests(ordinaryTours, Guest2);
        }
        public List<OrdinaryTourRequests> GetRequestsForChosenYear(int year, Guest2 guest)
        {
            foreach (OrdinaryTourRequests request in ordinaryTourRequestsService.GetByGuestId(guest.Id))
            {
                if (request.StartDate.Year == year)
                    ordinaryTours.Add(request);
            }
            return ordinaryTours;
        }
    }
}
