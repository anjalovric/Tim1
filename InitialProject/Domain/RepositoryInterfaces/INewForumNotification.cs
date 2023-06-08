using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface INewForumNotificationRepository : IGenericRepository<NewForumNotification>
    {
        public void Add(NewForumNotification notification);
        public NewForumNotification GetByOwnerAndForum(Owner owner, Location location);
        public void Delete(NewForumNotification notification);
    }
}
