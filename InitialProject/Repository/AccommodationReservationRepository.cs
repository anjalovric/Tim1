using System.Collections.Generic;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
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

        public int NextId()
        {
            CancelledAccommodationReservationRepository cancelledAccommodationReservationRepository = new CancelledAccommodationReservationRepository();
            List<AccommodationReservation> storedCancelledReservations = cancelledAccommodationReservationRepository.GetAll();
            if (storedCancelledReservations.Count < 1 && _accommodationReservations.Count < 1)
                return 1;
            int cancelledId = cancelledAccommodationReservationRepository.GetMaxId();
            int id = GetMaxId();
            if (cancelledId > id)
                return cancelledId + 1;
            return id + 1;
        }
        public void Add(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
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

        

        public AccommodationReservation GetById(int id)
        {
            return _accommodationReservations.Find(c => c.Id == id);
        }

       

       private int GetMaxId()
       {
           int id = 0;
           foreach(AccommodationReservation reservation in _accommodationReservations)
           {
               if(reservation.Id>id)
                   id = reservation.Id;
           }
           return id;
       }


    }
}
