using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class LastYearAccommodationReservationsService
    {
        AccommodationReservationService accommodationReservationService;
        public LastYearAccommodationReservationsService()
        {
            accommodationReservationService = new AccommodationReservationService();
        }
        public int GetReservationsNumberByMonthForGuest(DateTime date, Guest1 guest1, DateTime current)
        {
            List<AccommodationReservation> storedReservations = accommodationReservationService.GetAll();
            return storedReservations.FindAll(r => r.Arrival.Date > date.AddDays(-date.Day).Date && r.Arrival.Date <= date.AddMonths(1).AddDays(-date.Day).Date && r.Arrival.Date<=current.Date && r.Guest.Id==guest1.Id).Count();
        }
        public int GetReservationsNumberByMonthForAccommodation(DateTime date, Accommodation selectedAccommodation, DateTime current)
        {
            List<AccommodationReservation> storedReservations = accommodationReservationService.GetAll();
            return storedReservations.FindAll(r => r.Departure.Date > date.AddDays(-date.Day).Date && r.Departure.Date <= date.AddMonths(1).AddDays(-date.Day).Date && r.Departure.Date<=current.Date && r.Accommodation.Id == selectedAccommodation.Id).Count();
        }
    }
}
