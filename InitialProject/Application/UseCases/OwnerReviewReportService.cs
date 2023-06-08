using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.APPLICATION.UseCases
{
    public class OwnerReviewReportService
    {
        private AccommodationService accommodationService;
        private OwnerReviewService ownerReviewService;
        public OwnerReviewReportService()
        {
            accommodationService = new AccommodationService();
            ownerReviewService = new OwnerReviewService();
        }

        public List<Accommodation> GetAllAccommodationByOwner(Owner owner)
        {
            return accommodationService.GetAllByOwner(owner);
        }

        public double GetAverageCleanlinessByAccommodation(Accommodation accommodation)
        {
            double sum = 0;
            List<OwnerReview> allReviews = ownerReviewService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodation.Id);
            foreach(OwnerReview ownerReview in allReviews)
            {
                sum += ownerReview.Cleanliness;
            }
            if(allReviews.Count()>0)
                return sum / allReviews.Count();
            return 0;
        }

        public double GetAverageCorrectnessByAccommodation(Accommodation accommodation)
        {
            double sum = 0;
            List<OwnerReview> allReviews = ownerReviewService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodation.Id);
            foreach (OwnerReview ownerReview in allReviews)
            {
                sum += ownerReview.Correctness;
            }
            if (allReviews.Count() > 0)
                return sum / allReviews.Count();
            return 0;
        }

        public double GetNumberOfReviews(Owner owner)
        {
            return ownerReviewService.GetNumberOfReviewsByOwner(owner);
        }

        public double GetAverageRate(Owner owner)
        {
            return ownerReviewService.CalculateAverageRateByOwner(owner);
        }
    }
}
