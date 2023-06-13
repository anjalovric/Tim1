using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.WPF.Views.Guest1Views;
using System.Linq;

namespace InitialProject.Service
{
    public class AccommodationReservationService
    {
        private List<AccommodationReservation> reservations;
        private IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
        private Guest1Service guest1Service;
        public AccommodationReservationService()
        {
            guest1Service = new Guest1Service();
            MakeReservations();
        }
        public List<AccommodationReservation> GetAll()
        {
            MakeReservations();
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
                    CompletedReservations.Add(reservation);
            }
            CompletedReservations.Reverse();
            return CompletedReservations;
        }
        public List<AccommodationReservation> GetNotCompletedReservations(Guest1 guest1)
        {
            List<AccommodationReservation> NotCompletedReservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in reservations)
            {
                if (reservation.Departure >= DateTime.Now && reservation.Guest.Id == guest1.Id)
                    NotCompletedReservations.Add(reservation);
            }
            NotCompletedReservations.Reverse();
            return NotCompletedReservations;
        }    
        private void SetAccommodations()
        {
            AccommodationService accommodationService = new AccommodationService();
            foreach (AccommodationReservation reservation in reservations)
                reservation.Accommodation = accommodationService.GetById(reservation.Accommodation.Id);
        } 
        public void Delete(AccommodationReservation accommodationReservation)
        {
            accommodationReservationRepository.Delete(accommodationReservation);
        }
        private void SetGuests()
        {
            
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
        public DateTime GetNewSuperGuestActivationDateIfPossible(Guest1 guest1)
        {
            int counter = 0;
            List<DateTime> departureDates = new List<DateTime>();
            foreach(AccommodationReservation reservation in reservations)
            {
                if (reservation.Guest.Id == guest1.Id && IsLastYearReservationCompleted(reservation))
                {
                    counter++;
                    departureDates.Add(reservation.Departure);
                }                   
            }
            if (counter >= 10)
            {
                var tenthDeparture = departureDates.OrderBy(d => d).ElementAtOrDefault(9);
                return tenthDeparture;
            }
            return DateTime.MinValue;
        }
        public bool IsLastYearReservationCompleted(AccommodationReservation reservation)
        {
            return reservation.Departure <= DateTime.Now && reservation.Departure > DateTime.Now.AddYears(-1);
        }  
        public DateTime GetProlongActivationDate(Guest1 guest1, DateTime activationDate)
        {
            List<AccommodationReservation> completedReservations = reservations.FindAll(n => n.Guest.Id == guest1.Id && n.Departure > activationDate && n.Departure <= activationDate.AddYears(1) && n.Departure <= DateTime.Now);//number of reservations in next year from activationDate
            int counter = completedReservations.Count;
            if(counter >= 10)
            {
                List<DateTime> departureDates = new List<DateTime>();
                foreach (AccommodationReservation reservation in completedReservations)
                    departureDates.Add(reservation.Departure);
                var tenthDeparture = departureDates.OrderBy(d => d).ElementAtOrDefault(9);
                return tenthDeparture;
            }
            return DateTime.MinValue;   //if expired(<10 reservations) or not expired(<10 reservations but 1 year hasn't passed)
        }
        public bool IsGuestCommentSpecial(Guest1 guest1, int locationId, DateTime CreatingCommentDate)
        {
            return reservations.Find(r => r.Accommodation.Location.Id == locationId && r.Guest.Id == guest1.Id && r.Arrival<CreatingCommentDate) != null;
        }
        public bool HadReservationOnLocation(String username, Location location)
        {
            MakeReservations();
            return reservations.Find(r => r.Accommodation.Location.Id == location.Id && r.Guest.Username == username && r.Departure < DateTime.Now) != null;
        }
    }
}