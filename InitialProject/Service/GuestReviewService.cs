using System.Collections.Generic;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Repository;
using System;

namespace InitialProject.Service
{
    public class GuestReviewService
    {
        private IGuestReviewRepository guestReviewRepository = Injector.CreateInstance<IGuestReviewRepository>();
        private List<GuestReview> guestReviews;

        public GuestReviewService()
        {
            guestReviews = new List<GuestReview>(guestReviewRepository.GetAll());
            SetReservations();
        }

        private void SetReservations()
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            List<AccommodationReservation> accommodationReservations = new List<AccommodationReservation>();
            accommodationReservations = accommodationReservationService.GetAll();

            foreach (GuestReview review in guestReviews)
            {
                AccommodationReservation ownerReservation = accommodationReservations.Find(n => n.Id == review.Reservation.Id);
                if (ownerReservation != null)
                {
                    review.Reservation = ownerReservation;
                }
            }
        }

        public bool HasReview(AccommodationReservation reservation)
        {
            return guestReviewRepository.HasReview(reservation);
        }
        public List<GuestReview> GetAllByOwner(Owner owner)
        {
            List<GuestReview> reviewsByOwner = new List<GuestReview>();

            foreach (GuestReview review in guestReviews)
            {
                if (review.Reservation != null && review.Reservation.Accommodation.Owner.Id == owner.Id)
                    reviewsByOwner.Add(review);
            }
            return reviewsByOwner;
        }

        public List<GuestReview> GetAllToDisplay(Guest1 guest1)
        {
            List<GuestReview> reviewsToDisplay = new List<GuestReview>();

            foreach (GuestReview guestReview in guestReviews)
            {
                AccommodationReservation reservationToReview = guestReview.Reservation;
                bool isThisGuest1 = reservationToReview.Guest.Id == guest1.Id;

                if (IsReviewForDisplay(reservationToReview) && isThisGuest1)
                {
                    reviewsToDisplay.Add(guestReview);
                }
            }
            return reviewsToDisplay;
        }

        private bool IsReviewForDisplay(AccommodationReservation reservationToReview)
        {
            OwnerReviewService ownerReviewService = new OwnerReviewService();

            bool isOwnerReviewed = ownerReviewService.HasReview(reservationToReview);
            bool fiveDaysPassed = (DateTime.Now.Date - reservationToReview.Departure.Date).TotalDays > 5;
            return isOwnerReviewed || fiveDaysPassed;
        }
    
        public void Add(GuestReview guestReview)
        {
            guestReviewRepository.Add(guestReview);
        }

        public bool IsReservationForReview(AccommodationReservation reservation, Owner owner)
        {
            bool stayedLessThan5DaysAgo = (reservation.Departure.Date < DateTime.Now.Date) && (DateTime.Now.Date - reservation.Departure.Date).TotalDays <= 5;
            bool alreadyReviewed = HasReview(reservation);
            bool isThisOwner = reservation.Accommodation.Owner.Id == owner.Id;

            return stayedLessThan5DaysAgo && !alreadyReviewed && isThisOwner;
        }

        public double GetAverageCleanlinessReview(Guest1 guest1)
        {
            double sumCleanlinessReview = 0;
            int counter = 0;

            foreach (GuestReview guestReview in guestReviews)
            {
                if(guestReview.Reservation.Guest.Id == guest1.Id)
                {
                    sumCleanlinessReview += guestReview.Cleanliness;
                    counter++;
                }
            }
            if (counter == 0)
                return 1;
            return sumCleanlinessReview/counter;
        }

        public double GetAverageFollowingRulesReview(Guest1 guest1)
        {
            double sumFollowingRulesReview = 0;
            int counter = 0;

            foreach (GuestReview guestReview in guestReviews)
            {
                if (guestReview.Reservation.Guest.Id == guest1.Id)
                {
                    sumFollowingRulesReview += guestReview.RulesFollowing;
                    counter++;
                }
            }
            if (counter == 0)
                return 1;
            return sumFollowingRulesReview / counter;
        }
        public int GetReviewsNumberByGuest(Guest1 guest1)
        {
            int counter = 0;

            foreach (GuestReview guestReview in guestReviews)
            {
                if (guestReview.Reservation.Guest.Id == guest1.Id)
                {
                    counter++;
                }
            }
            return counter;
        }

        public int GetAverageRating(Guest1 guest1)  //pitati da li je ok da ovako dobijem prosj.ocjenu
        {
           return Convert.ToInt32((GetAverageCleanlinessReview(guest1) + GetAverageFollowingRulesReview(guest1)) / 2);
               //da li ako nema ocjena da prikazem onda 1 zvjezdu i da sve bude na 1(Slideri), svakako prikazujem i broj reviews
        }

    }
}
