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
        public int GetReservationsNumberByMonth(DateTime month)
        {
            List<AccommodationReservation> storedReservations = accommodationReservationService.GetAll();
            return storedReservations.FindAll(r => r.Arrival>month && r.Arrival<=month.AddMonths(1)).Count();
        }
    }
}
