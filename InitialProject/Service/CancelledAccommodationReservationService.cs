using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.Service
{
    public class CancelledAccommodationReservationService
    {
        private ICancelledAccommodationReservationRepository cancelledAccommodationReservationRepository = Injector.CreateInstance<ICancelledAccommodationReservationRepository>();
        private List<AccommodationReservation> cancelledReservations;
        public CancelledAccommodationReservationService()
        {
            cancelledReservations = new List<AccommodationReservation>(cancelledAccommodationReservationRepository.GetAll());
            SetAccommodations();
        }
        private void SetAccommodations()
        {
            AccommodationService accommodationService = new AccommodationService();
            foreach (AccommodationReservation reservation in cancelledReservations)
            {
                reservation.Accommodation = accommodationService.GetById(reservation.Accommodation.Id);
            }
        }
        public void Add(AccommodationReservation reservation)
        {
            cancelledAccommodationReservationRepository.Add(reservation);
        }
        public List<AccommodationReservation> GetAll()
        {
            return cancelledReservations;
        }    
        public bool IsCancelled(AccommodationReservation reservation)
        {
            return cancelledReservations.Find(n => n.Id == reservation.Id) != null;
        }

        public void Delete(AccommodationReservation reservation)
        {
            cancelledAccommodationReservationRepository.Delete(reservation);
        }
    }
}