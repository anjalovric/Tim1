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
    public class OwnerNotificationRepository : IOwnerNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/ownerNotifications.csv";

        private readonly Serializer<OwnerNotification> _serializer;

        private List<OwnerNotification> _notifications;

        public OwnerNotificationRepository()
        {
            _serializer = new Serializer<OwnerNotification>();
            _notifications = _serializer.FromCSV(FilePath);
        }

        public List<OwnerNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public OwnerNotification Add(OwnerNotificationType type, Owner owner)
        {
            OwnerNotification notification = new OwnerNotification();
            notification.Id = NextId();
            notification.Type = type;
            notification.Owner = owner;
            _notifications = _serializer.FromCSV(FilePath);
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

        public void Delete(OwnerNotificationType type, Owner owner)
        {
            _notifications = _serializer.FromCSV(FilePath);
            OwnerNotification founded = _notifications.Find(c => c.Type.Equals(type) && c.Owner.Id == owner.Id);
            _notifications.Remove(founded);
            _serializer.ToCSV(FilePath, _notifications);
        }

        public OwnerNotification GetById(int id)
        {
            return _notifications.Find(n => n.Id == id);
        }
    }
}
