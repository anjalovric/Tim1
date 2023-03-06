using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class AccommodationImageRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationImages.csv";

        private readonly Serializer<AccommodationImage> _serializer;

        private List<AccommodationImage> _accommodationImages;

        public AccommodationImageRepository()
        {
            _serializer = new Serializer<AccommodationImage>();
            _accommodationImages = _serializer.FromCSV(FilePath);
        }
    }
}
