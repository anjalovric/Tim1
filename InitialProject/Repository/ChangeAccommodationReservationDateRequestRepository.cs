using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class ChangeAccommodationReservationDateRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/changesAccommodationReservationDatesRequests.csv";

        private readonly Serializer<ChangeAccommodationReservationDateRequest> _serializer;

        private List<ChangeAccommodationReservationDateRequest> _requests;

        public ChangeAccommodationReservationDateRequestRepository()
        {
            _serializer = new Serializer<ChangeAccommodationReservationDateRequest>();
            _requests = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _requests = _serializer.FromCSV(FilePath);
            if (_requests.Count < 1)
            {
                return 1;
            }
            return _requests.Max(c => c.Id) + 1;
        }

        public void Add(ChangeAccommodationReservationDateRequest request)
        {
            request.Id = NextId();
            _requests.Add(request);
            _serializer.ToCSV(FilePath, _requests);
        }

        public List<ChangeAccommodationReservationDateRequest> GetAll()
        {
            return _requests;
        }

        public ChangeAccommodationReservationDateRequest GetById(int id)
        {
            return _requests.Find(n => n.Id == id);
        }
    }
}
