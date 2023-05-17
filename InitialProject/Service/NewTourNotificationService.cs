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
        private INewTourNotificationRepository newTourNotificationRepository = Injector.CreateInstance<INewTourNotificationRepository>();
        public NewTourNotificationService()
        {
        }

        public void Save(NewTourNotification notification)
        {
            newTourNotificationRepository.Save(notification);
        }

        public void Delete(NewTourNotification notification)
        {
            newTourNotificationRepository.Delete(notification);
        }
        public List<NewTourNotification> GetAll()
        {
            return newTourNotificationRepository.GetAll();
        }
        public List<NewTourNotification> GetByGuestId(int id)
        {
            return newTourNotificationRepository.GetByGuestId(id);
        }
        public void Update(NewTourNotification notification)
        {
            newTourNotificationRepository.Update(notification);
        }
    }
}
