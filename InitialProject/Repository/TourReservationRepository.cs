using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace InitialProject.Repository
{
    public class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReservations.csv";
        private readonly Serializer<TourReservation> _serializer;
        private List<TourReservation> _tourReservations;
        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            _tourReservations = new List<TourReservation>();
            _tourReservations = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            if (_tourReservations.Count < 1)
            {
                return 1;
            }
            return _tourReservations.Max(c => c.Id) + 1;
        }
        public TourReservation Save(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            _tourReservations = _serializer.FromCSV(FilePath);
            _tourReservations.Add(tourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
            return tourReservation;
        }
        public void Delete(TourReservation tourReservation)
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            TourReservation foundedTourReservation = _tourReservations.Find(c => c.Id == tourReservation.Id);
            _tourReservations.Remove(foundedTourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
        }
        public TourReservation Update(TourReservation tourReservation,int guestsNumber,Boolean withVoucher)
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            TourReservation current = _tourReservations.Find(c => c.Id == tourReservation.Id);
            current.Id = tourReservation.Id;
            current.CurrentGuestsNumber = guestsNumber;
            current.TourInstanceId = tourReservation.TourInstanceId;
            current.GuestId = tourReservation.GuestId;
            current.WithVaucher = withVoucher;
            _serializer.ToCSV(FilePath, _tourReservations);
            return tourReservation;
        }
        public void Add(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            _tourReservations.Add(tourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
        }
        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public List<TourReservation> GetReservationsForTourInstance(int tourInstanceId)
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            List<TourReservation> list = new List<TourReservation>();
            foreach (TourReservation tour in _tourReservations)
            {
                if(tour.TourInstanceId==tourInstanceId)
                    list.Add(tour);
            }
            return list;
        }
        public TourReservation GetById(int id)
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            return _tourReservations.Find(n => n.Id == id);
        }
        public List<TourReservation> GetReservationsForTour(TourInstance selected)
        {
            List<TourReservation> tourReservations = GetAll();
            List<TourReservation> availableReservations = new List<TourReservation>();
            foreach (TourReservation reservation in tourReservations)
            {
                if (reservation.TourInstanceId == selected.Id)
                {
                    availableReservations.Add(reservation);
                }
            }
            return availableReservations;
        }
    }
}
