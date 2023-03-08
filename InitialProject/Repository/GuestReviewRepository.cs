using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class GuestReviewRepository
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
    }
}
