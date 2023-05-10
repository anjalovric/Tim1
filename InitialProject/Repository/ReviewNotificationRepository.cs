using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class ReviewNotificationRepository:IReviewNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/reviewNotifications.csv";

        private readonly Serializer<GuideAndTourReviewNotification> _serializer;

        private List<GuideAndTourReviewNotification> _reviews;

        public ReviewNotificationRepository()
        {
            _serializer = new Serializer<GuideAndTourReviewNotification>();
            _reviews = _serializer.FromCSV(FilePath);
        }
        public GuideAndTourReviewNotification Update(GuideAndTourReviewNotification voucher)
        {
            _reviews = _serializer.FromCSV(FilePath);
            GuideAndTourReviewNotification current = _reviews.Find(c => c.Id == voucher.Id);
            int index = _reviews.IndexOf(current);
            _reviews.Remove(current);
            _reviews.Insert(index, voucher);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _reviews);
            return voucher;
        }
        public List<GuideAndTourReviewNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GuideAndTourReviewNotification Save(GuideAndTourReviewNotification review)
        {
            review.Id = NextId();
            _reviews = _serializer.FromCSV(FilePath);
            _reviews.Add(review);
            _serializer.ToCSV(FilePath, _reviews);
            return review;
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

        public GuideAndTourReviewNotification GetById(int id)
        {
            return _reviews.Find(c => c.Id == id);
        }
    }
}
