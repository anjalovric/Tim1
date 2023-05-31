using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class ComplexTourRequestsRepository: IComplexTourRequestsRepository
    {
        private const string FilePath = "../../../Resources/Data/complexRequests.csv";


        private readonly Serializer<ComplexTourRequests> _serializer;


        private List<ComplexTourRequests> _requests;


        public ComplexTourRequestsRepository()
        {
            _serializer = new Serializer<ComplexTourRequests>();

            _requests = _serializer.FromCSV(FilePath);
        }
        public List<ComplexTourRequests> GetAll()
        {
            return _serializer.FromCSV(FilePath); ;
        }
        public ComplexTourRequests Save(ComplexTourRequests request)
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
        public void Delete(ComplexTourRequests request)
        {
            _requests = _serializer.FromCSV(FilePath);
            ComplexTourRequests founded = _requests.Find(c => c.Id == request.Id);
            _requests.Remove(founded);
            _serializer.ToCSV(FilePath, _requests);
        }
        public ComplexTourRequests Update(ComplexTourRequests request)
        {
            _requests = _serializer.FromCSV(FilePath);
            ComplexTourRequests current = _requests.Find(c => c.Id == request.Id);
            int index = _requests.IndexOf(current);
            _requests.Remove(current);
            _requests.Insert(index, request);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _requests);
            return request;
        }

        public ComplexTourRequests GetById(int id)
        {
            return _requests.Find(c => c.Id == id);
        }
        public List<ComplexTourRequests> GetByGuestId(int id)
        {
            _requests = GetAll();
            List<ComplexTourRequests> complexTourRequests = new List<ComplexTourRequests>();
            Guest2Service guest2Service = new Guest2Service();
            foreach (ComplexTourRequests request in _requests)
            {
                if (request.Guest2.Id == id)
                {
                    complexTourRequests.Add(request);
                }
            }
            return complexTourRequests;
        }
    }
}
