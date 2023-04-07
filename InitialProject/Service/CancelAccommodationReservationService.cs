using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class CancelAccommodationReservationService
    {
        private CancelledAccommodationReservationRepository cancelledAccommodationReservationRepository;
        private AccommodationReservationService accommodationReservationService;


        public CancelAccommodationReservationService()
        {
            cancelledAccommodationReservationRepository = new CancelledAccommodationReservationRepository();
            accommodationReservationService = new AccommodationReservationService();
        }

        public void Add(AccommodationReservation reservation)
        {
            cancelledAccommodationReservationRepository.Add(reservation);
        }

        public List<AccommodationReservation> GetAll()
        {
            return cancelledAccommodationReservationRepository.GetAll();
        }
    }
}
