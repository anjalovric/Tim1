using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IOwnerReviewRepository : IGenericRepository<OwnerReview>
    {
        public bool HasReview(AccommodationReservation reservation);
        public void Add(OwnerReview review);
    }
}
