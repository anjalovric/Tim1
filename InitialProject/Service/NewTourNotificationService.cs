using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace InitialProject.Service
{
    public class NewTourNotificationService
    {
        private IGuest2NotificationRepository notificationRepository = Injector.CreateInstance<IGuest2NotificationRepository>();
        public NewTourNotificationService()
        {
        }

        public void Save(Guest2Notification notification)
        {
            notificationRepository.Save(notification);
        }

        public void Delete(Guest2Notification notification)
        {
            notificationRepository.Delete(notification);
        }
        public List<Guest2Notification> GetAll()
        {
            return notificationRepository.GetAll();
        }
        public ObservableCollection<Guest2Notification> GetByGuestId(int id)
        {
            return notificationRepository.GetByGuestId(id);
        }
        public bool IsRequestAccepted(Guest2 guest)
        {
            List<Guest2Notification> notifications = notificationRepository.GetAll();
            return notifications.Find(n => n.Type.Equals(Guest2NotificationType.REQUEST_ACCEPTED) && n.Guest2.Id == guest.Id) != null;
        }
        public bool IsConfirmPresence(Guest2 guest)
        {
            List<Guest2Notification> notifications = notificationRepository.GetAll();
            return notifications.Find(n => n.Type.Equals(Guest2NotificationType.CONFIRM_PRESENCE) && n.Guest2.Id == guest.Id) != null;
        }
    }
}
