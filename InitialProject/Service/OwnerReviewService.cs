using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    internal class OwnerReviewService
    {
        private OwnerReviewRepository ownerReviewRepository;
        private List<OwnerReview> ownerReviews;
        public OwnerReviewService()
        {
            ownerReviewRepository = new OwnerReviewRepository();
            ownerReviews = new List<OwnerReview>(ownerReviewRepository.GetAll());

        }

        public List<OwnerReview> GetAll()
        {
            return ownerReviewRepository.GetAll();
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
                //bool isThisOwner = reservationToReview.Accommodation.Owner.Id == owner.Id;

                if ((isGuestReviewed || fiveDaysPassed) ) // && isThisOwner
                {
                    reviewsToDisplay.Add(ownerReview);
                }
            }
            return reviewsToDisplay;
        }
    }
}
