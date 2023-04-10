using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IGuestReviewRepository
    {
        public List<GuestReview> GetAll();
        public bool HasReview(AccommodationReservation reservation);
        public void Save(GuestReview guestReview);
        public int NextId();
        public GuestReview GetById(int id);

    }
}
