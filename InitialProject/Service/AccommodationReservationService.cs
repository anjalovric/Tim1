using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationReservationService
    {
        private AccommodationReservationRepository accommodationReservationRepository;
        private List<AccommodationReservation> accommodationReservations;
        public AccommodationReservationService()
        {
            accommodationReservationRepository = new AccommodationReservationRepository();
            MakeReservations();
        }

        private void MakeReservations()
        {
            accommodationReservations = accommodationReservationRepository.GetAll();
            AddAccommodations();
        }

        public List<AccommodationReservation> GetAll()
        {
            return accommodationReservations;
        }

        private void AddAccommodations()
        {
            AccommodationService accommodationService = new AccommodationService();
            List<Accommodation> allAccommodation = accommodationService.GetAll();
            foreach(AccommodationReservation reservation in accommodationReservations)
            {
                Accommodation reservationAccommodation = allAccommodation.Find(n => n.Id == reservation.Accommodation.Id);
                if(reservationAccommodation != null)
                {
                    reservation.Accommodation = reservationAccommodation;
                }
            }
        }
    }
}
