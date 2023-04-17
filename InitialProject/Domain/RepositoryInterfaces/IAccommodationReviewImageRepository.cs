using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationReviewImageRepository : IGenericRepository<AccommodationReviewImage>
    {
        public void Add(AccommodationReviewImage accommodationReviewImage);

        public void Delete(AccommodationReviewImage accommodationReviewImage);
    }
}
