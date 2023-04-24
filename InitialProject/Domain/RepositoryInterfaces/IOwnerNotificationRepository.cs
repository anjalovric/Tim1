using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IOwnerNotificationRepository : IGenericRepository<OwnerNotification>
    {
        public OwnerNotification Add(OwnerNotificationType type, Owner owner);
        public void Delete(OwnerNotificationType type, Owner owner);
    }
}
