using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class GuestAverageReviewService
    {
        private List<GuestReview> guestReviews;
        public GuestAverageReviewService()
        {
            GuestReviewService guestReviewService = new GuestReviewService();
            guestReviews = new List<GuestReview>(guestReviewService.GetAll());
        }
        public double GetAverageCleanlinessReview(Guest1 guest1)
        {
            double sumCleanlinessReview = 0;
            int counter = 0;

            foreach (GuestReview guestReview in guestReviews)
            {
                if (guestReview.Reservation.Guest.Id == guest1.Id)
                {
                    sumCleanlinessReview += guestReview.Cleanliness;
                    counter++;
                }
            }
            if (counter == 0)
                return 1;
            return sumCleanlinessReview / counter;
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

        public int GetAverageRating(Guest1 guest1)
        {
            return Convert.ToInt32((GetAverageCleanlinessReview(guest1) + GetAverageFollowingRulesReview(guest1)) / 2);
        }
    }
}
