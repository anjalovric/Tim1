using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class TourReviewImageRepository:ITourReviewImageRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReviewImages.csv";

        private readonly Serializer<TourReviewImage> _serializer;

        private List<TourReviewImage> _tourReviewImages;

        public TourReviewImageRepository()
        {
            _serializer = new Serializer<TourReviewImage>();
            _tourReviewImages = _serializer.FromCSV(FilePath);
        }

        public List<TourReviewImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourReviewImage Save(TourReviewImage review)
        {
            review.Id = NextId();
            _tourReviewImages = _serializer.FromCSV(FilePath);
            _tourReviewImages.Add(review);
            _serializer.ToCSV(FilePath, _tourReviewImages);
            return review;
        }
        public int NextId()
        {
            _tourReviewImages = _serializer.FromCSV(FilePath);
            if (_tourReviewImages.Count < 1)
            {
                return 1;
            }
            return _tourReviewImages.Max(c => c.Id) + 1;
        }

        public void Delete(TourReviewImage review)
        {
            _tourReviewImages = _serializer.FromCSV(FilePath);
            TourReviewImage founded = _tourReviewImages.Find(c => c.Id == review.Id);
            _tourReviewImages.Remove(founded);
            _serializer.ToCSV(FilePath, _tourReviewImages);
        }
        public TourReviewImage Update(TourReviewImage review,int id)
        {
            _tourReviewImages = _serializer.FromCSV(FilePath);
            TourReviewImage current = _tourReviewImages.Find(c => c.Id == review.Id);
            int index = _tourReviewImages.IndexOf(current);
            _tourReviewImages.Remove(current);
            current.GuideAndTourReviewId = id;
            _tourReviewImages.Insert(index, current);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tourReviewImages);
            return review;
        }
        public List<TourReviewImage> GetByReviewId(int reviewId)
        {
            List<TourReviewImage> images= new List<TourReviewImage>();
            foreach(TourReviewImage image in _tourReviewImages)
            {
                if(image.GuideAndTourReviewId==reviewId) images.Add(image);
            }
            return images;
        }
        public TourReviewImage GetById(int id)
        {
            return _tourReviewImages.Find(x  => x.Id == id);
        }
    }
}
