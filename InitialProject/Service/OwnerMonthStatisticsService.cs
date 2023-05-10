using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.Service
{
    public class OwnerMonthStatisticsService
    {
        private AccommodationReservationService reservationService;
        private CancelledAccommodationReservationService cancelledReservationService;
        private ReschedulingAccommodationRequestService requestService;
        private AccommodationRenovationSuggestionService suggestionService;
        public OwnerMonthStatisticsService()
        {
            reservationService = new AccommodationReservationService();
            cancelledReservationService = new CancelledAccommodationReservationService();
            requestService = new ReschedulingAccommodationRequestService();
            suggestionService = new AccommodationRenovationSuggestionService();
        }

        public List<OwnerOneMonthStatisticViewModel> GetMonthStatistics(Accommodation accommodation, int year)
        {
            List<OwnerOneMonthStatisticViewModel> result = new List<OwnerOneMonthStatisticViewModel>();
            for(int month=1; month <=12; month ++)
            {
                OwnerOneMonthStatisticViewModel oneMonthViewModel = new OwnerOneMonthStatisticViewModel();
                oneMonthViewModel.Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
                oneMonthViewModel.Year = year;
                oneMonthViewModel.Accommodation = accommodation;
                oneMonthViewModel.Reservations = GetReservationNumberByMonth(accommodation, year, month);
                oneMonthViewModel.Cancellations = GetCancelledNumberByMonth(accommodation, year, month);
                oneMonthViewModel.Reschedulings = GetRequestNumberByMonth(accommodation, year, month);
                oneMonthViewModel.RenovationSuggestions = GetSuggestionNumberByMonth(accommodation, year, month);
                result.Add(oneMonthViewModel);
            }
            return result;
        }
        private int GetReservationNumberByMonth(Accommodation accommodation, int year, int month)
        {
            int counter = 0;
            foreach(var reservation in reservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                if(reservation.Arrival.Year == year && reservation.Arrival.Month == month)
                {
                    counter++;
                }
            }
            return counter;
        }

        private int GetCancelledNumberByMonth(Accommodation accommodation, int year, int month)
        {
            int counter = 0;
            foreach (var reservation in cancelledReservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                if (reservation.Arrival.Year == year && reservation.Arrival.Month == month)
                {
                    counter++;
                }
            }
            return counter;
        }

        private int GetRequestNumberByMonth(Accommodation accommodation, int year, int month)
        {
            int counter = 0;
            foreach (var request in requestService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodation.Id))
            {
                if (request.Reservation.Arrival.Year == year && request.Reservation.Arrival.Month == month)
                {
                    counter++;
                }
            }
            return counter;
        }

        private int GetSuggestionNumberByMonth(Accommodation accommodation, int year, int month)
        {
            int counter = 0;
            foreach (var suggestion in suggestionService.GetByAccommodation(accommodation))
            {
                if (suggestion.Reservation.Arrival.Year == year && suggestion.Reservation.Arrival.Month == month)
                    counter++;
            }
            return counter;
        }
        public string GetBusiestMonth(Accommodation accommodation, int year)
        {
            double busyness = 0;
            int busiestMonth = 0;
            for (int month= 1; month <=12; month++)
            {
                int daysInMonth = DateTime.DaysInMonth(year, month);
                if (GetBusyDaysNumberInMonth(accommodation, year, month) / daysInMonth > busyness)
                {
                    busyness = GetBusyDaysNumberInMonth(accommodation, year, month) / daysInMonth;
                    busiestMonth = month;
                }
            }
            return busiestMonth !=0 ? DateTimeFormatInfo.CurrentInfo.GetMonthName(busiestMonth) : "";
        }

        private double GetBusyDaysNumberInMonth(Accommodation accommodation, int year, int month)
        {
            int counter = 0;
            foreach (var reservation in reservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                if(reservation.Arrival.Year == year && reservation.Arrival.Month == month && reservation.Departure.Month==month)
                    counter += reservation.Departure.Day - reservation.Arrival.Day + 1;
                else if(reservation.Arrival.Year == year && reservation.Arrival.Month == month && reservation.Departure.Year==year)
                {
                    DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    counter += lastDayOfMonth.Day - reservation.Arrival.Day+1;
                }
            }
            return counter;
        }
    }
}
