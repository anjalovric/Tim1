using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class OwnerReviewService
    {
        private IOwnerReviewRepository ownerReviewRepository = Injector.CreateInstance<IOwnerReviewRepository>();
        private List<OwnerReview> ownerReviews;
        public OwnerReviewService()
        {
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

            foreach (OwnerReview ownerReview in ownerReviews)
            {
                AccommodationReservation reservationToReview = ownerReview.Reservation;
                IsReviewForDisplay(reservationToReview);
                bool isThisOwner = reservationToReview.Accommodation.Owner.Id == owner.Id;

                if (IsReviewForDisplay(reservationToReview) && isThisOwner)
                {
                    reviewsToDisplay.Add(ownerReview);
                }
            }
            return reviewsToDisplay;
        }

        private bool IsReviewForDisplay(AccommodationReservation reservationToReview)
        {
            GuestReviewService guestReviewService = new GuestReviewService();

            bool isGuestReviewed = guestReviewService.HasReview(reservationToReview);
            bool fiveDaysPassed = (DateTime.Now.Date - reservationToReview.Departure.Date).TotalDays > 5;
            return isGuestReviewed || fiveDaysPassed;
        }

        public bool HasReview(AccommodationReservation reservation)
        {
            return ownerReviewRepository.HasReview(reservation);
        }
        public void Add(OwnerReview review)
        {
            ownerReviewRepository.Add(review);
        }
        public double CalculateAverageRateByOwner(Owner owner)
        {
            List<OwnerReview> reviewsToDisplay = GetAllToDisplay(owner);
            int rateSum = 0;
            int numberOfReviews = GetNumberOfReviewsByOwner(owner);
            foreach (OwnerReview review in reviewsToDisplay)
            {
                rateSum += review.Cleanliness;
                rateSum += review.Correctness;
            }
            if (numberOfReviews > 0)
            {
                return (double)rateSum / (2 * numberOfReviews);
            }
            return 0;
        }
        public int GetNumberOfReviewsByOwner(Owner owner)
        {
            List<OwnerReview> reviewsToDisplay = GetAllToDisplay(owner);
            int numberOfReviews = 0;
            foreach (OwnerReview review in reviewsToDisplay)
            {
                numberOfReviews++;
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
        
        public bool IsReservationValidToReview(AccommodationReservation SelectedCompletedReservation)
        {
            return SelectedCompletedReservation.Departure >= DateTime.Now.AddDays(-5);
        }
    }
}