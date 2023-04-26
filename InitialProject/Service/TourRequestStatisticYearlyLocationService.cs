using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourRequestStatisticYearlyLocationService
    {
        public TourRequestStatisticYearlyLocationService() { }
        private List<int> GetYearsForLocation(Location location)
        {
            OrdinaryTourRequestsService ordinaryTourRequests = new OrdinaryTourRequestsService();
            List<int> years = new List<int>();
            List<OrdinaryTourRequests> request = ordinaryTourRequests.GetByLocation(location);
            if (request.Count > 0)
            {
                years.Add(request[0].StartDate.Year);
                foreach (OrdinaryTourRequests tourRequest in request)
                {
                    if (!years.Contains(tourRequest.StartDate.Year))
                        years.Add(tourRequest.StartDate.Year);
                }
            }
            return years;
        }
        private int GetByYear(Location location, int year)
        {
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            OrdinaryTourRequestsService ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            if (ordinaryTourRequestsService.GetByLocation(location) != null)
                foreach (OrdinaryTourRequests tourRequest in ordinaryTourRequestsService.GetByLocation(location))
                    if (tourRequest.StartDate.Year == year)
                        ordinaryTourRequests.Add(tourRequest);
            return ordinaryTourRequests.Count;
        }
        public List<GuideOneYearRequestStatisticViewModel> GetYearStatistic(Location location)
        {
            List<GuideOneYearRequestStatisticViewModel> statistics = new List<GuideOneYearRequestStatisticViewModel>();
            if (GetYearsForLocation(location) != null)
            {
                foreach (int year in GetYearsForLocation(location))
                {
                    GuideOneYearRequestStatisticViewModel guideOneYearRequestStatisticViewModel = new GuideOneYearRequestStatisticViewModel();
                    guideOneYearRequestStatisticViewModel.Year = year;
                    guideOneYearRequestStatisticViewModel.Number = GetByYear(location, year);
                    statistics.Add(guideOneYearRequestStatisticViewModel);
                }
            }
            return statistics;
        }
    }
}
