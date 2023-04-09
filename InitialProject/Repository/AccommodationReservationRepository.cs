using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;
using Org.BouncyCastle.Asn1.Ocsp;

namespace InitialProject.Repository
{
    public class AccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = new List<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationReservation> GetAll()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            return _accommodationReservations;
        }

         public int NextId()        //ne koristim je, sta da uradim s njom?
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }
        public void Add(AccommodationReservation accommodationReservation, int nextId)
        {
            accommodationReservation.Id = nextId;
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation founded = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            _accommodationReservations.Remove(founded);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public AccommodationReservation Update(AccommodationReservation newReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accommodationReservations.Find(c => c.Id == newReservation.Id);
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, newReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return newReservation;
        }

    }
}
