using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.View;

namespace InitialProject.Repository
{
    public class CancelledAccommodationReservationRepository : ICancelledAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/cancelledAccommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _cancelledAccommodationReservations;

        public CancelledAccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _cancelledAccommodationReservations = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _cancelledAccommodationReservations = _serializer.FromCSV(FilePath);
            if (_cancelledAccommodationReservations.Count < 1)
            {
                return 1;
            }
            return _cancelledAccommodationReservations.Max(c => c.Id) + 1;
        }
        
        public void Add(AccommodationReservation reservation)
        {
            AccommodationReservation cancelled = new AccommodationReservation(reservation.Guest, reservation.Accommodation, reservation.Arrival, reservation.Departure);
            cancelled.Id = reservation.Id;
            _cancelledAccommodationReservations.Add(cancelled);
            _serializer.ToCSV(FilePath, _cancelledAccommodationReservations);
        }
        public List<AccommodationReservation> GetAll()
        {
            return _cancelledAccommodationReservations;
        }

        public AccommodationReservation GetById(int id)
        {
            return _cancelledAccommodationReservations.Find(n => n.Id == id);
        }
        public int GetMaxId()
        {
            int id = 0;
            foreach (AccommodationReservation reservation in _cancelledAccommodationReservations)
            {
                if (reservation.Id > id)
                    id = reservation.Id;
            }
            return id;
        }
    }
}
