using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class RequestNotificationRepository:IRequestNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/requestNotifications.csv";

        private readonly Serializer<RequestNotification> _serializer;

        private List<RequestNotification> _reviews;

        public RequestNotificationRepository()
        {
            _serializer = new Serializer<RequestNotification>();
            _reviews = _serializer.FromCSV(FilePath);
        }
        public RequestNotification Update(RequestNotification request)
        {
            _reviews = _serializer.FromCSV(FilePath);
            RequestNotification current = _reviews.Find(c => c.Id == request.Id);
            int index = _reviews.IndexOf(current);
            _reviews.Remove(current);
            _reviews.Insert(index, request);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _reviews);
            return request;
        }
        public List<RequestNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public RequestNotification Save(RequestNotification request)
        {
            request.Id = NextId();
            _reviews = _serializer.FromCSV(FilePath);
            _reviews.Add(request);
            _serializer.ToCSV(FilePath, _reviews);
            return request;
        }

        public int NextId()
        {
            _reviews = _serializer.FromCSV(FilePath);
            if (_reviews.Count < 1)
            {
                return 1;
            }
            return _reviews.Max(c => c.Id) + 1;
        }

        public RequestNotification GetById(int id)
        {
            return _reviews.Find(c => c.Id == id);
        }
    }
}
