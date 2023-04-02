using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class GuestReviewService
    {
        private GuestReviewRepository guestReviewRepository;

        public GuestReviewService()
        {
            guestReviewRepository = new GuestReviewRepository();
        }

        public bool IsGuestReviewed(AccommodationReservation accommodationReservation)
        {
            return guestReviewRepository.HasReview(accommodationReservation);
        }
    }
}
