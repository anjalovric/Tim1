using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourReviewImageService
    {
        private ITourReviewImageRepository tourReviewImageRepository=Injector.CreateInstance<ITourReviewImageRepository>();
        public TourReviewImageService() { }
        public List<TourReviewImage> GetAll()
        {
            return tourReviewImageRepository.GetAll();
        }
        public void Save(TourReviewImage image)
        {
            tourReviewImageRepository.Save(image);
        }
        public void Update(TourReviewImage image,int id)
        {
            tourReviewImageRepository.Update(image,id);
        }
        public void Delete(TourReviewImage image)
        {
            tourReviewImageRepository.Delete(image);
        }
        public List<TourReviewImage> GetByReviewId(int reviewId)
        {
           return tourReviewImageRepository.GetByReviewId(reviewId);
        }
    }
}
