using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.Service;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InitialProject.Repository
{
    public class NewTourNotificationRepository:INewTourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/guest2Notifications.csv";

        private readonly Serializer<NewTourNotification> _serializer;

        private List<NewTourNotification> _notifications;

        public NewTourNotificationRepository()
        {
            _serializer = new Serializer<NewTourNotification>();
            _notifications = _serializer.FromCSV(FilePath);
        }

        public List<NewTourNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public NewTourNotification Save(NewTourNotification notification)
        {
            notification.Id = NextId();
            _notifications.Add(notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }

        public int NextId()
        {
            _notifications = _serializer.FromCSV(FilePath);
            if (_notifications.Count < 1)
            {
                return 1;
            }
            return _notifications.Max(c => c.Id) + 1;
        }

        public void Delete(NewTourNotification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            NewTourNotification founded = _notifications.Find(c => c.Id==notification.Id);
            _notifications.Remove(founded);
            _serializer.ToCSV(FilePath, _notifications);
        }

        public NewTourNotification GetById(int id)
        {
            return _notifications.Find(n => n.Id == id);
        }
        public List<NewTourNotification> GetByGuestId(int id)
        {
            _notifications = GetAll();
            List<NewTourNotification> notifications = new List<NewTourNotification>();
            foreach (NewTourNotification notification in _notifications)
            {
                if (notification.Guest2.Id == id)
                {
                    notifications.Add(notification);
                }
            }
            return notifications;
        }
        public void Update(NewTourNotification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            NewTourNotification current = _notifications.Find(c => c.Id == notification.Id);
            int index = _notifications.IndexOf(current);
            _notifications.Remove(current);
            _notifications.Insert(index, notification);
            _serializer.ToCSV(FilePath, _notifications);
        }
    }
}
