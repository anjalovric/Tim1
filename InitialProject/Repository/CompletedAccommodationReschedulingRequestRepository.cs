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
    public class CompletedAccommodationReschedulingRequestRepository : ICompletedAccommodationReschedulingRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/completedAccommodationReschedulingRequests.csv";

        private readonly Serializer<CompletedAccommodationReschedulingRequest> _serializer;

        private List<CompletedAccommodationReschedulingRequest> _completedRequests;

        public CompletedAccommodationReschedulingRequestRepository()
        {
            _serializer = new Serializer<CompletedAccommodationReschedulingRequest>();
            _completedRequests = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _completedRequests = _serializer.FromCSV(FilePath);
            if (_completedRequests.Count < 1)
            {
                return 1;
            }
            return _completedRequests.Max(c => c.Id) + 1;
        }

        public void Add(CompletedAccommodationReschedulingRequest request)
        {
            request.Id = NextId();
            _completedRequests.Add(request);
            _serializer.ToCSV(FilePath, _completedRequests);
        }

        public List<CompletedAccommodationReschedulingRequest> GetAll()
        {
            return _completedRequests;
        }

        public CompletedAccommodationReschedulingRequest GetById(int id)
        {
            return _completedRequests.Find(n => n.Id == id);
        }
    }
}
