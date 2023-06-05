using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.Service
{
    public class OwnerYearStatisticsService
    {
        private AccommodationReservationService reservationService;
        private CancelledAccommodationReservationService cancelledReservationService;
        private ReschedulingAccommodationRequestService requestService;
        private AccommodationRenovationSuggestionService suggestionService;
        public OwnerYearStatisticsService()
        {
            reservationService = new AccommodationReservationService();
            cancelledReservationService = new CancelledAccommodationReservationService();
            requestService = new ReschedulingAccommodationRequestService();
            suggestionService = new AccommodationRenovationSuggestionService();
        }

        public List<OwnerOneYearStatisticsViewModel> GetStatisticsByYear(Accommodation accommodation)
        {
            List<OwnerOneYearStatisticsViewModel> result = new List<OwnerOneYearStatisticsViewModel>();
            foreach(int year in GetAllYears(accommodation))
            {
                OwnerOneYearStatisticsViewModel oneYearViewModel = new OwnerOneYearStatisticsViewModel();
                oneYearViewModel.Year = year;
                oneYearViewModel.Accommodation = accommodation;
                oneYearViewModel.Reservations = GetReservationNumberByYear(accommodation,year);
                oneYearViewModel.Cancellations = GetCancelledNumberByYear(accommodation,year);
                oneYearViewModel.Reschedulings = GetRequestNumberByYear(accommodation, year);
                oneYearViewModel.RenovationSuggestions = GetSuggestionNumberByYear(accommodation, year);
                result.Add(oneYearViewModel);
            }
            return result;
        }
        public List<int> GetAllYears(Accommodation accommodation)
        {
            HashSet<int> years = new HashSet<int>();
            AvailableDatesForAccommodationService datesService = new AvailableDatesForAccommodationService();
            datesService.GetAllYearsWithReservations(accommodation, years);

            datesService.GetAllYearsWithCancellations(accommodation, years);

            datesService.GetAllYearsWithRequests(accommodation, years);

            datesService.GetAllYearsWithRenovationSuggestions(accommodation, years);
            return years.OrderByDescending(x => x).ToList();
        }

        public int GetReservationNumberByYear(Accommodation accommodation, int year)
        {
            int counter=0;
            foreach (var reservation in reservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                if (reservation.Arrival.Year == year)
                    counter++;
            }
            return counter;
        }

        private int GetCancelledNumberByYear(Accommodation accommodation, int year)
        {
            int counter = 0;
            foreach (var reservation in cancelledReservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                if (reservation.Arrival.Year == year)
                    counter++;
            }
            return counter;
        }

        private int GetRequestNumberByYear(Accommodation accommodation, int year)
        {
            int counter = 0;
            foreach (var request in requestService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodation.Id))
            {
                if (request.Reservation.Arrival.Year == year)
                    counter++;
            }
            return counter;
        }

        private int GetSuggestionNumberByYear(Accommodation accommodation, int year)
        {
            int counter = 0;
            foreach (var suggestion in suggestionService.GetByAccommodation(accommodation))
            {
                if (suggestion.Reservation.Arrival.Year == year)
                    counter++;
            }
            return counter;
        }
        public int GetBusiestYear(Accommodation accommodation)
        {
            double busyness = 0;
            int busiestYear = 0;
            foreach(int year in GetAllYears(accommodation))
            {
                int daysInYear = DateTime.IsLeapYear(year) ? 366 : 365;
                if ((double)GetBusyDaysNumberByYear(accommodation, year)/daysInYear > busyness)
                {
                    busyness = GetBusyDaysNumberByYear(accommodation, year)/daysInYear;
                    busiestYear = year;
                }
            }
            return busiestYear;
        }

        public int GetBusyDaysNumberByYear(Accommodation accommodation, int year)
        {
            int counter = 0;
            foreach (var reservation in reservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                if(reservation.Arrival.Year == year && reservation.Departure.Year == year)
                    counter += (reservation.Departure.Date - reservation.Arrival.Date).Days +1;
                else if(reservation.Arrival.Year == year)
                {
                    DateTime lastDayOfYear = new DateTime(year, 12, 31);
                    counter += (lastDayOfYear.Date - reservation.Arrival.Date).Days +1;
                }
            }
            return counter;
        }

    }
}
