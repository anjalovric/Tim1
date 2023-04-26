using InitialProject.Domain.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourRequestStatisticYearlyService
    {
        public TourRequestStatisticYearlyService() { }
        private List<int> GetYearsForLanguage(string language)
        {
            OrdinaryTourRequestsService ordinaryTourRequests = new OrdinaryTourRequestsService();
            List<int> years= new List<int>();
            List<OrdinaryTourRequests> request = ordinaryTourRequests.GetByLanguage(language);
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
        private int GetByYear(string language,int year) 
        {
            List<OrdinaryTourRequests> ordinaryTourRequests= new List<OrdinaryTourRequests>();
            OrdinaryTourRequestsService ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            if(ordinaryTourRequestsService.GetByLanguage(language) != null)
                foreach(OrdinaryTourRequests tourRequest in ordinaryTourRequestsService.GetByLanguage(language))
                    if(tourRequest.StartDate.Year == year)
                        ordinaryTourRequests.Add(tourRequest);
            return ordinaryTourRequests.Count;
        }
        public List<GuideOneYearRequestStatisticViewModel> GetYearStatistic(string language) 
        {
            List<GuideOneYearRequestStatisticViewModel> statistics = new List<GuideOneYearRequestStatisticViewModel>();
            if (GetYearsForLanguage(language) != null)
            {
                foreach (int year in GetYearsForLanguage(language))
                {
                    GuideOneYearRequestStatisticViewModel guideOneYearRequestStatisticViewModel = new GuideOneYearRequestStatisticViewModel();
                    guideOneYearRequestStatisticViewModel.Year = year;
                    guideOneYearRequestStatisticViewModel.Number= GetByYear(language, year);
                    statistics.Add(guideOneYearRequestStatisticViewModel);
                }
            }
            return statistics;
        }
    }
}
