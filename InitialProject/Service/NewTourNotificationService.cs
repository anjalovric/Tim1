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
    public class NewTourNotificationService
    {
        private INewTourNotificationRepository notificationRepository = Injector.CreateInstance<INewTourNotificationRepository>();
        public NewTourNotificationService()
        {
        }

        public void Save(NewTourNotification notification)
        {
            notificationRepository.Save(notification);
        }

        public void Delete(NewTourNotification notification)
        {
            notificationRepository.Delete(notification);
        }
        public List<NewTourNotification> GetAll()
        {
            return notificationRepository.GetAll();
        }
        public ObservableCollection<NewTourNotification> GetByGuestId(int id)
        {
            return notificationRepository.GetByGuestId(id);
        }
        public void Update(NewTourNotification notification)
        {
            notificationRepository.Update(notification);
        }
    }
}
