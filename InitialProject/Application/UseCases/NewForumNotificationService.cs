using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Domain.Model;

namespace InitialProject.APPLICATION.UseCases
{
    public class NewForumNotificationService
    {
        private INewForumNotificationRepository forumNotificationRepository = Injector.CreateInstance<INewForumNotificationRepository>();
        public NewForumNotificationService()
        {

        }

        public NewForumNotification GetByOwnerAndLocation(Owner owner, Location location)
        {
            return forumNotificationRepository.GetByOwnerAndForum(owner, location);
        }

        public void Add(Owner owner, Location location)
        {
            if (forumNotificationRepository.GetByOwnerAndForum(owner, location) == null)
            {
                NewForumNotification notification = new NewForumNotification();
                notification.Owner = owner;
                notification.Location = location;
                forumNotificationRepository.Add(notification);
            }
        }

        public void Delete(Owner owner, Location location)
        {
            NewForumNotification notification = new NewForumNotification();
            notification.Owner = owner;
            notification.Location = location;
            forumNotificationRepository.Delete(notification);
        }
    }
}
