using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class AccommodationReviewImageRepository : IAccommodationReviewImageRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReviewImages.csv";

        private readonly Serializer<AccommodationReviewImage> _serializer;

        private List<AccommodationReviewImage> _accommodationReviewImages;

        public AccommodationReviewImageRepository()
        {
            _serializer = new Serializer<AccommodationReviewImage>();
            _accommodationReviewImages = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationReviewImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }



        public void Add(AccommodationReviewImage image)
        {
            image.Id = NextId();
            _accommodationReviewImages.Add(image);
            _serializer.ToCSV(FilePath, _accommodationReviewImages);
        }

        public int NextId()
        {
            _accommodationReviewImages = _serializer.FromCSV(FilePath);
            if (_accommodationReviewImages.Count < 1)
            {
                return 1;
            }
            return _accommodationReviewImages.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationReviewImage image)
        {
            _accommodationReviewImages = _serializer.FromCSV(FilePath);
            AccommodationReviewImage founded = _accommodationReviewImages.Find(c => c.Id == image.Id);
            _accommodationReviewImages.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationReviewImages);
        }

        public AccommodationReviewImage GetById(int id)
        {
            return _accommodationReviewImages.Find(n => n.Id == id);
        }
    }
}
