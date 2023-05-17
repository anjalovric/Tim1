using InitialProject.Domain.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourRequestStatisticMonthlyLanguageService
    {
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        public TourRequestStatisticMonthlyLanguageService() 
        {
            ordinaryTourRequestsService= new OrdinaryTourRequestsService();
        }
        private List<int> GetMonthsForLanguage(string language,int year)
        {
            List<int> months = new List<int>();
            List<OrdinaryTourRequests> request = GetByYearAndLanguage(language,year);
                months.Add(request[0].StartDate.Month);
                foreach (OrdinaryTourRequests tourRequest in request)
                {
                    if (!months.Contains(tourRequest.StartDate.Month))
                        months.Add(tourRequest.StartDate.Month);
                }
            return months;
        }
        private int GetRequestsCountByMonth(string language, int month,int year)
        {
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
                foreach (OrdinaryTourRequests tourRequest in GetByYearAndLanguage(language,year))
                    if (tourRequest.StartDate.Month == month)
                        ordinaryTourRequests.Add(tourRequest);
            return ordinaryTourRequests.Count;
        }
        public List<MonthRequestStatisticViewModel> GetMonthStatistic(string language,int year)
        {
            List<MonthRequestStatisticViewModel> statistics = new List<MonthRequestStatisticViewModel>();

                foreach (int month in GetMonthsForLanguage(language,year))
                {
                    MonthRequestStatisticViewModel monthRequestStatisticViewModel = new MonthRequestStatisticViewModel();
                    monthRequestStatisticViewModel.MonthNumber = month+".";
                    monthRequestStatisticViewModel.Number = GetRequestsCountByMonth(language, month,year);
                    monthRequestStatisticViewModel.Month = GetMonth(month);
                    statistics.Add(monthRequestStatisticViewModel);
                }
            return statistics;
        }
        private List<OrdinaryTourRequests> GetByYearAndLanguage(string language,int year) 
        {
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            foreach(OrdinaryTourRequests requests in ordinaryTourRequestsService.GetAll())
                if(requests.Language == language && requests.StartDate.Year==year) 
                    ordinaryTourRequests.Add(requests);
            return ordinaryTourRequests;           
        }
        private string GetMonth(int month)
        {
            string Month = null;
            if (month == 1) Month = "January";
            else if (month == 2) Month = "February";
            else if (month == 3) Month = "March";
            else if (month == 4) Month = "April";
            else if (month == 5) Month = "May";
            else if (month == 6) Month = "June";
            else if (month == 7) Month = "July";
            else if (month == 8) Month = "August";
            else if (month == 9) Month = "September";
            else if (month == 10) Month = "October";
            else if (month == 11) Month = "November";
            else Month = "December";
            return Month;
        }
    }
}
