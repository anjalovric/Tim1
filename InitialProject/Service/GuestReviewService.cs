﻿using System.Collections.Generic;
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
                    review.Reservation = ownerReservation;
            }
        }
        public List<GuestReview> GetAll()
        {
            return guestReviews;
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
                    reviewsToDisplay.Add(guestReview);
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
        public void Delete(GuestReview review)
        {
            guestReviewRepository.Delete(review);
        }
    }
}
