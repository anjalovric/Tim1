using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class AccommodationAverageReviewService
    {
        private List<OwnerReview> ownerReviews;
        public AccommodationAverageReviewService()
        {
            OwnerReviewService ownerReviewService = new OwnerReviewService();
            ownerReviews = new List<OwnerReview>(ownerReviewService.GetAll());
        }
        public double GetAverageCleanlinessReview(Accommodation accommodation)
        {
            double sumCleanlinessReview = 0;
            int counter = 0;

            foreach (OwnerReview ownerReview in ownerReviews)
            {
                if (ownerReview.Reservation.Accommodation.Id == accommodation.Id)
                {
                    sumCleanlinessReview += ownerReview.Cleanliness;
                    counter++;
                }
            }
            if (counter == 0)
                return 1;
            return sumCleanlinessReview / counter;
        }

        public double GetAverageCorrectnessReview(Accommodation accommodation)
        {
            double sumCorrectnessReview = 0;
            int counter = 0;

            foreach (OwnerReview ownerReview in ownerReviews)
            {
                if (ownerReview.Reservation.Accommodation.Id == accommodation.Id)
                {
                    sumCorrectnessReview += ownerReview.Correctness;
                    counter++;
                }
            }
            if (counter == 0)
                return 1;
            return sumCorrectnessReview / counter;
        }
        public int GetReviewsNumberByAccommodation(Accommodation accommodation)
        {
            int counter = 0;

            foreach (OwnerReview ownerReview in ownerReviews)
            {
                if (ownerReview.Reservation.Accommodation.Id == accommodation.Id)
                {
                    counter++;
                }
            }
            return counter;
        }

        public int GetAverageRating(Accommodation accommodation)
        {
            return Convert.ToInt32((GetAverageCleanlinessReview(accommodation) + GetAverageCorrectnessReview(accommodation)) / 2);
        }
    }
}
