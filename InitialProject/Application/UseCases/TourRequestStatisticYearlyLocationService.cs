using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourRequestStatisticYearlyLocationService
    {
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        public TourRequestStatisticYearlyLocationService() 
        {
            ordinaryTourRequestsService= new OrdinaryTourRequestsService();
        }
        private List<int> GetYearsForLocation(Location location)
        {
            List<int> years = new List<int>();
            List<OrdinaryTourRequests> requests = ordinaryTourRequestsService.GetByLocation(location);
            if (requests.Count > 0)
            {
                years.Add(requests[0].StartDate.Year);
                foreach (OrdinaryTourRequests tourRequest in requests)
                {
                    if (!years.Contains(tourRequest.StartDate.Year))
                        years.Add(tourRequest.StartDate.Year);
                }
            }
            return years;
        }
        private int GetRequestsCountByYear(Location location, int year)
        {
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            if (ordinaryTourRequestsService.GetByLocation(location) != null)
                foreach (OrdinaryTourRequests tourRequest in ordinaryTourRequestsService.GetByLocation(location))
                    if (tourRequest.StartDate.Year == year)
                        ordinaryTourRequests.Add(tourRequest);
            return ordinaryTourRequests.Count;
        }
        public List<GuideOneYearRequestStatisticViewModel> GetLocationYearStatistic(Location location)
        {
            List<GuideOneYearRequestStatisticViewModel> statistics = new List<GuideOneYearRequestStatisticViewModel>();
            if (GetYearsForLocation(location) != null)
            {
                foreach (int year in GetYearsForLocation(location))
                {
                    GuideOneYearRequestStatisticViewModel guideOneYearRequestStatisticViewModel = new GuideOneYearRequestStatisticViewModel();
                    guideOneYearRequestStatisticViewModel.Year = year;
                    guideOneYearRequestStatisticViewModel.Number = GetRequestsCountByYear(location, year);
                    statistics.Add(guideOneYearRequestStatisticViewModel);
                }
            }
            return statistics;
        }
    }
}
