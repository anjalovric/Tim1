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
    public class AccommodationTypeRepository : IAccommodationTypeRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationTypes.csv";

        private readonly Serializer<AccommodationType> _serializer;

        private List<AccommodationType> _accommodationTypes;

        public AccommodationTypeRepository()
        {
            _serializer = new Serializer<AccommodationType>();
            _accommodationTypes = new List<AccommodationType>();
            _accommodationTypes = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationType> GetAll()
        {
            return _accommodationTypes;
        }

        public AccommodationType GetById(int id)
        {
            return _accommodationTypes.Find(n => n.Id == id);
        }

        public int NextId()
        {
            throw new NotImplementedException();
        }
    }
}
