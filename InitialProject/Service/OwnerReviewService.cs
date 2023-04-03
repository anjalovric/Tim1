﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class OwnerReviewService
    {
        private OwnerReviewRepository ownerReviewRepository;
        private List<OwnerReview> ownerReviews;
        public OwnerReviewService()
        {
            ownerReviewRepository = new OwnerReviewRepository();
            ownerReviews = new List<OwnerReview>(ownerReviewRepository.GetAll());
            MakeReservations();
        }

        public List<OwnerReview> GetAll()
        {
            return ownerReviews;
        }

        public List<OwnerReview> GetAllToDisplay(Owner owner)
        {
            List<OwnerReview> reviewsToDisplay = new List<OwnerReview>();
            GuestReviewService guestReviewService = new GuestReviewService();

            foreach (OwnerReview ownerReview in ownerReviews)
            {
                AccommodationReservation reservationToReview = ownerReview.Reservation;
                bool isGuestReviewed = guestReviewService.IsGuestReviewed(reservationToReview);
                bool fiveDaysPassed = (DateTime.Now.Date - reservationToReview.Departure.Date).TotalDays > 5;
                bool isThisOwner = reservationToReview.Accommodation.Owner.Id == owner.Id;

                if ((isGuestReviewed || fiveDaysPassed) && isThisOwner)
                {
                    reviewsToDisplay.Add(ownerReview);
                }
            }
            return reviewsToDisplay;
        }

        public double CalculateAverageRateByOwner(Owner owner)
        {
            int rateSum = 0;
            int numberOfReviews = GetNumberOfReviewsByOwner(owner);
            foreach (OwnerReview review in ownerReviews)
            {
                if (review.Reservation.Accommodation.Owner.Id == owner.Id)
                {
                    rateSum += review.Cleanliness;
                    rateSum += review.Correctness;
                }
            }

            return (double)rateSum / (2 * numberOfReviews);
        }

        public int GetNumberOfReviewsByOwner(Owner owner)
        {
            int numberOfReviews = 0;
            foreach (OwnerReview review in ownerReviews)
            {
                if (review.Reservation.Accommodation.Owner.Id == owner.Id)
                {
                    numberOfReviews++;
                }
            }
            return numberOfReviews;
        }

        private void MakeReservations()
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            List<AccommodationReservation> accommodationReservations = new List<AccommodationReservation>();
            accommodationReservations = accommodationReservationService.GetAll();

            foreach (OwnerReview review in ownerReviews)
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
