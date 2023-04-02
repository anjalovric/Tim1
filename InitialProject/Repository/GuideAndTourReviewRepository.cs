using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class GuideAndTourReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/guideAndTourReview.csv";

        private readonly Serializer<GuideAndTourReview> _serializer;

        private List<GuideAndTourReview> _reviews;

        public GuideAndTourReviewRepository()
        {
            _serializer = new Serializer<GuideAndTourReview>();
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
        public bool HasReview(TourReservation reservation)
        {
            _reviews = _serializer.FromCSV(FilePath);
            return _reviews.Find(n => n.Reservation.Id == reservation.Id) != null;
        }

        public void Save(GuideAndTourReview review)
        {
            review.Id = NextId();
            _reviews.Add(review);
            _serializer.ToCSV(FilePath, _reviews);
        }

        public List<GuideAndTourReview> GetAll()
        {
            return _reviews;
        }
    }
}
