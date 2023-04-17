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
        private List<AccommodationReservation> reservations;
        public AvailableDatesForAccommodationService()
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            reservations = accommodationReservationService.GetAll();
        }

        public bool IsAvailableInDateRange(AccommodationReservation reservation, DateTime startDate, DateTime endDate)
        {
            DateTime date = startDate;
            while (date <= endDate)
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
            foreach (var reservation in reservations)
            {
                bool isSameAccommodation = reservation.Accommodation.Id == reservationToCheck.Accommodation.Id;
                bool isSameReservation = reservation.Id == reservationToCheck.Id;
                if (isSameAccommodation && !isSameReservation)
                    isAvailable = isAvailable && !(date.Date >= reservation.Arrival.Date && date.Date <= reservation.Departure.Date);
            }
            return isAvailable;
        }
    }
}
