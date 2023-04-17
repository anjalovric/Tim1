using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class OwnerReviewRepository : IOwnerReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/ownerAndAccommodationReviews.csv";

        private readonly Serializer<OwnerReview> _serializer;

        private List<OwnerReview> _reviews;

        public OwnerReviewRepository()
        {
            _serializer = new Serializer<OwnerReview>();
            _reviews = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _reviews = _serializer.FromCSV(FilePath);
            if (_reviews.Count < 1)
            {
                return 1;
            }
            return _reviews.Max(c => c.Id) + 1;
        }

        public bool HasReview(AccommodationReservation reservation)
        {
            _reviews = _serializer.FromCSV(FilePath);
            return _reviews.Find(n => n.Reservation.Id == reservation.Id) != null;
        }

        public void Add(OwnerReview review)
        {
            review.Id = NextId();
            _reviews.Add(review);
            _serializer.ToCSV(FilePath, _reviews);
        }

        public List<OwnerReview> GetAll()
        {
            return _reviews;
        }

        public OwnerReview GetById(int id)
        {
            return _reviews.Find(n => n.Id == id);
        }
    }
}
