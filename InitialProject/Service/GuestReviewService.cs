﻿using System.Collections.Generic;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class GuestReviewService
    {
        private GuestReviewRepository guestReviewRepository;
        private List<GuestReview> guestReviews;

        public GuestReviewService()
        {
            guestReviewRepository = new GuestReviewRepository();
            guestReviews = new List<GuestReview>(guestReviewRepository.GetAll());
        }

        public bool IsGuestReviewed(AccommodationReservation accommodationReservation)
        {
            return guestReviewRepository.HasReview(accommodationReservation);
        }

        private void MakeReservations()
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
    }
}