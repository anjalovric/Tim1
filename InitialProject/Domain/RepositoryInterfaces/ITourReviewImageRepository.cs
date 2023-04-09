using InitialProject.Domain.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourReviewImageRepository:IGenericRepository<TourReviewImage>
    {
        public TourReviewImage Save(TourReviewImage review);

        public void Delete(TourReviewImage review);

        public TourReviewImage Update(TourReviewImage review, int id);

        public List<TourReviewImage> GetByReviewId(int reviewId);
    }
}
