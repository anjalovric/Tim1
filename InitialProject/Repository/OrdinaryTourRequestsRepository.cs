using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using InitialProject.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace InitialProject.Repository
{
    public class OrdinaryTourRequestsRepository:IOrdinaryTourRequestsRepository
    {
        private const string FilePath = "../../../Resources/Data/ordinaryRequests.csv";


        private readonly Serializer<OrdinaryTourRequests> _serializer;


        private List<OrdinaryTourRequests> _requests;


        public OrdinaryTourRequestsRepository()
        {
            _serializer = new Serializer<OrdinaryTourRequests>();

            _requests = _serializer.FromCSV(FilePath);
        }
        public List<OrdinaryTourRequests> GetAll()
        {
            return _serializer.FromCSV(FilePath); ;
        }
        public OrdinaryTourRequests Save(OrdinaryTourRequests request)
        {
            request.Id = NextId();
            _requests = _serializer.FromCSV(FilePath);
            _requests.Add(request);
            _serializer.ToCSV(FilePath, _requests);
            return request;
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
        public void Delete(OrdinaryTourRequests request)
        {
            _requests = _serializer.FromCSV(FilePath);
            OrdinaryTourRequests founded = _requests.Find(c => c.Id == request.Id);
            _requests.Remove(founded);
            _serializer.ToCSV(FilePath, _requests);
        }
        public OrdinaryTourRequests Update(OrdinaryTourRequests request)
        {
            _requests = _serializer.FromCSV(FilePath);
            OrdinaryTourRequests current = _requests.Find(c => c.Id == request.Id);
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, request);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _requests);
            return request;
        }
        
        public OrdinaryTourRequests GetById(int id)
        {
            return _requests.Find(c => c.Id == id);
        }
        public List<OrdinaryTourRequests> GetByGuestId(int id)
        {
            _requests = GetAll();
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            Guest2Service guest2Service = new Guest2Service();
            foreach(OrdinaryTourRequests request in _requests)
            {
                if (request.GuestId == id)
                {
                    ordinaryTourRequests.Add(request);
                }
            }
            return ordinaryTourRequests;
        }
    }
}
