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
    public class Guest2NotificationRepository:IGuest2NotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/guest2Notifications.csv";

        private readonly Serializer<Guest2Notification> _serializer;

        private List<Guest2Notification> _notifications;

        public Guest2NotificationRepository()
        {
            _serializer = new Serializer<Guest2Notification>();
            _notifications = _serializer.FromCSV(FilePath);
        }

        public List<Guest2Notification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Guest2Notification Save(Guest2Notification notification)
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

        public void Delete(Guest2Notification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            Guest2Notification founded = _notifications.Find(c => c.Id==notification.Id);
            _notifications.Remove(founded);
            _serializer.ToCSV(FilePath, _notifications);
        }

        public Guest2Notification GetById(int id)
        {
            return _notifications.Find(n => n.Id == id);
        }
        public ObservableCollection<Guest2Notification> GetByGuestId(int id)
        {
            _notifications = GetAll();
            ObservableCollection<Guest2Notification> notifications = new ObservableCollection<Guest2Notification>();
            foreach (Guest2Notification notification in _notifications)
            {
                if (notification.Guest2.Id == id)
                {
                    notifications.Add(notification);
                }
            }
            return notifications;
        }
        public void Update(Guest2Notification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            Guest2Notification current = _notifications.Find(c => c.Id == notification.Id);
            int index = _notifications.IndexOf(current);
            _notifications.Remove(current);
            _notifications.Insert(index, notification);
            _serializer.ToCSV(FilePath, _notifications);
        }
    }
}
