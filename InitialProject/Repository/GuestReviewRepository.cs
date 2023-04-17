using System;
using System.Collections.Generic;
using System.Linq;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class GuestReviewRepository : IGuestReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/guestReviews.csv";

        private readonly Serializer<GuestReview> _serializer;

        private List<GuestReview> _reviews;

        public GuestReviewRepository()
        {
            _serializer = new Serializer<GuestReview>();
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
            return _reviews.Find(n => n.Reservation.Id == reservation.Id) != null;
        }

        public void Add(GuestReview review)
        {
            review.Id = NextId();
            _reviews.Add(review);
            _serializer.ToCSV(FilePath, _reviews);
        }

        public List<GuestReview> GetAll()
        {
            return _reviews;
        }
        public GuestReview GetById(int id)
        {
            return _reviews.Find(n => n.Id == id);
           
        }
    }
}
