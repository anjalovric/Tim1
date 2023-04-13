using System;
using System.Collections.Generic;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{//sta sa cancelled repoz.koji pravim i ovdje? da li da bude injector?
    public class AccommodationReservationService
    {
        private List<AccommodationReservation> reservations;
        private IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();

        public AccommodationReservationService()
        {
            MakeReservations();
        }

        public List<AccommodationReservation> GetAll()
        {
            return reservations;
        }

        public void MakeReservations()
        {
            reservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
            SetAccommodations();
            SetGuests();
        }

        public void Add(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Add(reservation);
        }
        public List<AccommodationReservation> GetCompletedReservations(Guest1 guest1)
        {
            List<AccommodationReservation> CompletedReservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.Departure < DateTime.Now && reservation.Guest.Id == guest1.Id)
                {
                    CompletedReservations.Add(reservation);
                }
            }
            return CompletedReservations;
        }
        public List<AccommodationReservation> GetNotCompletedReservations(Guest1 guest1)
        {
            List<AccommodationReservation> NotCompletedReservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.Departure >= DateTime.Now && reservation.Guest.Id == guest1.Id)
                {
                    NotCompletedReservations.Add(reservation);
                }
            }
            return NotCompletedReservations;
        }
       
        private void SetAccommodations()
        {
            AccommodationService accommodationService = new AccommodationService();
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Accommodation = accommodationService.GetById(reservation.Accommodation.Id);
            }
        }
        
        public void Delete(AccommodationReservation accommodationReservation)
        {
            accommodationReservationRepository.Delete(accommodationReservation);
        }
        private void SetGuests()
        {
            Guest1Service guest1Service = new Guest1Service();
            List<Guest1> allGuest = guest1Service.GetAll(); 
            foreach(AccommodationReservation reservation in reservations)
            {
                Guest1 guestForReservation = allGuest.Find(n => n.Id == reservation.Guest.Id);
                if(guestForReservation != null)
                    reservation.Guest = guestForReservation;
            }
        }
        public void Update(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Update(reservation);
        }

        public List<AccommodationReservation> GetAllForReviewByOwner(Owner owner)
        {
            List<AccommodationReservation> reservationsToReview = new List<AccommodationReservation>();
            GuestReviewService guestReviewService = new GuestReviewService();

            if (reservations.Count > 0)
            {
                foreach (AccommodationReservation reservation in reservations)
                {
                    if (guestReviewService.IsReservationForReview(reservation, owner))
                        reservationsToReview.Add(reservation);
                }
            }
            return reservationsToReview;
        }
    }
}