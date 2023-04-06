using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationReviewImageService
    {
        private AccommodationReviewImageRepository accommodationReviewImageRepository;

        public AccommodationReviewImageService()
        {
            accommodationReviewImageRepository = new AccommodationReviewImageRepository();
        }

        public List<AccommodationReviewImage> GetAll()
        {
            return accommodationReviewImageRepository.GetAll();
        }
        public void Add(AccommodationReviewImage image)
        {
            accommodationReviewImageRepository.Add(image);
        }

        public void Delete(AccommodationReviewImage image)
        {
            accommodationReviewImageRepository.Delete(image);
        }

    }
}
