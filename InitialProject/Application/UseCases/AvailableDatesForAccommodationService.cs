using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class AvailableDatesForAccommodationService
    {
        private AccommodationRenovationService renovationService;
        private AccommodationReservationService accommodationReservationService;
        public AvailableDatesForAccommodationService()
        {
            accommodationReservationService = new AccommodationReservationService();
            renovationService = new AccommodationRenovationService();
        }

        public bool IsAvailableInDateRange(AccommodationReservation reservation, DateTime startDate, DateTime endDate)
        {
            DateTime date = startDate;
            while (date.Date <= endDate.Date)
            {
                if (!IsAvailableOnDate(reservation, date))
                    return false;
                date = date.AddDays(1);
            }
            return true;
        }
        private bool IsAvailableOnDate(AccommodationReservation reservationToCheck, DateTime date)
        {
            bool isAvailable = true;
            List<AccommodationReservation> reservations = accommodationReservationService.GetAll();
            foreach (var reservation in reservations)
            {
                bool isSameAccommodation = reservation.Accommodation.Id == reservationToCheck.Accommodation.Id;
                bool isSameReservation = reservation.Id == reservationToCheck.Id;
                bool isInRenovation = renovationService.IsRenovationOnDate(reservation.Accommodation, date);
                if (isSameAccommodation && !isSameReservation)
                    isAvailable = isAvailable && !(date.Date >= reservation.Arrival.Date && date.Date <= reservation.Departure.Date) && !isInRenovation;
            }
            return isAvailable;
        }
        public void GetAllYearsWithRequests(Accommodation accommodation, HashSet<int> years)
        {
            ReschedulingAccommodationRequestService requestService = new ReschedulingAccommodationRequestService(); 
            foreach (var request in requestService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodation.Id))
            {
                years.Add(request.Reservation.Arrival.Year);
            }
        }
        public void GetAllYearsWithCancellations(Accommodation accommodation, HashSet<int> years)
        {
            CancelledAccommodationReservationService cancelledReservationService = new CancelledAccommodationReservationService();
            foreach (var reservation in cancelledReservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                years.Add(reservation.Arrival.Year);
            }
        }

        public void GetAllYearsWithReservations(Accommodation accommodation, HashSet<int> years)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            foreach (var reservation in reservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                years.Add(reservation.Arrival.Year);
            }
        }
        public void GetAllYearsWithRenovationSuggestions(Accommodation accommodation, HashSet<int> years)
        {
            AccommodationRenovationSuggestionService suggestionService = new AccommodationRenovationSuggestionService();
            foreach (var suggestion in suggestionService.GetByAccommodation(accommodation))
            {
                years.Add(suggestion.Reservation.Arrival.Year);
            }
        }

        public List<AvailableDatesForAccommodation> GetAvailableDateRanges(DateTime startDate, DateTime endDate, int duration, Accommodation accommodation)
        {
            List<AvailableDatesForAccommodation> ranges = new List<AvailableDatesForAccommodation>();
            List<AccommodationReservation> reservations = accommodationReservationService.GetAll();
            duration --;
            foreach(var reservation in reservations.FindAll(n => n.Accommodation.Id == accommodation.Id))
            {
                FindDateRanges(startDate, endDate, duration, ranges, reservation);
            }
            if (reservations.FindAll(n => n.Accommodation.Id == accommodation.Id).Count == 0)
                ranges = GetWithoutReservations(startDate, endDate, duration, accommodation);
            return ranges;
        }

        private void FindDateRanges(DateTime startDate, DateTime endDate, int duration, List<AvailableDatesForAccommodation> ranges, AccommodationReservation reservation)
        {
            DateTime date = startDate;
            while (date.AddDays(duration).Date <= endDate.Date)
            {
                if (IsAvailableInDateRange(reservation, date, date.AddDays(duration)) && ranges.Find(n => n.Arrival.Date == date.Date && n.Departure.Date == date.AddDays(duration).Date) == null)
                    ranges.Add(new AvailableDatesForAccommodation(date.Date, date.AddDays(duration).Date));
                date = date.AddDays(1);
            }
        }

        private List<AvailableDatesForAccommodation> GetWithoutReservations(DateTime startDate, DateTime endDate, int duration, Accommodation accommodation)
        {
            List<AvailableDatesForAccommodation> ranges = new List<AvailableDatesForAccommodation>();
            DateTime date = startDate;
            while (date.AddDays(duration).Date <= endDate.Date)
            {
                ranges.Add(new AvailableDatesForAccommodation(date.Date, date.AddDays(duration).Date));
                date = date.AddDays(1);
            }
            return ranges;
        }
    }
}
