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
    public class AccommodationImageRepository : IAccommodationImageRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationImages.csv";

        private readonly Serializer<AccommodationImage> _serializer;

        private List<AccommodationImage> _accommodationImages;

        public AccommodationImageRepository()
        {
            _serializer = new Serializer<AccommodationImage>();
            _accommodationImages = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _accommodationImages = _serializer.FromCSV(FilePath);
            if (_accommodationImages.Count < 1)
            {
                return 1;
            }
            return _accommodationImages.Max(c => c.Id) + 1;
        }

        public int Add(AccommodationImage image)
        {
            image.Id = NextId();
            _accommodationImages.Add(image);
            _serializer.ToCSV(FilePath, _accommodationImages);
            return image.Id;
        }

        public List<AccommodationImage> GetAll()
        {
            return _accommodationImages;
        }

        public List<string> GetUrlByAccommodationId(int id)
        {
            List<string> images = new List<string>();

            foreach (AccommodationImage image in _accommodationImages)
            {
                if (image.Accommodation.Id == id)
                    images.Add(image.Url);
            }
            return images;
        }

        public AccommodationImage GetById(int id)
        {
            return _accommodationImages.Find(n => n.Id == id);
        }
    }
}
