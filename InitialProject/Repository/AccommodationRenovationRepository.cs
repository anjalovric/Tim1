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
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovations.csv";

        private readonly Serializer<AccommodationRenovation> _serializer;

        private List<AccommodationRenovation> _renovations;

        public AccommodationRenovationRepository()
        {
            _serializer = new Serializer<AccommodationRenovation>();
            _renovations = _serializer.FromCSV(FilePath);
        }
        public List<AccommodationRenovation> GetAll()
        {
            return _renovations;
        }

        public AccommodationRenovation GetById(int id)
        {
            return _renovations.Find(n => n.Id == id);
        }

        public int NextId()
        {
            _renovations = _serializer.FromCSV(FilePath);
            if (_renovations.Count < 1)
            {
                return 1;
            }
            return _renovations.Max(c => c.Id) + 1;
        }

        public void Add(AccommodationRenovation renovation)
        {
            renovation.Id = NextId();
            _renovations.Add(renovation);
            _serializer.ToCSV(FilePath, _renovations);
        }

        public void Delete(AccommodationRenovation renovation)
        {
            _renovations = _serializer.FromCSV(FilePath);
            AccommodationRenovation founded = _renovations.Find(c => c.Id == renovation.Id);
            _renovations.Remove(founded);
            _serializer.ToCSV(FilePath, _renovations);
        }
    }
}
