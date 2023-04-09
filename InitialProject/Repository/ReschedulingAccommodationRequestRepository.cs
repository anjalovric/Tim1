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
    public class ReschedulingAccommodationRequestRepository : IReschedulingAccommodationRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/reschedulingAccommodationRequests.csv";

        private readonly Serializer<ReschedulingAccommodationRequest> _serializer;

        private List<ReschedulingAccommodationRequest> _requests;

        public ReschedulingAccommodationRequestRepository()
        {
            _serializer = new Serializer<ReschedulingAccommodationRequest>();
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

        public void Add(ReschedulingAccommodationRequest request)
        {
            request.Id = NextId();
            _requests.Add(request);
            _serializer.ToCSV(FilePath, _requests);
        }

        public List<ReschedulingAccommodationRequest> GetAll()
        {
            return _requests;
        }

        public ReschedulingAccommodationRequest GetById(int id)
        {
            return _requests.Find(n => n.Id == id);
        }
        public ReschedulingAccommodationRequest Update(ReschedulingAccommodationRequest updatedRequest)
        {
            _requests = _serializer.FromCSV(FilePath);
            ReschedulingAccommodationRequest current = _requests.Find(c => c.Id == updatedRequest.Id);
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, updatedRequest); 
            _serializer.ToCSV(FilePath, _requests);
            return updatedRequest;
        }
    }
}
