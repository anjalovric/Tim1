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
using System.Printing;
using System.DirectoryServices.ActiveDirectory;
using InitialProject.Serializer;

namespace InitialProject.Service
{
    public class Guest2NotificationService
    {
        private IGuest2NotificationRepository notificationRepository = Injector.CreateInstance<IGuest2NotificationRepository>();
        public Guest2NotificationService()
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
        public void Update(Guest2Notification notification)
        {
            notificationRepository.Update(notification);
        }
    }
}
