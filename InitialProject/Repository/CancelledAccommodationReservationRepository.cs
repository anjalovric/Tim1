using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class CancelledAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/cancelledAccommodationReservations.csv";

        private readonly Serializer<CancelledAccommodationReservation> _serializer;

        private List<CancelledAccommodationReservation> _cancelledAccommodationReservations;

        public CancelledAccommodationReservationRepository()
        {
            _serializer = new Serializer<CancelledAccommodationReservation>();
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

        public void Add(CancelledAccommodationReservation reservation)
        {
            reservation.Id = NextId();
            _cancelledAccommodationReservations.Add(reservation);
            _serializer.ToCSV(FilePath, _cancelledAccommodationReservations);
        }

        public List<CancelledAccommodationReservation> GetAll()
        {
            return _cancelledAccommodationReservations;
        }

        public CancelledAccommodationReservation GetById(int id)
        {
            return _cancelledAccommodationReservations.Find(n => n.Id == id);
        }




    }
}
